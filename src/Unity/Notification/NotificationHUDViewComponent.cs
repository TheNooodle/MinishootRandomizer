using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MinishootRandomizer;

public class NotificationHUDViewComponent : MonoBehaviour
{
    private IItemPresentationProvider _itemPresentationProvider;

    private FadingAnimationHUDComponent _fadingAnimation;
    private Image _image;
    private TextMeshProUGUI _text;

    void Awake()
    {
        _itemPresentationProvider = Plugin.ServiceContainer.Get<IItemPresentationProvider>();
        _fadingAnimation = GetComponent<FadingAnimationHUDComponent>();
        _image = GetComponentInChildren<Image>();
        _text = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void NotifyItemCollection(Item item)
    {
        ItemPresenation itemPresentation = _itemPresentationProvider.GetItemPresentation(item);
        _text.SetText(itemPresentation.Name);

        _image.sprite = itemPresentation.SpriteData.Sprite;
        // For the image, we also have to adjust the image size.
        RectTransform rectTransform = _image.GetComponent<RectTransform>();
        Vector2 maxSize = new Vector2(100.0f, 100.0f);
        Vector2 spriteSize = new Vector2(_image.sprite.rect.width, _image.sprite.rect.height);
        Vector2 adjustedSize = spriteSize;

        if (spriteSize.x > maxSize.x || spriteSize.y > maxSize.y)
        {
            float widthRatio = maxSize.x / spriteSize.x;
            float heightRatio = maxSize.y / spriteSize.y;
            float minRatio = Mathf.Min(widthRatio, heightRatio);
            adjustedSize = spriteSize * minRatio;
        }

        rectTransform.sizeDelta = adjustedSize;
        _fadingAnimation.BeginFadeIn();
    }

    public bool IsBusy()
    {
        return _fadingAnimation.IsAnimating;
    }
}
