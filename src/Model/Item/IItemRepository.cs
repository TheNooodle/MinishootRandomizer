namespace MinishootRandomizer;

public interface IItemRepository
{
    public Item Get(string identifier);
}

public class ItemNotFoundException : System.Exception
{
    private readonly string _itemName;

    public string ItemName => _itemName;

    public ItemNotFoundException(string itemName) : base($"Item with identifier {itemName} not found")
    {
        _itemName = itemName;
    }
}
