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
        ScarabMarkerData scarabMarkerData = markerData.ScarabMarkerData;
        SpiritMarkerData spiritMarkerData = markerData.SpiritMarkerData;
        ObjectiveMarkerData objectiveMarkerData = markerData.ObjectiveMarkerData;

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
                OolLocationMarker oolLocationMarker = new OolLocationMarker(locations);
                markerComponent.AddMarker(locationMarker);
                markerComponent.AddMarker(oolLocationMarker);
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
                    OolNpcMarker oolNpcMarker = new OolNpcMarker(npcLocation, npcMarkerData.NpcIdentifier);
                    markerComponent.AddMarker(npcMarker);
                    markerComponent.AddMarker(oolNpcMarker);
                }
            }

            if (scarabMarkerData != null)
            {
                Dictionary<string, Location> scarabLocations = new Dictionary<string, Location>();
                foreach (KeyValuePair<string, string> scarabLocationPair in scarabMarkerData.ScarabLocationMap)
                {
                    Location scarabLocation = _locationRepository.Get(scarabLocationPair.Value);
                    if (scarabLocation == null)
                    {
                        _logger.LogError($"Location {scarabLocationPair.Value} not found while creating marker");
                    }
                    else
                    {
                        scarabLocations.Add(scarabLocationPair.Key, scarabLocation);
                    }
                }

                ScarabMarker scarabMarker = new ScarabMarker(scarabLocations);
                OolScarabMarker oolScarabMarker = new OolScarabMarker(scarabLocations);
                markerComponent.AddMarker(scarabMarker);
                markerComponent.AddMarker(oolScarabMarker);
            }

            if (spiritMarkerData != null)
            {
                Location spiritLocation = _locationRepository.Get(spiritMarkerData.LocationName);
                if (spiritLocation == null)
                {
                    _logger.LogError($"Location {spiritMarkerData.LocationName} not found while creating marker");
                }
                else
                {
                    SpiritMarker spiritMarker = new SpiritMarker(spiritLocation, spiritMarkerData.SpiritIdentifier);
                    OolSpiritMarker oolSpiritMarker = new OolSpiritMarker(spiritLocation, spiritMarkerData.SpiritIdentifier);
                    markerComponent.AddMarker(spiritMarker);
                    markerComponent.AddMarker(oolSpiritMarker);
                }
            }

            if (objectiveMarkerData != null)
            {
                Location objectiveLocation = _locationRepository.Get(objectiveMarkerData.ObjectiveLocationIdentifier);
                if (objectiveLocation == null)
                {
                    _logger.LogError($"Location {objectiveMarkerData.ObjectiveLocationIdentifier} not found while creating marker");
                }
                else
                {
                    ObjectiveMarker objectiveMarker = new ObjectiveMarker(objectiveLocation, objectiveMarkerData.ObjectiveGoal);
                    OolObjectiveMarker oolObjectiveMarker = new OolObjectiveMarker(objectiveLocation, objectiveMarkerData.ObjectiveGoal);
                    markerComponent.AddMarker(objectiveMarker);
                    markerComponent.AddMarker(oolObjectiveMarker);
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
