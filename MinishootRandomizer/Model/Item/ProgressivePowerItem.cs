namespace MinishootRandomizer;

public class ProgressivePowerItem : Item
{
    private readonly Stats _stats;
    private readonly Modules _module;

    public ProgressivePowerItem(string identifier, ItemCategory category, Stats stats, Modules module) : base(identifier, category)
    {
        _stats = stats;
        _module = module;
    }

    public override void Collect()
    {
        // Unlock the power level if it is not already unlocked.
        // If it is already unlocked, unlock the idol module instead.
        if (!PlayerState.StatsLevel.TryGetValue(_stats, out int level) || level < 1)
        {
            PlayerState.SetStatsLevel(_stats, 1);
            ReflectionHelper.InvokeStaticAction(typeof(StatsPickup), "PowerCollected");
        }
        else
        {
            PlayerState.SetModule(_module, true);
        }
    }

    public override int GetOwnedQuantity()
    {
        int count = 0;
        count += PlayerState.StatsLevel.TryGetValue(_stats, out int owned) && owned >= 1 ? 1 : 0;
        count += PlayerState.Modules.TryGetValue(_module, out bool owned2) && owned2 ? 1 : 0;

        return count;
    }
}
