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
}
