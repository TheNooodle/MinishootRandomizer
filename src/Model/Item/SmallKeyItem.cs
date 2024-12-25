namespace MinishootRandomizer;

public class SmallKeyItem : Item
{
    public int DungeonId { get; private set; }

    public SmallKeyItem(string identifier, ItemCategory category, int dungeonId) : base(identifier, category)
    {
        DungeonId = dungeonId;
    }

    public override void Collect()
    {
        PlayerState.DungeonKeys[DungeonId] += 1;
        ReflectionHelper.InvokeStaticAction(typeof(PlayerState), "KeysChanged");
        SaveManager.SaveSlot();
    }

    public override string GetSpriteIdentifier()
    {
        return "Small Key";
    }

    public override int GetOwnedQuantity()
    {
        return PlayerState.DungeonKeys[DungeonId];
    }
}
