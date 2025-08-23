using System;

namespace MinishootRandomizer;

public class WorldStateProgressionStorage : IProgressionStorage
{
    public bool IsLocationChecked(Location location)
    {
        return WorldState.Get(location.Identifier);
    }

    public void SetLocationChecked(Location location, bool isChecked = true)
    {
        WorldState.Set(location.Identifier, isChecked);
    }

    public bool IsGoalCompleted(Goals goal)
    {
        return goal switch
        {
            Goals.Dungeon5 => WorldState.Get("Dungeon5 167 Boss4 T3 S3"),
            Goals.Snow => WorldState.Get("Snow 003 BossTrueLast T3 S3"),
            Goals.SpiritTower => WorldState.Get(Item.GoldenCrystalHeart),
            _ => throw new NotImplementedException($"Goal {goal} is not supported by WorldStateProgressionStorage!")
        };
    }
}
