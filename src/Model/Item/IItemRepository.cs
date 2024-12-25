using System.Collections.Generic;

namespace MinishootRandomizer;

public interface IItemRepository
{
    public Item Get(string identifier);
    public List<Item> GetAll();
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
