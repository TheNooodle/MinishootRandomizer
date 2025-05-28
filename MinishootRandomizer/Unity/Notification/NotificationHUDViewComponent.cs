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
    private AdjustImageComponent _imageAdjuster;
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
        _imageAdjuster = GetComponentInChildren<AdjustImageComponent>();
        _image = _imageAdjuster.GetComponent<Image>();
        _text = GetComponentInChildren<TextMeshProUGUI>();
        _parentRectTransform = transform.parent.GetComponent<RectTransform>();
        _currentRectTransform = GetComponent<RectTransform>();
        _imageRectTransform = _imageAdjuster.GetComponent<RectTransform>();
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
        _imageAdjuster.MaxWidth = _currentRectTransform.rect.width * _imageWidthRatio;
        _imageAdjuster.MaxHeight = _currentRectTransform.rect.height;
        _imageAdjuster.AdjustImageSize();
    }

    public void NotifyItemCollection(Item item)
    {
        _item = item;
        ItemPresentation itemPresentation = _itemPresentationProvider.GetItemPresentation(item);
        if (itemPresentation is TrapItemPresentation trapItemPresentation)
        {
            itemPresentation = trapItemPresentation.TrueItemPresentation;
        }
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
