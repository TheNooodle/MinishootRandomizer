namespace MinishootRandomizer;

public class CurrentMapHandler
{
    private ITrackerMapProvider _trackerMapProvider;
    private IRandomizerEngine _randomizerEngine;

    public CurrentMapHandler(ITrackerMapProvider trackerMapProvider, IRandomizerEngine randomizerEngine)
    {
        _trackerMapProvider = trackerMapProvider;
        _randomizerEngine = randomizerEngine;
    }

    public TrackerMap GetCurrentMap()
    {
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
        // @TODO: determine the actual cave the player is in.
        return _trackerMapProvider.GetTrackerMap("StartingGrotto");
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
