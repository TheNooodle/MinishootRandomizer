namespace MinishootRandomizer;

public class ScarabItem : Item
{
    public ScarabItem(string identifier, ItemCategory itemCategory) : base(identifier, itemCategory)
    {}

    public override void Collect()
    {
        PlayerState.SetCurrency(Currency.Scarab, 1);
    }
}
