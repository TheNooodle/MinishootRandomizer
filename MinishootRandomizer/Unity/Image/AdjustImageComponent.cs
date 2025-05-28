using UnityEngine;
using UnityEngine.UI;

namespace MinishootRandomizer;

/// <summary>
/// Adjusts the size of an image component to fit within specified maximum width and height.
/// The image will maintain its aspect ratio while scaling down to fit within the limits.
/// </summary>
public class AdjustImageComponent : MonoBehaviour
{
    private Image _image;
    private RectTransform _imageRectTransform;

    public float MaxWidth = 100f;
    public float MaxHeight = 100f;
    public bool AdjustOnUpdate = true;

    void Awake()
    {
        _image = GetComponent<Image>();
        _imageRectTransform = _image.GetComponent<RectTransform>();
    }

    void Update()
    {
        if (AdjustOnUpdate)
        {
            AdjustImageSize();
        }
    }

    public void AdjustImageSize()
    {
        // For the image, we also have to adjust the image size.
        // Vector2 maxSize = new Vector2(MaxWidth, MaxHeight);
        // Vector2 spriteSize = new Vector2(_image.sprite.rect.width, _image.sprite.rect.height);
        // Vector2 adjustedSize = spriteSize;

        // if (spriteSize.x > maxSize.x || spriteSize.y > maxSize.y)
        // {
        //     float widthRatio = maxSize.x / spriteSize.x;
        //     float heightRatio = maxSize.y / spriteSize.y;
        //     float minRatio = Mathf.Min(widthRatio, heightRatio);
        //     adjustedSize = spriteSize * minRatio;
        // }

        // _imageRectTransform.sizeDelta = adjustedSize;
    }
}
