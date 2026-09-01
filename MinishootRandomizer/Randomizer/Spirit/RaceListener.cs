namespace MinishootRandomizer;

public class RaceListener
{
    private readonly IRandomizerEngine _randomizerEngine;
    private readonly ILocationRepository _locationRepository;
    private readonly GameEventDispatcher _gameEventDispatcher;

    public RaceListener(IRandomizerEngine randomizerEngine, ILocationRepository locationRepository, GameEventDispatcher gameEventDispatcher)
    {
        _randomizerEngine = randomizerEngine;
        _locationRepository = locationRepository;
        _gameEventDispatcher = gameEventDispatcher;
    }

    public void OnRaceWon(int raceIndex)
    {
        // If ShuffleSpirits is not enabled, we must re-establish the vanilla behavior
        ShuffleSpirits shuffleSpirits = _randomizerEngine.GetSetting<ShuffleSpirits>();
        if (!shuffleSpirits.Enabled)
        {
            WorldState.Set("NpcTiny" + raceIndex, true);
            return;
        }

        // If ShuffleSpirits is enabled, we give the corresponding item.
        string locationName = SpiritLocation.IndexToNameMap[raceIndex];
        Location location = _locationRepository.Get(locationName);
        Item item = _randomizerEngine.CheckLocation(location);
        item.Collect();
        _gameEventDispatcher.DispatchItemCollected(item);
    }
}
