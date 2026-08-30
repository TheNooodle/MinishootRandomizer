using System;
using UnityEngine;
using UnityEngine.UI;

namespace MinishootRandomizer;

public class CoreItemViewFactory : IItemViewFactory
{
    private readonly IObjectFinder _objectFinder;
    private readonly IItemRepository _itemRepository;
    private readonly ISpriteProvider _spriteProvider;

    public CoreItemViewFactory(IObjectFinder objectFinder, IItemRepository itemRepository, ISpriteProvider spriteProvider)
    {
        _objectFinder = objectFinder;
        _itemRepository = itemRepository;
        _spriteProvider = spriteProvider;
    }

    public GameObject CreateItemViewObject(ItemView itemView)
    {
        GameObject parent = _objectFinder.FindObject(itemView.ParentSelector);
        if (parent == null)
        {
            throw new Exception(itemView.ParentSelector + " not found for " + itemView.ItemIdentifier.RemoveSpace() + " view!");
        }

        GameObject itemViewObject = new GameObject(itemView.ItemIdentifier + "ItemViewHUD");
        itemViewObject.transform.SetParent(parent.transform, false);
        RectTransform rectTransform = itemViewObject.AddComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.pivot = new Vector2(0.5f, 0.5f);
        rectTransform.anchoredPosition = itemView.Offset;
        rectTransform.sizeDelta = itemView.Size;

        itemViewObject.AddComponent<CanvasRenderer>();
        Image spriteImage = itemViewObject.AddComponent<Image>();
        spriteImage.sprite = _spriteProvider.GetSprite(itemView.SpriteDataIdentifier).Sprite;
        ItemViewHUDComponent itemViewHUDComponent = itemViewObject.AddComponent<ItemViewHUDComponent>();
        itemViewHUDComponent.SetItem(_itemRepository.Get(itemView.ItemIdentifier));

        return itemViewObject;
    }
}
