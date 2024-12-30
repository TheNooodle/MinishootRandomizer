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
        // new LocationMarkerData(new List<string> {
        //     "Dungeon 4 - Dungeon reward",
        // }, new List<Tuple<float, float>> {
        //     new Tuple<float, float>(-3.74f, 5.28f),
        // }),
        new LocationMarkerData(new List<string> {
            "Dungeon 5 - Central Item",
        }, new List<Tuple<float, float>> {
            new Tuple<float, float>(1.66f, 6.98f),
            new Tuple<float, float>(-1.09f, 6.98f),
        }),
        new LocationMarkerData(new List<string> {
            "Family House Cave - Before shortcut",
            "Family House Cave - Hidden Tunnel",
            "Family House Cave - Near button",
            "Family House Cave - Near tree",
            "Family House Cave - Reunited Family",
            "Family House Cave - Sewers"
        }, new List<Tuple<float, float>> {
            new Tuple<float, float>(-0.13f, 0.62f),
            new Tuple<float, float>(-3.73f, 0.42f),
            new Tuple<float, float>(-3.74f, -0.41f),
        }),
        new LocationMarkerData(new List<string> {
            "Forest - Boss Reward",
        }, new List<Tuple<float, float>> {
            new Tuple<float, float>(1.46f, -4.21f),
        }),
        new LocationMarkerData(new List<string> {
            "Forest - Bush behind tree",
        }, new List<Tuple<float, float>> {
            new Tuple<float, float>(-0.23f, -1.26f),
        }),
        new LocationMarkerData(new List<string> {
            "Forest - Enemy tree near Beach",
        }, new List<Tuple<float, float>> {
            new Tuple<float, float>(2.40f, -3.26f),
        }),
        new LocationMarkerData(new List<string> {
            "Forest - Far South East",
        }, new List<Tuple<float, float>> {
            new Tuple<float, float>(2.63f, -7.77f),
        }),
        new LocationMarkerData(new List<string> {
            "Forest - Faraway island item",
            "Forest - Faraway island extra"
        }, new List<Tuple<float, float>> {
            new Tuple<float, float>(-2.48f, -7.57f),
        }),
        new LocationMarkerData(new List<string> {
            "Forest - Hidden east tunnel",
        }, new List<Tuple<float, float>> {
            new Tuple<float, float>(1.54f, -5.38f),
        }),
        new LocationMarkerData(new List<string> {
            "Forest - Pot",
        }, new List<Tuple<float, float>> {
            new Tuple<float, float>(2.57f, -4.01f),
        }),
        new LocationMarkerData(new List<string> {
            "Forest - Secret pond behind bushes",
        }, new List<Tuple<float, float>> {
            new Tuple<float, float>(0.28f, -2.98f),
        }),
        new LocationMarkerData(new List<string> {
            "Forest - Secret pond bush",
        }, new List<Tuple<float, float>> {
            new Tuple<float, float>(0.52f, -1.46f),
        }),
        new LocationMarkerData(new List<string> {
            "Forest - Secret within Secret",
        }, new List<Tuple<float, float>> {
            new Tuple<float, float>(0.86f, -0.86f),
        }),
        new LocationMarkerData(new List<string> {
            "Forest - Tunnel below big tree enemy",
        }, new List<Tuple<float, float>> {
            new Tuple<float, float>(-1.37f, -4.58f),
        }),
        new LocationMarkerData(new List<string> {
            "Forest Grotto - After ramp",
        }, new List<Tuple<float, float>> {
            new Tuple<float, float>(0.94f, -2.82f),
            new Tuple<float, float>(-0.09f, -0.93f),
        }),
        new LocationMarkerData(new List<string> {
            "Forest Shop 1",
            "Forest Shop 2",
            "Forest Shop 3"
        }, new List<Tuple<float, float>> {
            new Tuple<float, float>(-0.51f, -2.96f),
        }),
        new LocationMarkerData(new List<string> {
            "Green - Behind Closed Arena",
        }, new List<Tuple<float, float>> {
            new Tuple<float, float>(2.43f, -0.98f),
        }),
        new LocationMarkerData(new List<string> {
            "Green - Bridge Shortcut",
        }, new List<Tuple<float, float>> {
            new Tuple<float, float>(1.77f, 0.77f),
        }),
        new LocationMarkerData(new List<string> {
            "Green - Button Item",
        }, new List<Tuple<float, float>> {
            new Tuple<float, float>(2.26f, 1.65f),
        }),
        new LocationMarkerData(new List<string> {
            "Green - Closed Arena Item",
        }, new List<Tuple<float, float>> {
            new Tuple<float, float>(2.43f, -0.72f),
        }),
        new LocationMarkerData(new List<string> {
            "Green - Crossroad Arena Item",
        }, new List<Tuple<float, float>> {
            new Tuple<float, float>(2.89f, 0.82f),
        }),
        new LocationMarkerData(new List<string> {
            "Green - Forest Entrance",
        }, new List<Tuple<float, float>> {
            new Tuple<float, float>(1.52f, -1.10f),
        }),
        new LocationMarkerData(new List<string> {
            "Green - Grove near button",
        }, new List<Tuple<float, float>> {
            new Tuple<float, float>(-0.57f, -0.11f),
        }),
        new LocationMarkerData(new List<string> {
            "Green - Grove under ruins",
        }, new List<Tuple<float, float>> {
            new Tuple<float, float>(-1.14f, 0.00f),
        }),
        new LocationMarkerData(new List<string> {
            "Green - Hidden before Island Arena",
        }, new List<Tuple<float, float>> {
            new Tuple<float, float>(3.71f, -0.06f),
        }),
        new LocationMarkerData(new List<string> {
            "Green - Island Arena Item",
        }, new List<Tuple<float, float>> {
            new Tuple<float, float>(3.71f, 2.05f),
        }),
        new LocationMarkerData(new List<string> {
            "Green - Outside Dungeon 1 Cave",
        }, new List<Tuple<float, float>> {
            new Tuple<float, float>(1.54f, 1.36f),
        }),
        new LocationMarkerData(new List<string> {
            "Green - River near Forest Entrance",
        }, new List<Tuple<float, float>> {
            new Tuple<float, float>(-1.25f, -1.11f),
        }),
        new LocationMarkerData(new List<string> {
            "Green - Shortcut to Town Pillars",
        }, new List<Tuple<float, float>> {
            new Tuple<float, float>(-1.34f, 0.82f),
        }),
        new LocationMarkerData(new List<string> {
            "Green Grotto - Before race",
            "Green Grotto - Corner",
            "Green Grotto - Drop",
        }, new List<Tuple<float, float>> {
            new Tuple<float, float>(2.43f, -0.10f),
            new Tuple<float, float>(1.37f, -0.62f),
            new Tuple<float, float>(2.37f, -1.54f),
        }),
        new LocationMarkerData(new List<string> {
            "Jar Shop 1",
            "Jar Shop 2",
            "Jar Shop 3",
        }, new List<Tuple<float, float>> {
            new Tuple<float, float>(-1.49f, 0.39f),
        }),
        new LocationMarkerData(new List<string> {
            "Junkyard - East pond",
        }, new List<Tuple<float, float>> {
            new Tuple<float, float>(1.83f, 5.54f),
        }),
        new LocationMarkerData(new List<string> {
            "Junkyard - Inside Sunken City",
        }, new List<Tuple<float, float>> {
            new Tuple<float, float>(-2.74f, 4.80f),
        }),
        new LocationMarkerData(new List<string> {
            "Junkyard - South East",
        }, new List<Tuple<float, float>> {
            new Tuple<float, float>(0.88f, 3.57f),
        }),
        new LocationMarkerData(new List<string> {
            "Junkyard East Shack - Item",
        }, new List<Tuple<float, float>> {
            new Tuple<float, float>(1.26f, 5.21f),
        }),
        new LocationMarkerData(new List<string> {
            "Junkyard West Shack - Item",
        }, new List<Tuple<float, float>> {
            new Tuple<float, float>(-1.60f, 6.00f),
        }),
        new LocationMarkerData(new List<string> {
            "Primordial Cave - Meet the Primordial Scarab",
        }, new List<Tuple<float, float>> {
            new Tuple<float, float>(-3.74f, 7.25f),
        }),
        new LocationMarkerData(new List<string> {
            "Scarab Temple - Backroom",
            "Scarab Temple - Central Item",
            "Scarab Temple - East side",
            "Scarab Temple - Middle Entrance"
        }, new List<Tuple<float, float>> {
            new Tuple<float, float>(0.53f, 1.02f),
            new Tuple<float, float>(2.34f, 1.32f),
            new Tuple<float, float>(2.89f, 1.95f),
            new Tuple<float, float>(2.88f, 2.72f),
        }),
        new LocationMarkerData(new List<string> {
            "Scarab Temple - Bottom Left Torch Item",
            "Swamp Jumps Grotto - Drop"
        }, new List<Tuple<float, float>> {
            new Tuple<float, float>(0.97f, 2.65f),
        }),
        new LocationMarkerData(new List<string> {
            "Sewers - Behind West Entrance",
            "Sewers - Central room boss reward",
            "Sewers - Central room corner",
            "Sewers - Drop",
            "Sewers - Near Family House Cave",
            "Sewers - North pot room",
            "Sewers - South pot room",
            "Sunken City - Inside the walls"
        }, new List<Tuple<float, float>> {
            new Tuple<float, float>(-4.77f, 2.92f),
            new Tuple<float, float>(-5.92f, 3.52f),
            new Tuple<float, float>(-7.52f, 3.72f),
            new Tuple<float, float>(-6.97f, 4.52f),
            new Tuple<float, float>(-5.31f, 5.84f),
            new Tuple<float, float>(-4.00f, 4.87f),
            new Tuple<float, float>(-3.48f, 4.87f),
        }),
        new LocationMarkerData(new List<string> {
            "Spirit Tower - Item",
        }, new List<Tuple<float, float>> {
            new Tuple<float, float>(0.26f, -4.51f),
        }),
        new LocationMarkerData(new List<string> {
            "Sunken City - Below West bridge",
        }, new List<Tuple<float, float>> {
            new Tuple<float, float>(-4.35f, 4.31f),
        }),
        new LocationMarkerData(new List<string> {
            "Sunken City - Inside the walls",
        }, new List<Tuple<float, float>> {
            new Tuple<float, float>(-3.75f, 4.28f),
        }),
        new LocationMarkerData(new List<string> {
            "Sunken City - Near West entrance",
        }, new List<Tuple<float, float>> {
            new Tuple<float, float>(-4.40f, 3.05f),
        }),
        new LocationMarkerData(new List<string> {
            "Sunken City - North bridge",
        }, new List<Tuple<float, float>> {
            new Tuple<float, float>(-3.33f, 5.54f),
        }),
        new LocationMarkerData(new List<string> {
            "Sunken City - North East district",
        }, new List<Tuple<float, float>> {
            new Tuple<float, float>(-3.26f, 5.36f),
        }),
        new LocationMarkerData(new List<string> {
            "Sunken City - North West pot",
        }, new List<Tuple<float, float>> {
            new Tuple<float, float>(-7.74f, 7.39f),
        }),
        new LocationMarkerData(new List<string> {
            "Sunken City - South West building item",
        }, new List<Tuple<float, float>> {
            new Tuple<float, float>(-7.52f, 3.60f),
        }),
        new LocationMarkerData(new List<string> {
            "Sunken City - West bridge",
        }, new List<Tuple<float, float>> {
            new Tuple<float, float>(-4.57f, 4.62f),
        }),
        new LocationMarkerData(new List<string> {
            "Sunken City - West bridge pot",
        }, new List<Tuple<float, float>> {
            new Tuple<float, float>(-4.57f, 4.01f),
        }),
        new LocationMarkerData(new List<string> {
            "Sunken City Building - Drop",
        }, new List<Tuple<float, float>> {
            new Tuple<float, float>(-3.03f, 4.22f),
            new Tuple<float, float>(-3.03f, 5.12f),
            new Tuple<float, float>(-3.29f, 5.08f),
        }),
        new LocationMarkerData(new List<string> {
            "Sunken Temple - Boss reward",
            "Sunken Temple - Entrance",
            "Sunken Temple - Secret tunnel",
            "Sunken Temple - South West tunnel",
            "Sunken Temple - Under the lilypad"
        }, new List<Tuple<float, float>> {
            new Tuple<float, float>(-5.92f, 5.02f),
        }),
        new LocationMarkerData(new List<string> {
            "Swamp - Blocked tunnel",
        }, new List<Tuple<float, float>> {
            new Tuple<float, float>(5.77f, 3.20f),
        }),
        new LocationMarkerData(new List<string> {
            "Swamp - Hidden Before Big Enemy",
        }, new List<Tuple<float, float>> {
            new Tuple<float, float>(3.46f, 4.31f),
        }),
        new LocationMarkerData(new List<string> {
            "Swamp - Hidden in plant",
        }, new List<Tuple<float, float>> {
            new Tuple<float, float>(5.17f, 2.40f),
        }),
        new LocationMarkerData(new List<string> {
            "Swamp - Near cracked wall",
        }, new List<Tuple<float, float>> {
            new Tuple<float, float>(5.03f, 5.11f),
        }),
        new LocationMarkerData(new List<string> {
            "Swamp - North item",
        }, new List<Tuple<float, float>> {
            new Tuple<float, float>(5.26f, 7.19f),
        }),
        new LocationMarkerData(new List<string> {
            "Swamp - South West Island Hidden in trees Item",
        }, new List<Tuple<float, float>> {
            new Tuple<float, float>(2.20f, 3.02f),
        }),
        new LocationMarkerData(new List<string> {
            "Swamp - Under rocks",
        }, new List<Tuple<float, float>> {
            new Tuple<float, float>(2.95f, 4.57f),
        }),
        new LocationMarkerData(new List<string> {
            "Swamp Shop 1",
            "Swamp Shop 2",
            "Swamp Shop 3",
            "Swamp Shop Extra",
        }, new List<Tuple<float, float>> {
            new Tuple<float, float>(4.46f, 4.34f),
        }),
        new LocationMarkerData(new List<string> {
            "Swamp Tower - Top of tower",
        }, new List<Tuple<float, float>> {
            new Tuple<float, float>(3.71f, 2.82f),
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
