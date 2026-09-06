using UnityEngine;
using UnityEngine.UI;

namespace MinishootRandomizer;

public class LayoutTitleArranger : ITitleArranger
{
    private readonly IObjectFinder _objectFinder;
    private readonly ILogger _logger = new NullLogger();

    private bool _isArranged = false;

    public LayoutTitleArranger(IObjectFinder objectFinder, ILogger logger = null)
    {
        _objectFinder = objectFinder;
        _logger = logger ?? new NullLogger();
    }

    public GameObject ArrangeMapTitle()
    {
        GameObject mapObject = _objectFinder.FindObject(new ByComponent(typeof(Map)));
        if (mapObject == null)
        {
            _logger.LogError("Cannot arrange map title : Map object not found!");
            return null;
        }

        GameObject titleObject = GetTitleObject(mapObject);
        if (titleObject == null)
        {
            _logger.LogError("Cannot arrange map title : Title object not found!");
            return null;
        }

        GameObject progressObject = GetProgressObject(titleObject);
        if (progressObject == null)
        {
            _logger.LogError("Cannot arrange map title : Progress object not found!");
            return null;
        }

        return CreateLayoutObject(mapObject, titleObject, progressObject);
    }

    private GameObject CreateLayoutObject(GameObject mapObject, GameObject titleObject, GameObject progressObject)
    {
        GameObject layoutObject = new GameObject("TitleLayout");
        layoutObject.transform.SetParent(mapObject.transform, false);
        layoutObject.layer = 5;
        RectTransform layoutRect = layoutObject.AddComponent<RectTransform>();
        layoutRect.anchorMin = new Vector2(0.0f, 1.0f); // Top-left corner
        layoutRect.anchorMax = new Vector2(0.0f, 1.0f);
        layoutRect.pivot = new Vector2(0.0f, 1.0f);
        layoutRect.anchoredPosition = new Vector2(45.0f, -10.0f);
        layoutRect.sizeDelta = new Vector2(600.0f, 100.0f);
        layoutRect.SetAsLastSibling();

        VerticalLayoutGroup verticalLayoutGroup = layoutObject.AddComponent<VerticalLayoutGroup>();
        verticalLayoutGroup.spacing = -10.0f;
        verticalLayoutGroup.childAlignment = TextAnchor.UpperLeft;
        verticalLayoutGroup.childControlWidth = false;
        verticalLayoutGroup.childControlHeight = false;
        verticalLayoutGroup.childForceExpandWidth = false;
        verticalLayoutGroup.childForceExpandHeight = false;

        GameObject mapTitleLayoutObject = new GameObject("MapTitleLayout");
        mapTitleLayoutObject.layer = 5;
        mapTitleLayoutObject.AddComponent<RectTransform>();
        HorizontalLayoutGroup horizontalLayoutGroup = mapTitleLayoutObject.AddComponent<HorizontalLayoutGroup>();
        horizontalLayoutGroup.spacing = 20.0f;
        horizontalLayoutGroup.childAlignment = TextAnchor.MiddleLeft;
        horizontalLayoutGroup.childControlWidth = false;
        horizontalLayoutGroup.childControlHeight = false;
        horizontalLayoutGroup.childForceExpandWidth = false;
        horizontalLayoutGroup.childForceExpandHeight = false;

        // Reparent title and progress objects to the new layout object
        titleObject.transform.SetParent(mapTitleLayoutObject.transform, false);
        mapTitleLayoutObject.transform.SetParent(layoutObject.transform, false);
        progressObject.transform.SetParent(layoutObject.transform, false);

        // Create input prompts object
        GameObject previousMapPromptObject = new GameObject("PreviousMapPrompt");
        previousMapPromptObject.layer = 5;
        InputPrompt previousMapPrompt = previousMapPromptObject.AddComponent<InputPrompt>();
        ReflectionHelper.SetPrivateFieldValue(previousMapPrompt, "forDevice", InputDeviceType.All);
        ReflectionHelper.SetPrivateFieldValue(previousMapPrompt, "showText", false);
        ReflectionHelper.SetPrivateFieldValue(previousMapPrompt, "action", "PowerSlow");
        ReflectionHelper.SetPrivateFieldValue(previousMapPrompt, "sortingLayer", "UI");
        ReflectionHelper.SetPrivateFieldValue(previousMapPrompt, "sortingOrderBase", 150);
        DeviceManager.ChangedSettings += previousMapPrompt.UpdateView;
        RectTransform previousMapPromptRect = previousMapPromptObject.AddComponent<RectTransform>();
        previousMapPromptRect.SetScale(38f);
        previousMapPromptObject.AddComponent<LayoutElement>();
        previousMapPromptObject.AddComponent<ForceSizeDeltaComponent>();

        GameObject nextMapPromptObject = new GameObject("NextMapPrompt");
        nextMapPromptObject.layer = 5;
        InputPrompt nextMapPrompt = nextMapPromptObject.AddComponent<InputPrompt>();
        ReflectionHelper.SetPrivateFieldValue(nextMapPrompt, "forDevice", InputDeviceType.All);
        ReflectionHelper.SetPrivateFieldValue(nextMapPrompt, "showText", false);
        ReflectionHelper.SetPrivateFieldValue(nextMapPrompt, "action", "PowerBomb");
        ReflectionHelper.SetPrivateFieldValue(nextMapPrompt, "sortingLayer", "UI");
        ReflectionHelper.SetPrivateFieldValue(nextMapPrompt, "sortingOrderBase", 150);
        DeviceManager.ChangedSettings += nextMapPrompt.UpdateView;
        RectTransform nextMapPromptRect = nextMapPromptObject.AddComponent<RectTransform>();
        nextMapPromptRect.SetScale(38f);
        nextMapPromptObject.AddComponent<LayoutElement>();
        nextMapPromptObject.AddComponent<ForceSizeDeltaComponent>();

        // Reparent input prompts to the new layout object
        previousMapPromptObject.transform.SetParent(mapTitleLayoutObject.transform, false);
        previousMapPromptObject.transform.SetAsFirstSibling();
        nextMapPromptObject.transform.SetParent(mapTitleLayoutObject.transform, false);
        nextMapPromptObject.transform.SetAsLastSibling();

        _isArranged = true;

        return layoutObject;
    }

    private GameObject GetTitleObject(GameObject mapObject)
    {
        // The Title object is a direct child of the Map object, named "TitleShadow".
        foreach (Transform child in mapObject.transform)
        {
            if (child.name == "TitleShadow")
            {
                return child.gameObject;
            }
        }

        return null;
    }

    private GameObject GetProgressObject(GameObject titleObject)
    {
        // The Progress object is the first child of the TitleShadow's first child.
        Transform titleTransform = titleObject.transform.GetChild(0).transform;
        return titleTransform.GetChild(0).gameObject;
    }

    public bool IsTitleArranged()
    {
        return _isArranged;
    }
}
