namespace MinishootRandomizer;

public class TrapItem : Item
{
    public TrapItem(string identifier, ItemCategory category) : base(identifier, category)
    {
    }

    public override void Collect()
    {
        // no-op, the effect will be handled asynchronously via event listeners.
    }

    public override string GetSpriteIdentifier()
    {
        return "PrimordialScarabDialog";
    }

    public override bool CanServeAsATrap()
    {
        return false;
    }
}
