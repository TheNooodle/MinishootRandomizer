using System.Collections.Generic;
using UnityEngine;

namespace MinishootRandomizer;

public class RandomizerMapComponent : MonoBehaviour
{
    private ITrackerMapProvider _trackerMapProvider;
    private IMarkerFactory _markerFactory;

    private List<TrackerMap> _initializedMaps = new List<TrackerMap>();

    private static TrackerMap currentMap = null;
    public static TrackerMap CurrentMap => currentMap;

    void Awake()
    {
        _trackerMapProvider = Plugin.ServiceContainer.Get<ITrackerMapProvider>();
        _markerFactory = Plugin.ServiceContainer.Get<IMarkerFactory>();
    }

    void Start()
    {
        TrackerMap overworldMap = _trackerMapProvider.GetTrackerMap("Overworld");
        SetCurrentMap(overworldMap);
    }

    private void SetCurrentMap(TrackerMap map)
    {
        if (map == null)
        {
            return;
        }

        currentMap = map;
        if (!_initializedMaps.Contains(map))
        {
            _markerFactory.CreateMarkerObjects(map);
            _initializedMaps.Add(map);
        }
    }
}
