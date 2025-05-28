using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MinishootRandomizer;

public class CoreKeyCounterObjectFactory : IKeyCounterObjectFactory
{
    private readonly IObjectFinder _objectFinder;
    private readonly ITextGameObjectFactory _textGameObjectFactory;
    private readonly ISpriteProvider _spriteProvider;

    private readonly ICachePool<SpriteData> _smallKeySpriteCache;
    private readonly ICachePool<SpriteData> _bossKeySpriteCache;

    private static Dictionary<int, int> _dungeonIdToSmallKeyCount = new Dictionary<int, int>()
    {
        { 1, 4 },
        { 2, 4 },
        { 3, 5 },
    };

    public CoreKeyCounterObjectFactory(
        IObjectFinder objectFinder,
        ITextGameObjectFactory textGameObjectFactory,
        ISpriteProvider spriteProvider
    )
    {
        _objectFinder = objectFinder;
        _textGameObjectFactory = textGameObjectFactory;
        _spriteProvider = spriteProvider;

        _smallKeySpriteCache = new StandardCachePool<SpriteData>(
            new SingleCacheStorage<SpriteData>()
        );
        _bossKeySpriteCache = new StandardCachePool<SpriteData>(
            new SingleCacheStorage<SpriteData>()
        );
    }

    public GameObject CreateKeyCounterGameObject()
    {
        // TODO: temp
        GameObject parent = _objectFinder.FindObject(new ByComponent(typeof(Map)));
        if (parent == null)
        {
            throw new Exception("Map not found!");
        }

        GameObject keyCounterViewObject = new GameObject("KeyCounterViewHUD");
        keyCounterViewObject.transform.SetParent(parent.transform, false);
        RectTransform rectTransform = keyCounterViewObject.AddComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0.0f, 1.0f);
        rectTransform.anchorMax = new Vector2(0.0f, 1.0f);
        rectTransform.pivot = new Vector2(0.0f, 1.0f);
        rectTransform.anchoredPosition = new Vector2(45.0f, -170.0f);
        rectTransform.sizeDelta = new Vector2(100.0f, 80.0f);

        VerticalLayoutGroup layout = keyCounterViewObject.AddComponent<VerticalLayoutGroup>();
        layout.spacing = 0.0f;
        layout.childAlignment = TextAnchor.UpperCenter;
        layout.childControlWidth = false;
        layout.childControlHeight = false;

        foreach (KeyValuePair<int, int> dungeon in _dungeonIdToSmallKeyCount)
        {
            CreateKeyCounterGameObject(
                keyCounterViewObject,
                dungeon.Key,
                dungeon.Value
            );
        }

