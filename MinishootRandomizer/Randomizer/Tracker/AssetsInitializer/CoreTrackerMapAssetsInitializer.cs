using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MinishootRandomizer;

public class CoreTrackerMapAssetsInitializer : ITrackerMapAssetsInitializer
{
    private static readonly List<string> _mapIdentifiers = new List<string>()
    {
        "StartingGrotto",
        "Overworld",
        "GreenGrotto",
        "ScarabTemple",
        "FamilyHouseCave",
        "Dungeon1",
        "CrystalGroveTemple",
        "Dungeon2",
        "DesertTemple",
        "Dungeon3",
        "Sewers",
        "SunkenTemple",
    };

    public IReadOnlyList<string> MapIdentifiers => _mapIdentifiers;

    private class MapAssetsState
    {
        public TrackerMap Map;
        public int NextMarkerDataIndex = 0;
        public bool ImageCreated = false;
        public List<GameObject> CreatedObjects = new List<GameObject>();

        public bool IsDone => Map != null
            && NextMarkerDataIndex >= Map.MarkerDatas.Count
            && ImageCreated;
    }

    private readonly ITrackerMapProvider _trackerMapProvider;
    private readonly IMarkerFactory _markerFactory;
    private readonly IObjectFinder _objectFinder;
    private readonly ISpriteProvider _spriteProvider;
    private readonly ILogger _logger;

    private readonly Dictionary<string, MapAssetsState> _states = new Dictionary<string, MapAssetsState>();
    private readonly List<string> _preloadOrder = new List<string>();
    private int _preloadCursor = 0;
    private GameObject _contentGameObject = null;

    public CoreTrackerMapAssetsInitializer(
        ITrackerMapProvider trackerMapProvider,
        IMarkerFactory markerFactory,
        IObjectFinder objectFinder,
        ISpriteProvider spriteProvider,
        ILogger logger = null
    )
    {
        _trackerMapProvider = trackerMapProvider;
        _markerFactory = markerFactory;
        _objectFinder = objectFinder;
        _spriteProvider = spriteProvider;
        _logger = logger ?? new NullLogger();
        BuildPreloadOrder();
    }

    public bool IsInitialized(TrackerMap map)
    {
        if (map == null)
        {
            return false;
        }
        return _states.TryGetValue(map.Identifier, out MapAssetsState state) && state.IsDone;
    }

    public bool HasPendingUnits()
    {
        while (_preloadCursor < _preloadOrder.Count)
        {
            MapAssetsState state = GetOrCreateState(_preloadOrder[_preloadCursor]);
            if (state != null && !state.IsDone)
            {
                return true;
            }
            _preloadCursor++;
        }

        return false;
    }

    public bool ProcessNextUnit()
    {
        if (!HasPendingUnits())
        {
            return false;
        }

        MapAssetsState state = GetOrCreateState(_preloadOrder[_preloadCursor]);
        ProcessUnit(state);
        if (state.IsDone)
        {
            _preloadCursor++;
        }

        return true;
    }

    public void InitializeNow(TrackerMap map)
    {
        if (map == null)
        {
            return;
        }

        MapAssetsState state = GetOrCreateState(map.Identifier);
        while (state != null && !state.IsDone)
        {
            ProcessUnit(state);
        }
    }

    public void Reset()
    {
        // The tracker UI objects survive a save close (they live on persistent canvases):
        // destroy the objects we created before forgetting about them, otherwise the next
        // session would create duplicates.
        foreach (MapAssetsState state in _states.Values)
        {
            foreach (GameObject gameObject in state.CreatedObjects)
            {
                if (gameObject != null)
                {
                    Object.Destroy(gameObject);
                }
            }
            state.CreatedObjects.Clear();
        }

        _states.Clear();
        _preloadCursor = 0;
        _contentGameObject = null;
        BuildPreloadOrder();
    }

    private void BuildPreloadOrder()
    {
        _preloadOrder.Clear();
        // The Overworld holds most of the markers and is the most often shown map:
        // it is preloaded first, then the other maps follow in display order.
        if (_mapIdentifiers.Contains("Overworld"))
        {
            _preloadOrder.Add("Overworld");
        }
        foreach (string identifier in _mapIdentifiers)
        {
            if (identifier != "Overworld")
            {
                _preloadOrder.Add(identifier);
            }
        }
    }

    private MapAssetsState GetOrCreateState(string identifier)
    {
        if (_states.TryGetValue(identifier, out MapAssetsState existingState))
        {
            return existingState;
        }

        TrackerMap map = _trackerMapProvider.GetTrackerMap(identifier);
        if (map == null)
        {
            _logger.LogError($"Could not find map with identifier {identifier} while initializing assets");
            return null;
        }

        MapAssetsState state = new MapAssetsState { Map = map };
        _states[identifier] = state;
        return state;
    }

    private void ProcessUnit(MapAssetsState state)
    {
        if (state.NextMarkerDataIndex < state.Map.MarkerDatas.Count)
        {
            MarkerData markerData = state.Map.MarkerDatas[state.NextMarkerDataIndex];
            List<GameObject> markers = _markerFactory.CreateMarkerObjects(state.Map, markerData);
            foreach (GameObject marker in markers)
            {
                if (marker != null)
                {
                    state.CreatedObjects.Add(marker);
                }
            }
            state.NextMarkerDataIndex++;
            return;
        }

        if (!state.ImageCreated)
        {
            GameObject mapImageObject = CreateMapObject(state.Map);
            if (mapImageObject != null)
            {
                state.CreatedObjects.Add(mapImageObject);
            }
            state.ImageCreated = true;
        }
    }

    private GameObject GetContentGameObject()
    {
        if (_contentGameObject == null)
        {
            GameObject mapObject = _objectFinder.FindObject(new ByComponent(typeof(Map)));
            if (mapObject != null)
            {
                foreach (Transform child in mapObject.transform)
                {
                    if (child.name == "Content")
                    {
                        _contentGameObject = child.gameObject;
                        break;
                    }
                }
            }
        }

        return _contentGameObject;
    }

    private GameObject CreateMapObject(TrackerMap map)
    {
        if (map.Identifier == "Overworld")
        {
            // The Overworld map already exists, we don't create it again.
            return null;
        }

        GameObject mapImageObject = new GameObject("RandomizerMap" + map.Identifier);
        _logger.LogDebug($"Instantiating map image object {mapImageObject.name}");
        mapImageObject.layer = LayerMask.NameToLayer("UI");
        GameObject contentGameObject = GetContentGameObject();
        if (contentGameObject == null)
        {
            _logger.LogError("Could not find Content object in Map");
            Object.Destroy(mapImageObject);
            return null;
        }
        SpriteData spriteData = _spriteProvider.GetSprite(map.SpriteData.SpriteName);
        mapImageObject.transform.SetParent(contentGameObject.transform, false);
        RectTransform spriteRectTransform = mapImageObject.AddComponent<RectTransform>();
        spriteRectTransform.pivot = new Vector2(0.5f, 0.5f);
        spriteRectTransform.sizeDelta = new Vector2(map.SpriteData.Width, map.SpriteData.Height);
        spriteRectTransform.anchoredPosition = new Vector2(0f, 0f);
        mapImageObject.AddComponent<CanvasRenderer>();
        Image spriteImage = mapImageObject.AddComponent<Image>();
        spriteImage.sprite = spriteData.Sprite;
        RandomizerMapSpriteComponent spriteComponent = mapImageObject.AddComponent<RandomizerMapSpriteComponent>();
        spriteComponent.Map = map;
        mapImageObject.transform.SetAsFirstSibling();

        return mapImageObject;
    }
}
