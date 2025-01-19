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
        // Note that the sprite might be different depending on settings, cf ItemPresentationProvider.
        return "Archipelago";
    }

    public override int GetOwnedQuantity()
    {
        // An ArchipelagoItem cannot be owned, as it belong to another world.
        return 0;
    }
}
