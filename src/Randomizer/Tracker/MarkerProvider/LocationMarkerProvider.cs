using System;
using System.Collections.Generic;
using UnityEngine;

namespace MinishootRandomizer;

public class LocationMarkerProvider : IMarkerProvider
{
    private readonly IObjectFinder _objectFinder;
    private readonly ILocationRepository _locationRepository;
    private readonly ILogger _logger = new NullLogger();

    private GameObject _markerParent;
    private GameObject _markerPrefab = null;

    public LocationMarkerProvider(IObjectFinder objectFinder, ILocationRepository locationRepository, ILogger logger = null)
    {
        _objectFinder = objectFinder;
        _locationRepository = locationRepository;
        _logger = logger ?? new NullLogger();
    }

    public List<GameObject> GetMarkerObjects()
    {
        _markerParent = _objectFinder.FindObject(new ByName("Collectable (Overworld)"));
        if (_markerParent == null)
        {
            _logger.LogError("Could not find MapMarkers object");
            return new List<GameObject>();
        }

        List<GameObject> markers = new List<GameObject>();
        foreach (LocationMarkerData markerData in _markerDatas)
        {
            markers.AddRange(CreateMarker(markerData));
        }

        return markers;
    }

    private List<GameObject> CreateMarker(LocationMarkerData markerData)
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

