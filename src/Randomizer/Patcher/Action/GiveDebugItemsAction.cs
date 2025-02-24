namespace MinishootRandomizer;

public class GiveDebugItemsAction : IPatchAction
{
    public void Dispose()
    {
        // no-op
    }

    public void Patch()
    {
        if (WorldState.Get("DebugItems"))
        {
            return;
        }

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

        PlayerState.DungeonKeys[1] = 4;
        WorldState.Set($"ObtainedD1SmallKey4", true);
        PlayerState.DungeonKeys[2] = 4;
        WorldState.Set($"ObtainedD2SmallKey4", true);
        PlayerState.DungeonKeys[3] = 5;
        WorldState.Set($"ObtainedD3SmallKey5", true);
        PlayerState.DungeonBossKeys[1] = 1;
        WorldState.Set($"ObtainedD1BossKey", true);
        PlayerState.DungeonBossKeys[2] = 1;
        WorldState.Set($"ObtainedD2BossKey", true);
        PlayerState.DungeonBossKeys[3] = 1;
        WorldState.Set($"ObtainedD3BossKey", true);
        ReflectionHelper.InvokeStaticAction(typeof(PlayerState), "KeysChanged");
        SaveManager.SaveSlot();

        WorldState.Set("DebugItems", true);
    }

    public void Unpatch()
    {
        // no-op
    }
}
