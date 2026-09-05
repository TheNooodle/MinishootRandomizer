using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MinishootRandomizer;

public class RandomizerMapComponent : MonoBehaviour
{
    private ITrackerMapProvider _trackerMapProvider;
    private IMarkerFactory _markerFactory;
    private ISpriteProvider _spriteProvider;
    private CurrentMapHandler _currentMapHandler;
    private ILogger _logger = new NullLogger();

    private List<TrackerMap> _initializedMaps = new List<TrackerMap>();
    private GameObject _contentGameObject = null;
    private bool _isInitialized = false;

    private static TrackerMap currentMap = null;
    public static TrackerMap CurrentMap => currentMap;
    public static float DebugScale = 0.0f;

    void Awake()
    {
        if (!_isInitialized)
        {
            Initialize();
        }
    }

    // We separate initialization from the Unity lifecycle methods to allow event handling.
    private void Initialize()
    {
        _trackerMapProvider = Plugin.ServiceContainer.Get<ITrackerMapProvider>();
        _markerFactory = Plugin.ServiceContainer.Get<IMarkerFactory>();
        _spriteProvider = Plugin.ServiceContainer.Get<ISpriteProvider>();
        _currentMapHandler = Plugin.ServiceContainer.Get<CurrentMapHandler>();
        _logger = Plugin.ServiceContainer.Get<ILogger>() ?? new NullLogger();
        _isInitialized = true;
    }

    void OnDestroy()
    {
        currentMap = null;
    }

    public void OnMapOpened()
    {
        if (!_isInitialized)
        {
            Initialize();
        }

        TrackerMap map = _currentMapHandler.GetCurrentMap();
        if (map != null && map != currentMap)
        {
            SetCurrentMap(map);
        }
    }

    public void OnAfterMapOpened()
    {
        // @todo: set progress counter, set goal
    }

    private void SetCurrentMap(TrackerMap map)
    {
        currentMap = map;

        // If the map has not been initialized yet, we create its objects (map image + markers).
        if (map != null && !_initializedMaps.Contains(map))
        {
            _markerFactory.CreateMarkerObjects(map);
            CreateMapObject(map);
            _initializedMaps.Add(map);
        }

        // We handle the Overworld map special case (hiding/showing the original map elements).
        HandleOverworldMap(map == null || map.Identifier == "Overworld");
    }

    private GameObject GetContentGameObject()
    {
        if (_contentGameObject == null)
        {
            foreach (Transform child in transform)
            {
                if (child.name == "Content")
                {
                    _contentGameObject = child.gameObject;
                    break;
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
        mapImageObject.layer = LayerMask.NameToLayer("UI");
        GameObject contentGameObject = GetContentGameObject();
        if (contentGameObject == null)
        {
            _logger.LogError("Could not find Content object in Map");
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

    private void HandleOverworldMap(bool isOverworld)
    {
        // To handle the already existing Overworld map, we hide it when we show our own map.
        List<string> gameObjectNamesToHide = new List<string>()
        {
            "Border",
            "Fragments",
            "Connections",
            "Icons"
        };
        GameObject contentGameObject = GetContentGameObject();
        if (contentGameObject == null)
        {
            _logger.LogError("Could not find Content object in Map");
            return;
        }

        foreach (Transform child in contentGameObject.transform)
        {
            if (gameObjectNamesToHide.Contains(child.name))
            {
                // If the current map is the Overworld, we show the original map objects.
                // Otherwise, we hide them.
                child.gameObject.SetActive(isOverworld);
            }
        }
    }
}
