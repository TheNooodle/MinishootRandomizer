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
        int obtainedQuantity = GetObtainedQuantity();
        WorldState.Set($"ObtainedD{DungeonId}SmallKey{obtainedQuantity}", true);
        SaveManager.SaveSlot();
    }

    public override string GetSpriteIdentifier()
    {
        return "Small Key";
    }

    public override int GetOwnedQuantity()
    {
        return GetObtainedQuantity();
    }

    private int GetObtainedQuantity()
    {
        int count = 0;
        string key = $"ObtainedD{DungeonId}SmallKey{count}";
        while (WorldState.Get(key))
        {
            count++;
            key = $"ObtainedD{DungeonId}SmallKey{count}";
        }

        return count;
    }
}
