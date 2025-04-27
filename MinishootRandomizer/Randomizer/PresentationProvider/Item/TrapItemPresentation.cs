namespace MinishootRandomizer;

public class TrapItemPresentation : ItemPresentation
{
    private ItemPresentation _trueItemPresentation;

    public ItemPresentation TrueItemPresentation => _trueItemPresentation;

    public TrapItemPresentation(Item item, string name, string description = "", SpriteData spriteData = null) : base(item, name, description, spriteData)
    {
    }

    public void SetTrueItemPresentation(ItemPresentation trueItemPresentation)
    {
        _trueItemPresentation = trueItemPresentation;
    }
}
