namespace MinishootRandomizer;

public class ArchipelagoItem : Item
{
    private readonly string _owner;

    public ArchipelagoItem(string identifier, ItemCategory category, string owner) : base(identifier, category)
    {
        _owner = owner;
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

    public override string GetName()
    {
        return $"{_owner}'s {Identifier}";
    }

    public override bool CanServeAsATrap()
    {
        // An ArchipelagoItem cannot be used as a trap.
        return false;
    }
}
