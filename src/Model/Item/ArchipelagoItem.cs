namespace MinishootRandomizer;

public class ArchipelagoItem : Item
{
    public ArchipelagoItem(string identifier, ItemCategory category) : base(identifier, category)
    {
    }

    public override void Collect()
    {
        // no-op
    }

    public override string GetSpriteIdentifier()
    {
        return "Archipelago";
    }
}
