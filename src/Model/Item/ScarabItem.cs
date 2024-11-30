namespace MinishootRandomizer;

public class ScarabItem : Item
{
    public ScarabItem(string identifier, ItemCategory itemCategory) : base(identifier, itemCategory)
    {}

    public override void Collect()
    {
        PlayerState.SetGameStats(GameStatsId.ScarabCaught, 1f);
        PlayerState.SetCurrency(Currency.Scarab, 1);
        ReflectionHelper.InvokeStaticAction(typeof(ScarabPickup), "Collected");
    }

    public override string GetSpriteIdentifier()
    {
        return "Scarab";
    }
}
