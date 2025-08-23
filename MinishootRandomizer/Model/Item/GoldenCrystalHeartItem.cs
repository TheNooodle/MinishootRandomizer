namespace MinishootRandomizer;

public class GoldenCrystalHeartItem : Item
{
    public GoldenCrystalHeartItem(string identifier, ItemCategory itemCategory) : base(identifier, itemCategory)
    {}

    public override void Collect()
    {
        // No-op
    }

    public override string GetSpriteIdentifier()
    {
        return "GoldenCrystalShard";
    }

    public override int GetOwnedQuantity()
    {
        return 0;
    }
}
