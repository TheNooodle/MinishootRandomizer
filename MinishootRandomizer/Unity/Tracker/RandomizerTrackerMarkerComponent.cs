using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MinishootRandomizer;

[RequireComponent(typeof(FloatyAnimationComponent))]
public class RandomizerTrackerMarkerComponent : MonoBehaviour
{
    // Static registry of all live tracker markers, so other components
    // (like the location list) can iterate them without a per-frame FindObjectsOfType.
    private static readonly HashSet<RandomizerTrackerMarkerComponent> _instances = new HashSet<RandomizerTrackerMarkerComponent>();
    public static IReadOnlyCollection<RandomizerTrackerMarkerComponent> GetInstances() => _instances;

    private IRandomizerEngine _randomizerEngine;
    private ILocationLogicChecker _logicChecker;
    private ILogicStateProvider _logicStateProvider;
    private ISpriteProvider _spriteProvider;

    private List<AbstractMarker> _markers = new List<AbstractMarker>();
    private AbstractMarker _currentMarker = null;
    private GameObject _spriteObject = null;
    private TrackerMap _map = null;
    private FloatyAnimationComponent _floatyAnimationComponent = null;
    private List<Location> _locations = new List<Location>();
    private Canvas _canvas = null;

    public IReadOnlyList<Location> Locations => _locations;

    void Awake()
    {
        _randomizerEngine = Plugin.ServiceContainer.Get<IRandomizerEngine>();
        _logicChecker = Plugin.ServiceContainer.Get<ILocationLogicChecker>();
        _logicStateProvider = Plugin.ServiceContainer.Get<ILogicStateProvider>();
        _spriteProvider = Plugin.ServiceContainer.Get<ISpriteProvider>();

        _floatyAnimationComponent = gameObject.GetComponent<FloatyAnimationComponent>();

        _instances.Add(this);
    }

    void OnDestroy()
    {
        _instances.Remove(this);
    }

    public void AddMarker(AbstractMarker marker)
    {
        _markers.Add(marker);
        _markers.Sort((a, b) => a.GetSortIndex().CompareTo(b.GetSortIndex()));
    }

    public void SetSpriteObject(GameObject spriteObject)
    {
        _spriteObject = spriteObject;
    }

    public void SetMap(TrackerMap map)
    {
        _map = map;
    }

    public void SetLocations(List<Location> locations)
    {
        _locations = locations;
    }

    // Returns the on-screen rectangle of the marker sprite when it is currently shown
    // (a hidden sprite cannot be hovered), along with the camera rendering its canvas
    // (null for a screen space overlay canvas). Used for pointer hit-testing.
    public bool TryGetHoverableRect(out RectTransform rect, out Camera canvasCamera)
    {
        rect = null;
        canvasCamera = null;

        if (_spriteObject == null || !_spriteObject.activeInHierarchy)
        {
            return false;
        }

        Image image = _spriteObject.GetComponent<Image>();
        if (image == null)
        {
            return false;
        }

        if (_canvas == null)
        {
            _canvas = GetComponentInParent<Canvas>();
            if (_canvas == null)
            {
                return false;
            }
        }

        rect = image.rectTransform;
        canvasCamera = _canvas.worldCamera;
        return true;
    }

    void Update()
    {
        // If the game is not randomized, we don't show any marker.
        if (!_randomizerEngine.IsRandomized())
        {
            HideMarker();
            return;
        }

        // If the map is not the current map, we don't show any marker.
        if (_map != RandomizerMapComponent.CurrentMap && !(_map.Identifier == "Overworld" && RandomizerMapComponent.CurrentMap == null))
        {
            HideMarker();
            return;
        }

        bool mustShow = false;

        // We compute the visibility of each underlying marker, and we show the first one that must be shown.
        foreach (AbstractMarker marker in _markers)
        {
            marker.ComputeVisibility(_randomizerEngine, _logicChecker, _logicStateProvider);
            if (marker.MustShow())
            {
                ChangeMarker(marker);
                mustShow = true;
                break;
            }
        }

        if (mustShow)
        {
            ShowMarker();
        }
        else
        {
            HideMarker();
        }
    }

    private void ChangeMarker(AbstractMarker newMarker)
    {
        if (newMarker == _currentMarker)
        {
            return;
        }

        _currentMarker = newMarker;
        MarkerSpriteInfo spriteInfo = newMarker.GetSpriteInfo();
        SpriteData newSpriteData = _spriteProvider.GetSprite(spriteInfo.SpriteIdentifier);
        Image image = _spriteObject.GetComponent<Image>();
        if (image == null)
        {
            return;
        }

        image.sprite = newSpriteData.Sprite;
        image.rectTransform.SetScale(new Vector2(spriteInfo.Scale.Item1, spriteInfo.Scale.Item2));

        _floatyAnimationComponent.SetAmplitude(newMarker.GetAnimationAmplitude());
    }

    private void HideMarker()
    {
        if (_spriteObject != null)
        {
            _spriteObject.SetActive(false);
        }
    }

    private void ShowMarker()
    {
        if (_spriteObject != null)
        {
            _spriteObject.SetActive(true);
        }
    }
}
