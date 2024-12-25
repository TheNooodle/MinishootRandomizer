namespace MinishootRandomizer;

public class NpcItem : Item
{
    private NpcIds _npcId;

    public NpcIds NpcId => _npcId;

    public NpcItem(string identifier, ItemCategory itemCategory, NpcIds npcId) : base(identifier, itemCategory)
    {
        _npcId = npcId;
    }

    public override void Collect()
    {
        WorldState.Set(_npcId.Str(), true);
        ReflectionHelper.InvokeStaticAction(typeof(CrystalNpc), "Freed");
    }

    public override string GetSpriteIdentifier()
    {
        return Identifier + " NPC";
    }

    public override int GetOwnedQuantity()
    {
        return WorldState.Get(_npcId.Str()) ? 1 : 0;
    }
}
