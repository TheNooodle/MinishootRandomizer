namespace MinishootRandomizer;

public class RaceListener
{
    private readonly IRandomizerEngine _randomizerEngine;
    private readonly ILogger _logger;

    public RaceListener(IRandomizerEngine randomizerEngine, ILogger logger)
    {
        _randomizerEngine = randomizerEngine;
        _logger = logger ?? new NullLogger();
    }

    public void OnRaceWon(int raceIndex)
    {
        // If SpiritSanity is not enabled, we must re-establish the vanilla behavior
        SpiritSanity spiritSanity = _randomizerEngine.GetSetting<SpiritSanity>();
        if (!spiritSanity.Enabled)
        {
            WorldState.Set("NpcTiny" + raceIndex, true);
            return;
        }

        // @TODO: Implement SpiritSanity behavior
        _logger.LogWarning($"Race {raceIndex} won, but SpiritSanity is not implemented yet.");
    }
}
