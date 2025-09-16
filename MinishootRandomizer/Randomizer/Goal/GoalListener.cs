namespace MinishootRandomizer;

public class GoalListener
{
    private readonly IRandomizerEngine _randomizerEngine;

    public GoalListener(IRandomizerEngine randomizerEngine)
    {
        _randomizerEngine = randomizerEngine;
    }

    public void OnItemCollected(Item item)
    {
        if (!_randomizerEngine.IsRandomized())
        {
            return;
        }

        CompletionGoals completionGoals = _randomizerEngine.GetSetting<CompletionGoals>();
        if (item.Identifier == Item.GoldenCrystalHeart && completionGoals.Goal == Goals.SpiritTower)
        {
            WorldState.Set(Item.GoldenCrystalHeart, true);
            _randomizerEngine.CompleteGoal(Goals.SpiritTower);
        }
    }
}
