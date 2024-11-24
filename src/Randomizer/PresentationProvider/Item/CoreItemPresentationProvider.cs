namespace MinishootRandomizer;

public class CoreItemPresentationProvider : IItemPresentationProvider
{
    private readonly ISpriteProvider _spriteProvider;

    public CoreItemPresentationProvider(ISpriteProvider spriteProvider)
    {
        _spriteProvider = spriteProvider;
    }

    public ItemPresenation GetItemPresentation(Item item)
    {
        string spriteIdentifier = item.GetSpriteIdentifier();
        ItemPresenation itemPresenation = new ItemPresenation(
            item,
            item.Identifier,
            "",
            _spriteProvider.GetSprite(spriteIdentifier)
        );

        return itemPresenation;
    }
}
