using System;
using System.Collections.Generic;
using UnityEngine;

namespace MinishootRandomizer;

public class CoreMarkerFactory : IMarkerFactory
{
    private readonly IMarkerDataProvider _markerDataProvider;
    private readonly IObjectFinder _objectFinder;
    private readonly ILocationRepository _locationRepository;
    private readonly ILogger _logger = new NullLogger();

    private GameObject _markerParent;
    private GameObject _markerPrefab = null;

    public CoreMarkerFactory(IMarkerDataProvider markerDataProvider, IObjectFinder objectFinder, ILocationRepository locationRepository, ILogger logger = null)
    {
        _markerDataProvider = markerDataProvider;
        _objectFinder = objectFinder;
        _locationRepository = locationRepository;
        _logger = logger ?? new NullLogger();
    }

    public List<GameObject> CreateMarkerObjects()
    {
        _markerParent = _objectFinder.FindObject(new ByName("Collectable (Overworld)"));
        if (_markerParent == null)
        {
            _logger.LogError("Could not find MapMarkers object");
            return new List<GameObject>();
        }

        List<MarkerData> locationMarkerDatas = _markerDataProvider.GetMarkerDatas();
        List<GameObject> markers = new List<GameObject>();
        foreach (MarkerData markerData in locationMarkerDatas)
        {
            markers.AddRange(CreateMarkers(markerData));
        }

        return markers;
    }

    private List<GameObject> CreateMarkers(MarkerData markerData)
    {
        if (_markerPrefab == null)
        {
            _markerPrefab = CreatePrefab();
            if (_markerPrefab == null)
            {
                return new List<GameObject>();
            }
        }

        List<Location> locations = new List<Location>();
        foreach (string locationName in markerData.LocationNames)
        {
            Location location = _locationRepository.Get(locationName);
            if (location == null)
            {
                _logger.LogError($"Location {locationName} not found while creating marker");
                continue;
            }
            locations.Add(location);
        }

        NpcMarkerData npcMarkerData = markerData.NpcMarkerData; 

        List<GameObject> markers = new List<GameObject>();
        for (int i = 0; i < markerData.Coordinates.Count; i++)
        {
            Tuple<float, float> coordinate = markerData.Coordinates[i];
            GameObject markerObject = GameObject.Instantiate(_markerPrefab);
            markerObject.name = $"{markerData}_{i}";
            markerObject.transform.SetParent(_markerParent.transform);
            markerObject.transform.position = new Vector3(coordinate.Item1, coordinate.Item2, 90f);
            markerObject.transform.localScale = new Vector3(0.18f, 0.18f, 0.18f);

            if (markerObject.GetComponent<ForceDeactivationComponent>())
            {
                GameObject.Destroy(markerObject.GetComponent<ForceDeactivationComponent>());
            }
            if (markerObject.GetComponent<MapMarkerView>())
            {
                GameObject.Destroy(markerObject.GetComponent<MapMarkerView>());
            }

            GameObject spriteObject = markerObject.transform.Find("Image").gameObject;
            RandomizerTrackerMarkerComponent markerComponent = markerObject.AddComponent<RandomizerTrackerMarkerComponent>();
            markerComponent.SetSpriteObject(spriteObject);

            if (locations.Count > 0)
            {
                LocationMarker locationMarker = new LocationMarker(locations);
                markerComponent.AddMarker(locationMarker);
            }

            if (npcMarkerData != null)
            {
                Location npcLocation = _locationRepository.Get(npcMarkerData.LocationName);
                if (npcLocation == null)
                {
                    _logger.LogError($"Location {npcMarkerData.LocationName} not found while creating marker");
                }
                else
                {
                    NpcMarker npcMarker = new NpcMarker(npcLocation, npcMarkerData.NpcIdentifier);
                    markerComponent.AddMarker(npcMarker);
                }
            }

            markerObject.SetActive(true);

            markers.Add(markerObject);
        }

        return markers;
    }

    private GameObject CreatePrefab()
    {
        GameObject prefabOrigin = _objectFinder.FindObject(new ByName("MapMarkerCollectableOverworld(Clone)"));
        if (prefabOrigin == null)
        {
            _logger.LogError("Could not find MapMarkerCollectableOverworld(Clone) object");
            return null;
        }

        _markerPrefab = GameObject.Instantiate(prefabOrigin);

        return _markerPrefab;
    }
}
