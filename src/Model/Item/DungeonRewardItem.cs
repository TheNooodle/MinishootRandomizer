namespace MinishootRandomizer;

public class DungeonRewardItem : Item
{
    public int DungeonId { get; private set; }

    public DungeonRewardItem(string identifier, ItemCategory category, int dungeonId) : base(identifier, category)
    {
        DungeonId = dungeonId;
    }

    public override void Collect()
    {
        // no-op
    }

    public override int GetOwnedQuantity()
    {
        return WorldState.Get($"Dungeon{DungeonId}CrystalBoss") ? 1 : 0;
    }
}
