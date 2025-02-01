using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MinishootRandomizer;

public class NotificationHUDViewComponent : MonoBehaviour
{
    private IItemPresentationProvider _itemPresentationProvider;

    private Item _item = null;
    private float _totalWidthRatio = 0.25f;
    private float _imageWidthRatio = 0.2f;
    private float _totalHeight = 100f;

    private FadingAnimationHUDComponent _fadingAnimation;
    private Image _image;
    private TextMeshProUGUI _text;
    private RectTransform _parentRectTransform;
    private RectTransform _currentRectTransform;
    private RectTransform _imageRectTransform;
    private RectTransform _textRectTransform;

    void Awake()
    {
        _itemPresentationProvider = Plugin.ServiceContainer.Get<IItemPresentationProvider>();
        _fadingAnimation = GetComponent<FadingAnimationHUDComponent>();
        _image = GetComponentInChildren<Image>();
        _text = GetComponentInChildren<TextMeshProUGUI>();
        _parentRectTransform = transform.parent.GetComponent<RectTransform>();
        _currentRectTransform = GetComponent<RectTransform>();
        _imageRectTransform = _image.GetComponent<RectTransform>();
        _textRectTransform = _text.GetComponent<RectTransform>();
    }

    void Update()
    {
        AdjustSize();
    }

    private void AdjustSize()
    {
        _currentRectTransform.sizeDelta = new Vector2(_parentRectTransform.rect.width * _totalWidthRatio, _totalHeight);

        // For the text, we have to adjust the text size.
        _textRectTransform.sizeDelta = new Vector2(_currentRectTransform.rect.width - _imageRectTransform.rect.width, _currentRectTransform.rect.height);

        if (_item == null)
        {
            return;
        }

        // For the image, we also have to adjust the image size.
        Vector2 maxSize = new Vector2(_currentRectTransform.rect.width * _imageWidthRatio, _currentRectTransform.rect.height);
        Vector2 spriteSize = new Vector2(_image.sprite.rect.width, _image.sprite.rect.height);
        Vector2 adjustedSize = spriteSize;

        if (spriteSize.x > maxSize.x || spriteSize.y > maxSize.y)
        {
            float widthRatio = maxSize.x / spriteSize.x;
            float heightRatio = maxSize.y / spriteSize.y;
            float minRatio = Mathf.Min(widthRatio, heightRatio);
            adjustedSize = spriteSize * minRatio;
        }

        _imageRectTransform.sizeDelta = adjustedSize;
    }

    public void NotifyItemCollection(Item item)
    {
        _item = item;
        ItemPresenation itemPresentation = _itemPresentationProvider.GetItemPresentation(item);
        _text.SetText(itemPresentation.Name);
        _image.sprite = itemPresentation.SpriteData.Sprite;

        AdjustSize();
        _fadingAnimation.BeginFadeIn();
    }

    public bool IsBusy()
    {
        return _fadingAnimation.IsAnimating;
    }
}
