namespace MinishootRandomizer;

public class CoreItemPresentationProvider : IItemPresentationProvider
{
    private readonly ISpriteProvider _spriteProvider;
    private readonly IRandomizerEngine _randomizerEngine;

    public CoreItemPresentationProvider(
        ISpriteProvider spriteProvider,
        IRandomizerEngine randomizerEngine
    ) {
        _spriteProvider = spriteProvider;
        _randomizerEngine = randomizerEngine;
    }

    public ItemPresenation GetItemPresentation(Item item)
    {
        string spriteIdentifier = item.GetSpriteIdentifier();

        // For ArchipelagoItems, we need to adjust the sprite depending on settings and item category.
        if (item is ArchipelagoItem)
        {
            ShowArchipelagoItemCategory setting = _randomizerEngine.GetSetting<ShowArchipelagoItemCategory>();
            if (setting.Enabled)
            {
                switch (item.Category)
                {
                    case ItemCategory.Progression:
                    case ItemCategory.Token:
                        spriteIdentifier = "ArchipelagoArrowUp";
                        break;
                    case ItemCategory.Filler:
                    case ItemCategory.Trap:
                        spriteIdentifier = "ArchipelagoGrayscale";
                        break;
                    default:
                        spriteIdentifier = "Archipelago";
                        break;
                }
            }
        }

        ItemPresenation itemPresenation = new ItemPresenation(
            item,
            item.Identifier,
            "",
            _spriteProvider.GetSprite(spriteIdentifier)
        );

        return itemPresenation;
    }
}
