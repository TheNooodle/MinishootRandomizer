using System.Collections.Generic;

namespace MinishootRandomizer;

public class CurrentMapHandler
{
    private ITrackerMapProvider _trackerMapProvider;
    private IRandomizerEngine _randomizerEngine;
    private ILogger _logger;

    private static Dictionary<string, string> _transitionToLocationMap = new Dictionary<string, string>()
    {
        // Starting Grotto
        {"Overworld > Cave_0", "StartingGrotto"},

        // Green Grotto
        {"Overworld > Cave_7",  "GreenGrotto"},
        {"Overworld > Cave_9",  "GreenGrotto"},
        {"Overworld > Cave_10", "GreenGrotto"},

        // Scarab Temple
        {"Cave > Cave_7",       "ScarabTemple"},
        {"Cave > Cave_9",       "ScarabTemple"},
        {"Overworld > Cave_2",  "ScarabTemple"},
        {"Overworld > Cave_3",  "ScarabTemple"},
        {"Overworld > Cave_4",  "ScarabTemple"},
        {"Overworld > Cave_5",  "ScarabTemple"},
        {"Overworld > Cave_8",  "ScarabTemple"},
        {"Overworld > Cave_12", "ScarabTemple"},
        {"Overworld > Cave_15", "ScarabTemple"},
        {"Overworld > Cave_19", "ScarabTemple"},
        {"Tower > Cave_0",      "ScarabTemple"},

        // Family Cave
        {"Overworld > Cave_13", "FamilyCave"},
        {"Overworld > Cave_25", "FamilyCave"},
        {"Overworld > Cave_26", "FamilyCave"},

        // Sewers
        {"Overworld > Cave_23", "Sewers"},
        {"Overworld > Cave_27", "Sewers"},
        {"Overworld > Cave_28", "Sewers"},
        {"Overworld > Cave_29", "Sewers"},
        {"Overworld > Cave_31", "Sewers"},
        {"Overworld > Cave_32", "Sewers"},
        {"Overworld > Cave_33", "Sewers"},
        {"Overworld > Cave_34", "Sewers"},
        {"Overworld > Cave_39", "Sewers"},
        {"Overworld > Cave_40", "Sewers"},
        {"Overworld > Cave_45", "Sewers"},
        {"Overworld > Cave_46", "Sewers"},
    };

    public CurrentMapHandler(ITrackerMapProvider trackerMapProvider, IRandomizerEngine randomizerEngine, ILogger logger)
    {
        _trackerMapProvider = trackerMapProvider;
        _randomizerEngine = randomizerEngine;
        _logger = logger ?? new NullLogger();
    }

    public TrackerMap GetCurrentMap()
    {
        if (!string.IsNullOrEmpty(RandomizerMapComponent.ForceCurrentMapIdentifier))
        {
            _logger.LogInfo("Forcing current map to " + RandomizerMapComponent.ForceCurrentMapIdentifier);
            TrackerMap forcedMap = _trackerMapProvider.GetTrackerMap(RandomizerMapComponent.ForceCurrentMapIdentifier);
            if (forcedMap != null)
            {
                return forcedMap;
            }
            _logger.LogWarning("Could not find forced map with identifier " + RandomizerMapComponent.ForceCurrentMapIdentifier);
        }

        string currentLocation = PlayerState.CurrLocation;
        TrackerMap map = null;
        switch (currentLocation)
        {
            case "Cave":
                map = HandleCaveLocation();
                break;
            case "Dungeon1":
                map = _trackerMapProvider.GetTrackerMap("Dungeon1");
                break;
            case "Dungeon2":
                map = _trackerMapProvider.GetTrackerMap("Dungeon2");
                break;
            case "Dungeon3":
                map = _trackerMapProvider.GetTrackerMap("Dungeon3");
                break;
            case "Temple1":
                map = _trackerMapProvider.GetTrackerMap("CrystalGroveTemple");
                break;
            case "Temple2":
                map = _trackerMapProvider.GetTrackerMap("DesertTemple");
                break;
            case "Temple3":
                map = _trackerMapProvider.GetTrackerMap("SunkenTemple");
                break;
            case "Overworld":
                map = _trackerMapProvider.GetTrackerMap("Overworld");
                break;
            default:
                map = null;
                break;
        }

        return map;
    }

    private TrackerMap HandleCaveLocation()
    {
        string currentCheckpoint = PlayerState.CurrCheckpoint;
        string lastTransitionName = global::Transition.LastTransition?.name;

        if (currentCheckpoint == "CaveCheckpoint0")
        {
            return _trackerMapProvider.GetTrackerMap("StartingGrotto");
        }
        
        if (_transitionToLocationMap.ContainsKey(lastTransitionName))
        {
            return _trackerMapProvider.GetTrackerMap(_transitionToLocationMap[lastTransitionName]);
        }

        return null;
    }

    public bool IsCurrentMapAvailable()
    {
        if (!_randomizerEngine.IsRandomized())
        {
            // Vanilla behavior
            return LocationManager.Current != null && LocationManager.Current.Id == "Overworld";
        }
        TrackerMap currentMap = GetCurrentMap();

        return currentMap != null;
    }
}
