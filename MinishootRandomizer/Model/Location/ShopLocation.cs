namespace MinishootRandomizer;

abstract public class ShopLocation : Location
{
    protected readonly int _defaultPrice;
    protected readonly Currency _defaultCurrency;

    public int DefaultPrice => _defaultPrice;
    public Currency DefaultCurrency => _defaultCurrency;

    protected ShopLocation(
        string identifier,
        string logicRule,
        LocationPool pool,
        int defaultPrice,
        Currency defaultCurrency
    ) : base(identifier, logicRule, pool)
    {
        _defaultPrice = defaultPrice;
        _defaultCurrency = defaultCurrency;
    }
}
