namespace MinishootRandomizer;

public class TowerHandler
{
    private readonly IRandomizerEngine _randomizerEngine;

    public TowerHandler(IRandomizerEngine randomizerEngine)
    {
        _randomizerEngine = randomizerEngine;
    }

    public int GetSpiritCount()
    {
        if (!_randomizerEngine.IsRandomized())
        {
            return 8; // Default count if not randomized
        }

        SpiritTowerRequirement requirement = _randomizerEngine.GetSetting<SpiritTowerRequirement>();
        return requirement.Value;
    }
}
