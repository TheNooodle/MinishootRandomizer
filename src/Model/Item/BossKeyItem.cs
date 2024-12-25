namespace MinishootRandomizer;

public class BossKeyItem : Item
{
    public int DungeonId { get; private set; }

    public BossKeyItem(string identifier, ItemCategory category, int dungeonId) : base(identifier, category)
    {
        DungeonId = dungeonId;
    }

    public override void Collect()
    {
        PlayerState.DungeonBossKeys[DungeonId] += 1;
        ReflectionHelper.InvokeStaticAction(typeof(PlayerState), "KeysChanged");
        SaveManager.SaveSlot();
    }

    public override string GetSpriteIdentifier()
    {
        return "Boss Key";
    }

    public override int GetOwnedQuantity()
    {
        return PlayerState.DungeonBossKeys[DungeonId];
    }
}
