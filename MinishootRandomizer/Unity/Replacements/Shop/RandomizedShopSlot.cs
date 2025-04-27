namespace MinishootRandomizer;

public class RandomizedShopSlot
{
    private Location _location;
    private Item _item;
    private int _price;
    private Currency _currency;

    public Location Location => _location;
    public Item Item => _item;
    public int Price => _price;
    public Currency Currency => _currency;

    public RandomizedShopSlot(Location location, Item item, int price, Currency currency)
    {
        _location = location;
        _item = item;
        _price = price;
        _currency = currency;
    }
}