        List<GameObject> markers = new List<GameObject>();
        for (int i = 0; i < markerData.Coordinates.Count; i++)
        {
            Tuple<float, float> coordinate = markerData.Coordinates[i];
            GameObject markerObject = GameObject.Instantiate(_markerPrefab);
            markerObject.name = $"LocationMarker_{markerData.LocationNames[0]}_{i}";
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
            RandomizerLocationMarker markerComponent = markerObject.AddComponent<RandomizerLocationMarker>();
            markerComponent.SetLocations(locations);
            markerComponent.SetSprite(spriteObject);
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

    private readonly List<LocationMarkerData> _markerDatas = new() {
        new LocationMarkerData(new List<string> {
            "Abyss - Ambush Island",
        }, new List<Tuple<float, float>> {
            new Tuple<float, float>(-1.34f, -2.30f),
        }),
        new LocationMarkerData(new List<string> {
            "Abyss - Backroom item",
            "Abyss Church - Unchosen statue",
            "Abyss North Connector - Under ruins",
        }, new List<Tuple<float, float>> {
            new Tuple<float, float>(-1.57f, -2.27f),
            new Tuple<float, float>(-5.78f, -1.86f),
            new Tuple<float, float>(-2.77f, 0.40f),
            new Tuple<float, float>(-6.72f, -2.85f),
        }),
        new LocationMarkerData(new List<string> {
            "Abyss - Near dungeon entrance",
        }, new List<Tuple<float, float>> {
            new Tuple<float, float>(-2.32f, -1.49f),
        }),
        new LocationMarkerData(new List<string> {
            "Abyss - Near protected enemy",
        }, new List<Tuple<float, float>> {
            new Tuple<float, float>(-3.52f, -1.98f),
        }),
        new LocationMarkerData(new List<string> {
            "Abyss - South of spinning enemy",
        }, new List<Tuple<float, float>> {
            new Tuple<float, float>(-2.81f, -2.04f),
        }),
        new LocationMarkerData(new List<string> {
            "Abyss - Village Entrance",
        }, new List<Tuple<float, float>> {
            new Tuple<float, float>(-4.84f, -3.10f),
        }),
        new LocationMarkerData(new List<string> {
            "Abyss - Within Crystal Grove",
        }, new List<Tuple<float, float>> {
            new Tuple<float, float>(-2.77f, 0.23f),
        }),
        new LocationMarkerData(new List<string> {
            "Abyss Ruined shop - Item",
        }, new List<Tuple<float, float>> {
            new Tuple<float, float>(-1.85f, -1.47f),
        }),
        new LocationMarkerData(new List<string> {
            "Abyss Shack - Hidden corridor",
            "Abyss Shack - Hidden room",
            "Abyss Shack - Under pot",
        }, new List<Tuple<float, float>> {
            new Tuple<float, float>(-6.54f, -3.21f),
        }),
        new LocationMarkerData(new List<string> {
            "Abyss Tower - Top of tower",
        }, new List<Tuple<float, float>> {
            new Tuple<float, float>(-4.80f, -2.51f),
        }),
        new LocationMarkerData(new List<string> {
            "Beach - Coast Hidden by plants",
        }, new List<Tuple<float, float>> {
            new Tuple<float, float>(4.25f, -0.36f),
        }),
        new LocationMarkerData(new List<string> {
            "Beach - Coast North hidden alcove",
        }, new List<Tuple<float, float>> {
            new Tuple<float, float>(4.48f, 0.30f),
        }),
        new LocationMarkerData(new List<string> {
            "Beach - Coast South hidden alcove",
        }, new List<Tuple<float, float>> {
            new Tuple<float, float>(3.05f, -1.19f),
        }),
        new LocationMarkerData(new List<string> {
            "Beach - Coconut pile",
        }, new List<Tuple<float, float>> {
            new Tuple<float, float>(7.00f, 0.78f),
        }),
        new LocationMarkerData(new List<string> {
            "Beach - East Island",
        }, new List<Tuple<float, float>> {
            new Tuple<float, float>(7.09f, -1.10f),
        }),
        new LocationMarkerData(new List<string> {
            "Beach - Protected item",
        }, new List<Tuple<float, float>> {
            new Tuple<float, float>(5.51f, -0.95f),
        }),
        new LocationMarkerData(new List<string> {
            "Beach - Seashell above dungeon",
        }, new List<Tuple<float, float>> {
            new Tuple<float, float>(3.96f, -1.67f),
        }),
        new LocationMarkerData(new List<string> {
            "Beach - South East Island",
        }, new List<Tuple<float, float>> {
            new Tuple<float, float>(6.23f, -2.43f),
        }),
        new LocationMarkerData(new List<string> {
            "Cemetery - Crying house",
        }, new List<Tuple<float, float>> {
            new Tuple<float, float>(0.32f, 1.31f),
        }),
        new LocationMarkerData(new List<string> {
            "Cemetery - Under enemy",
        }, new List<Tuple<float, float>> {
            new Tuple<float, float>(-0.77f, 2.31f),
        }),
        new LocationMarkerData(new List<string> {
            "Cemetery - West pot",
        }, new List<Tuple<float, float>> {
            new Tuple<float, float>(-1.53f, 2.25f),
        }),
        new LocationMarkerData(new List<string> {
            "Cemetery Tower - Top of tower",
            "Scarab Temple - After race 1",
            "Scarab Temple - After race 2",
            "Scarab Temple - After race 3"
        }, new List<Tuple<float, float>> {
            new Tuple<float, float>(2.00f, 3.85f),
        }),
        new LocationMarkerData(new List<string> {
            "Crystal Grove Temple - Boss reward",
            "Crystal Grove Temple - Dodge the east cannons",
            "Crystal Grove Temple - East tunnels",
            "Crystal Grove Temple - North east hidden room",
            "Crystal Grove Temple - South West Hidden pond",
            "Crystal Grove Temple - West pot"
        }, new List<Tuple<float, float>> {
            new Tuple<float, float>(-2.15f, 0.66f),
        }),
        new LocationMarkerData(new List<string> {
            "Crystal Grove Tower - Top of tower",
        }, new List<Tuple<float, float>> {
            new Tuple<float, float>(-2.15f, 1.64f),
        }),
        new LocationMarkerData(new List<string> {
            "Desert - North east platforms",
        }, new List<Tuple<float, float>> {
            new Tuple<float, float>(-2.67f, 2.77f),
        }),
        new LocationMarkerData(new List<string> {
            "Desert - On the river",
        }, new List<Tuple<float, float>> {
            new Tuple<float, float>(-4.96f, 1.64f),
        }),
        new LocationMarkerData(new List<string> {
            "Desert - Under ruins",
        }, new List<Tuple<float, float>> {
            new Tuple<float, float>(-3.94f, 1.64f),
        }),
        new LocationMarkerData(new List<string> {
            "Desert Grotto - Both torches lighted",
        }, new List<Tuple<float, float>> {
            new Tuple<float, float>(-5.05f, -0.55f),
            new Tuple<float, float>(-5.97f, -0.20f),
            new Tuple<float, float>(-6.89f, -0.58f),
        }),
        new LocationMarkerData(new List<string> {
            "Desert Temple - Boss reward",
            "Desert Temple - North East pot",
            "Desert Temple - Secret room"
        }, new List<Tuple<float, float>> {
            new Tuple<float, float>(-5.94f, 1.03f),
        }),
        new LocationMarkerData(new List<string> {
            "Dungeon 1 - Central item",
            "Dungeon 1 - Crystal near east armored spinner",
            // "Dungeon 1 - Dungeon reward",
            "Dungeon 1 - Entrance after south ramp",
            "Dungeon 1 - Entrance East Arena",
            "Dungeon 1 - Entrance west bridge",
            "Dungeon 1 - Far West Arena after spinner",
            "Dungeon 1 - Hidden below crystal wall",
            "Dungeon 1 - Hidden in West Arena",
            "Dungeon 1 - Inside the crystal wall",
            "Dungeon 1 - Near boss",
            "Dungeon 1 - Near east armored spinner",
            "Dungeon 1 - Near west armored spinner",
            "Dungeon 1 - North West Arena",
            "Dungeon 1 - Platform after crystal wall",
            "Dungeon 1 - South item",
            "Dungeon 1 - West bridge hidden item",
        }, new List<Tuple<float, float>> {
            new Tuple<float, float>(1.54f, 2.09f),
        }),
        new LocationMarkerData(new List<string> {
            "Dungeon 2 - Central item",
            // "Dungeon 2 - Dungeon reward",
            "Dungeon 2 - Hidden by plants",
            "Dungeon 2 - Item after jumps",
            "Dungeon 2 - North east beyond arena",
            "Dungeon 2 - North item",
            "Dungeon 2 - North west arena",
            "Dungeon 2 - Secret room",
            "Dungeon 2 - South west arena",
            "Dungeon 2 - Treasure room",
            "Dungeon 2 - Treasure room entrance",
            "Dungeon 2 - Walled arena extra",
            "Dungeon 2 - Walled arena item",
            "Dungeon 2 - West arena",
            "Dungeon 2 - West arena extra",
        }, new List<Tuple<float, float>> {
            new Tuple<float, float>(-2.74f, -0.65f),
        }),
        new LocationMarkerData(new List<string> {
            "Dungeon 3 - Behind North West doors",
            "Dungeon 3 - Central Item",
            // "Dungeon 3 - Dungeon reward",
            "Dungeon 3 - East Island",
            "Dungeon 3 - East rock 1",
            "Dungeon 3 - East rock 2",
            "Dungeon 3 - East wall",
            "Dungeon 3 - Hidden Tunnel",
            "Dungeon 3 - Inside middle pot",
            "Dungeon 3 - Item protected by spikes",
            "Dungeon 3 - North arena",
            "Dungeon 3 - Over the pit",
            "Dungeon 3 - Pot Island",
            "Dungeon 3 - Race on the water",
            "Dungeon 3 - South corridor",
            "Dungeon 3 - South of torches",
            "Dungeon 3 - South west of torches",
        }, new List<Tuple<float, float>> {
            new Tuple<float, float>(4.46f, -2.91f),
        }),




        new LocationMarkerData(new List<string> {
            "Town - Blacksmith Item 1",
            "Town - Blacksmith Item 2",
            "Town - Blacksmith Item 3",
            "Town - Blacksmith Item 4",
            "Town - Mercant Item 1",
            "Town - Mercant Item 2",
            "Town - Mercant Item 3",
            "Town - Plaza",
            "Town - Scarab Collector Item 1",
            "Town - Scarab Collector Item 2",
            "Town - Scarab Collector Item 3",
            "Town - Scarab Collector Item 4",
            "Town - Scarab Collector Item 5",
            "Town - Scarab Collector Item 6",
            "Starting Grotto - Entrance",
            "Starting Grotto - North Corridor",
            "Starting Grotto - Secret Wall",
            "Starting Grotto - West Item",
        }, new List<Tuple<float, float>> {
            new Tuple<float, float>(0.32f, 0.75f),
        }),
        new LocationMarkerData(new List<string> {
            "Town Pillars - Hidden below bridge"
        }, new List<Tuple<float, float>> {
            new Tuple<float, float>(-0.40f, 0.28f),
        }),
        new LocationMarkerData(new List<string> {
            "Town Pillars - Hidden Pond"
        }, new List<Tuple<float, float>> {
            new Tuple<float, float>(-0.88f, 0.17f),
        }),
        new LocationMarkerData(new List<string> {
            "Town Pillars Grotto - Reward"
        }, new List<Tuple<float, float>> {
            new Tuple<float, float>(-0.88f, 1.10f),
        }),
        new LocationMarkerData(new List<string> {
            "Zelda 1 Grotto - Behind the closed doors"
        }, new List<Tuple<float, float>> {
            new Tuple<float, float>(0.06f, -0.11f),
        }),
    };
}
