using System.Collections.Generic;

namespace MinishootRandomizer;

public class PickupItem : Item
{
    public PickupItem(string identifier, ItemCategory itemCategory, Stats stats) : base(identifier, itemCategory)
    {
        Stats = stats;
    }

    public Stats Stats { get; }

    public override void Collect()
    {
        PlayerState.SetStatsLevel(Stats, 1);
        if (Stats == Stats.Hp)
        {
            Player.Instance.UpdateStats(updateCurrentHP: true);
        }
        else if (Stats == Stats.Energy)
        {
            Player.EnergyUpView.Pop();
        }
        else if (new List<Stats> { Stats.PowerAllyLevel, Stats.PowerBombLevel, Stats.PowerSlowLevel }.Contains(Stats))
        {
            ReflectionHelper.InvokeStaticAction(typeof(StatsPickup), "PowerCollected");
        }
    }

    public override int GetOwnedQuantity()
    {
        return PlayerState.StatsLevel[Stats];
    }
}
