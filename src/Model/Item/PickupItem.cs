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
        if (Stats == Stats.BulletNumber && Plugin.IsDebug)
        {
            PlayerState.SetStatsLevel(Stats.BulletNumber, 5);
            PlayerState.SetStatsLevel(Stats.BulletDamage, 6);
            PlayerState.SetStatsLevel(Stats.FireRate, 5);
            PlayerState.SetStatsLevel(Stats.FireRange, 5);
            PlayerState.SetStatsLevel(Stats.MoveSpeed, 3);
            PlayerState.SetStatsLevel(Stats.Energy, 8);
            PlayerState.SetStatsLevel(Stats.Hp, 20);
            PlayerState.SetStatsLevel(Stats.CriticChance, 3);
            PlayerState.SetStatsLevel(Stats.Supershot, 3);
            PlayerState.SetStatsLevel(Stats.BulletSpeed, 3);
            PlayerState.SetStatsLevel(Stats.BoostSpeed, 3);

            PlayerState.SetSkill(Skill.Boost, true);
            PlayerState.SetSkill(Skill.Dash, true);
            PlayerState.SetSkill(Skill.Supershot, true);
            PlayerState.SetSkill(Skill.Hover, true);

            WorldState.Set(NpcIds.Bard.Str(), true);
            ReflectionHelper.InvokeStaticAction(typeof(CrystalNpc), "Freed");

            PlayerState.SetUniqueKey(KeyUse.Darker, true);
            PlayerState.SetUniqueKey(KeyUse.FinalBoss, true);

            PlayerState.SetModule(Modules.SpiritDash, true);
            PlayerState.SetModule(Modules.PrimordialCrystal, true);

            PlayerState.SetStatsLevel(Stats.PowerAllyLevel, 3);
            PlayerState.SetStatsLevel(Stats.PowerBombLevel, 3);
            PlayerState.SetStatsLevel(Stats.PowerSlowLevel, 3);
            ReflectionHelper.InvokeStaticAction(typeof(StatsPickup), "PowerCollected");

            Player.Instance.UpdateStats(updateCurrentHP: true);
        }
        else
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
    }

    public override int GetOwnedQuantity()
    {
        return PlayerState.StatsLevel[Stats];
    }
}
