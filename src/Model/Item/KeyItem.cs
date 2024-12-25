namespace MinishootRandomizer;

public class KeyItem : Item
{
    private KeyUse _key;

    public KeyUse Key => _key;

    public KeyItem(string identifier, ItemCategory itemCategory, KeyUse key) : base(identifier, itemCategory)
    {
        _key = key;
    }

    public override void Collect()
    {
        PlayerState.SetUniqueKey(_key, true);
    }

    public override int GetOwnedQuantity()
    {
        return (PlayerState.UniqueKeys.TryGetValue(_key, out bool owned) && owned) ? 1 : 0;
    }
}