        return keyCounterViewObject;
    }

    private GameObject CreateKeyCounterGameObject(GameObject parent, int dungeonId, int smallKeyCount)
    {
        GameObject groupObject = BuildGroupObject(parent, dungeonId);
        BuildLabelObject(groupObject, dungeonId);
        GameObject smallKeyGroupObject = BuildSmallKeyGroupObject(groupObject);
        for (int i = 0; i < smallKeyCount; i++)
        {
            BuildSmallKeySpriteObject(smallKeyGroupObject, i);
        }
        BuildBossKeySpriteObject(groupObject);

        return groupObject;
    }

    private GameObject BuildGroupObject(GameObject parent, int dungeonId)
    {
        GameObject keyCounterViewObject = new GameObject("KeyCounterViewD" + dungeonId);
        keyCounterViewObject.transform.SetParent(parent.transform, false);
        keyCounterViewObject.AddComponent<RectTransform>();

        HorizontalLayoutGroup layout = keyCounterViewObject.AddComponent<HorizontalLayoutGroup>();
        layout.spacing = 0.0f;
        layout.childAlignment = TextAnchor.MiddleLeft;
        layout.childControlWidth = false;
        layout.childControlHeight = false;

        return keyCounterViewObject;
    }

    private GameObject BuildLabelObject(GameObject parent, int dungeonId)
    {
        GameObject labelObject = _textGameObjectFactory.CreateTextGameObject("D" + dungeonId);
        labelObject.transform.SetParent(parent.transform, false);
        RectTransform textRectTransform = labelObject.GetComponent<RectTransform>();
        textRectTransform.anchorMin = new Vector2(0.0f, 0.0f);
        textRectTransform.anchorMax = new Vector2(0.0f, 0.0f);
        textRectTransform.sizeDelta = new Vector2(40.0f, 30.0f);
        TextMeshProUGUI textComponent = labelObject.GetComponent<TextMeshProUGUI>();
        textComponent.verticalAlignment = VerticalAlignmentOptions.Middle;

        return labelObject;
    }

    private GameObject BuildSmallKeyGroupObject(GameObject parent)
    {
        GameObject smallKeyGroupObject = new GameObject("SmallKeyGroup");
        smallKeyGroupObject.transform.SetParent(parent.transform, false);
        RectTransform smallKeyGroupRectTransform = smallKeyGroupObject.AddComponent<RectTransform>();
        smallKeyGroupRectTransform.sizeDelta = new Vector2(200.0f, 80.0f);

        HorizontalLayoutGroup layout = smallKeyGroupObject.AddComponent<HorizontalLayoutGroup>();
        layout.spacing = -25.0f;
        layout.childAlignment = TextAnchor.MiddleLeft;
        layout.childControlWidth = false;
        layout.childControlHeight = false;
        layout.childForceExpandWidth = false;

        return smallKeyGroupObject;
    }

    private GameObject BuildSmallKeySpriteObject(GameObject parent, int index)
    {
        SpriteData spriteData = _smallKeySpriteCache.Get("SmallKeySprite", () =>
            {
                return new CacheItem<SpriteData>(_spriteProvider.GetSprite("Small Key"));
            }
        );

        GameObject spriteObject = new GameObject("SmallKeySprite" + index);
        spriteObject.transform.SetParent(parent.transform, false);
        RectTransform spriteRectTransform = spriteObject.AddComponent<RectTransform>();
        spriteRectTransform.pivot = new Vector2(0.5f, 0.5f);
        spriteRectTransform.sizeDelta = new Vector2(60.0f, 30.0f);
        spriteRectTransform.anchoredPosition = new Vector2(250f, -50f);
        spriteRectTransform.eulerAngles = new Vector3(0.0f, 0.0f, 270.0f);
        spriteObject.AddComponent<CanvasRenderer>();
        Image spriteImage = spriteObject.AddComponent<Image>();
        spriteImage.sprite = spriteData.Sprite;
        AdjustImageComponent imageAdjuster = spriteObject.AddComponent<AdjustImageComponent>();
        imageAdjuster.AdjustOnUpdate = false;

        return spriteObject;
    }

    private GameObject BuildBossKeySpriteObject(GameObject parent)
    {
        SpriteData spriteData = _bossKeySpriteCache.Get("BossKeySprite", () =>
            {
                return new CacheItem<SpriteData>(_spriteProvider.GetSprite("Boss Key"));
            }
        );

        GameObject spriteObject = new GameObject("BossKeySprite");
        spriteObject.transform.SetParent(parent.transform, false);
        RectTransform spriteRectTransform = spriteObject.AddComponent<RectTransform>();
        spriteRectTransform.pivot = new Vector2(0.5f, 0.5f);
        spriteRectTransform.sizeDelta = new Vector2(50.0f, 50.0f);
        spriteObject.AddComponent<CanvasRenderer>();
        Image spriteImage = spriteObject.AddComponent<Image>();
        spriteImage.sprite = spriteData.Sprite;
        AdjustImageComponent imageAdjuster = spriteObject.AddComponent<AdjustImageComponent>();
        imageAdjuster.AdjustOnUpdate = false;

        return spriteObject;
    }
}
