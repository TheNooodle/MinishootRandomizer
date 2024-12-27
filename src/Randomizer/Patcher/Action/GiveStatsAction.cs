namespace MinishootRandomizer;

public class GiveStatsAction : IPatchAction
{
    private readonly Stats _stats;
    private readonly int _value;

    public GiveStatsAction(Stats stats, int value)
    {
        _stats = stats;
        _value = value;
    }

    public void Dispose()
    {
        // no-op
    }

    public void Patch()
    {
        if (PlayerState.StatsLevel[_stats] < _value)
        {
            PlayerState.StatsLevel[_stats] = _value;
            SaveManager.SaveSlot();
        }
    }

    public void Unpatch()
    {
        // no-op
    }
}
