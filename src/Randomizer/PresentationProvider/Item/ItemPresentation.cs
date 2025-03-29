namespace MinishootRandomizer;

public class ItemPresentation
{
    private Item _item;
    private string _name;
    private string _description = "";
    private SpriteData _spriteData = null;

    public Item Item => _item;
    public string Name => _name;
    public string Description => _description;
    public SpriteData SpriteData => _spriteData;

    public ItemPresentation(Item item, string name, string description = "", SpriteData spriteData = null)
    {
        _item = item;
        _name = name;
        _description = description;
        _spriteData = spriteData;
    }
}
