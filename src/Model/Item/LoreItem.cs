namespace MinishootRandomizer;

public class LoreItem : Item
{
    public LoreItem(string identifier, ItemCategory itemCategory) : base(identifier, itemCategory)
    {}

    public override void Collect()
    {
        PlayerState.SetCurrency(Currency.Lore, 1);
    }
}
