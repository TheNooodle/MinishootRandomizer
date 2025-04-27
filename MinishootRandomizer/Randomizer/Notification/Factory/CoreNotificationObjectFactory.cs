using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MinishootRandomizer;

public class CoreNotificationObjectFactory : INotificationObjectFactory
{
    private readonly IObjectFinder _objectFinder;

    public CoreNotificationObjectFactory(IObjectFinder objectFinder)
    {
        _objectFinder = objectFinder;
    }

    public GameObject CreateNotificationView()
    {
        GameObject parent = _objectFinder.FindObject(new ByComponent(typeof(HUD)));
        if (parent == null)
        {
            throw new Exception("HUD not found!");
        }

        GameObject notificationViewObject = BuildNotificationObject(parent);
        BuildNotificationSprite(notificationViewObject);
        BuildNotificationText(notificationViewObject);
        notificationViewObject.AddComponent<FadingAnimationHUDComponent>();
        notificationViewObject.AddComponent<NotificationHUDViewComponent>();

        return notificationViewObject;
    }

    private GameObject BuildNotificationObject(GameObject parent)
    {
        GameObject notificationViewObject = new GameObject("RandomizerNotificationViewHUD");
        notificationViewObject.transform.SetParent(parent.transform, false);
        RectTransform rectTransform = notificationViewObject.AddComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(1.0f, 0.0f);
        rectTransform.anchorMax = new Vector2(1.0f, 0.0f);
        rectTransform.pivot = new Vector2(1.0f, 0.0f);
        rectTransform.anchoredPosition = new Vector2(-100.0f, 100.0f);

        HorizontalLayoutGroup layout = notificationViewObject.AddComponent<HorizontalLayoutGroup>();
        layout.spacing = 50.0f;
        layout.childAlignment = TextAnchor.MiddleLeft;
        layout.childControlWidth = false;
        layout.childControlHeight = false;

        return notificationViewObject;
    }

    private GameObject BuildNotificationSprite(GameObject notificationViewObject)
    {
        GameObject spriteObject = new GameObject("Sprite");
        spriteObject.transform.SetParent(notificationViewObject.transform, false);
        RectTransform spriteRectTransform = spriteObject.AddComponent<RectTransform>();
        spriteRectTransform.pivot = new Vector2(0.5f, 0.5f);
        spriteObject.AddComponent<CanvasRenderer>();
        spriteObject.AddComponent<Image>();

        return spriteObject;
    }

    private GameObject BuildNotificationText(GameObject notificationViewObject)
    {
        GameObject textObject = new GameObject("Text");
        textObject.transform.SetParent(notificationViewObject.transform, false);
        RectTransform textRectTransform = textObject.AddComponent<RectTransform>();
        textRectTransform.anchorMin = new Vector2(0.0f, 0.0f);
        textRectTransform.anchorMax = new Vector2(0.0f, 0.0f);
        textObject.AddComponent<CanvasRenderer>();
        TextMeshProUGUI text = textObject.AddComponent<TextMeshProUGUI>();
        text.verticalAlignment = VerticalAlignmentOptions.Middle;
        text.font = GetFont();
        text.fontMaterial = GetFontMaterial();
        text.UpdateFontAsset();

        return textObject;
    }

    private TMP_FontAsset GetFont()
    {
        TMP_FontAsset[] fonts = Resources.FindObjectsOfTypeAll<TMP_FontAsset>();
        foreach (TMP_FontAsset font in fonts)
        {
            if (font.name == "Bombard")
            {
                return font;
            }
        }

        throw new Exception("Font not found!");
    }

    private Material GetFontMaterial()
    {
        Material[] materials = Resources.FindObjectsOfTypeAll<Material>();
        foreach (Material material in materials)
        {
            if (material.name == "BOMBARD - OutlineLite")
            {
                return material;
            }
        }

        throw new Exception("Material not found!");
    }
}
