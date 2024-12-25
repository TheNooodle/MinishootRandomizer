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
        // @TODO: Implement this method
        throw new System.NotImplementedException();
    }

    public override int GetOwnedQuantity()
    {
        // @TODO: Implement this method
        return 0;
    }
}
