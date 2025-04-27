using UnityEngine;

namespace MinishootRandomizer;

public class SpriteBasedFactory : IGameObjectFactory
{
    private readonly IGameObjectFactory _innerFactory;
    private readonly IItemPresentationProvider _itemPresentationProvider;
    private readonly ILogger _logger = new NullLogger();

    public SpriteBasedFactory(IGameObjectFactory innerFactory, IItemPresentationProvider itemPresentationProvider, ILogger logger = null)
    {
        _innerFactory = innerFactory;
        _itemPresentationProvider = itemPresentationProvider;
        _logger = logger ?? new NullLogger();
    }

    public GameObject CreateGameObject(string prefabIdentifier)
    {
        return _innerFactory.CreateGameObject(prefabIdentifier);
    }

    public GameObject CreateGameObjectForItem(Item item)
    {
        GameObject gameObject = _innerFactory.CreateGameObjectForItem(item);
        FloatyAnimationComponent animationComponent = gameObject.AddComponent<FloatyAnimationComponent>();
        RandomizerPickup pickupComponent = gameObject.GetComponent<RandomizerPickup>();
        animationComponent.SetSpeed(2.0f);
        animationComponent.SetAmplitude(0.2f);
        SpriteData itemSpriteData;
        try
        {
            ItemPresentation itemPresentation = _itemPresentationProvider.GetItemPresentation(item);
            itemSpriteData = itemPresentation.SpriteData;
            pickupComponent.ItemPresentation = itemPresentation;
        }
        catch (SpriteNotFound)
        {
            _logger.LogWarning($"Sprite not found for {item.Identifier}");
            return gameObject;
        }

        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            itemSpriteData.ApplyTo(spriteRenderer);
        }
        else
        {
            Transform spriteTransform = gameObject.transform.Find("Sprite(Clone)");
            if (spriteTransform == null)
            {
                _logger.LogWarning($"No Sprite object was found for {item.Identifier}");
                return gameObject;
            }

            spriteRenderer = spriteTransform.GetComponent<SpriteRenderer>();
            if (spriteRenderer == null)
            {
                _logger.LogWarning($"No SpriteRenderer was found for {item.Identifier}");
                return gameObject;
            }

            itemSpriteData.ApplyTo(spriteRenderer);
        }

        return gameObject;
    }
}
