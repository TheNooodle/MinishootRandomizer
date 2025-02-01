using System;

namespace MinishootRandomizer;

public class ReceivedItem : Item
{
    private readonly Item _innerItem;
    private readonly string _sender;

    public ReceivedItem(string identifier, ItemCategory category, Item innerItem, string sender) : base(identifier, category)
    {
        _innerItem = innerItem;
        _sender = sender;
    }

    public static ReceivedItem Create(Item item, string sender)
    {
        return new ReceivedItem(item.Identifier + " from " + sender, item.Category, item, sender);
    }

    public string Sender => _sender;

    public override void Collect()
    {
        _innerItem.Collect();
    }

    public override string GetSpriteIdentifier()
    {
        return _innerItem.GetSpriteIdentifier();
    }

    public override int GetOwnedQuantity()
    {
        throw new Exception("ReceivedItem does not have owned quantity");
    }
}
