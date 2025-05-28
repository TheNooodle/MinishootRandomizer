using System;
using TMPro;
using UnityEngine;

namespace MinishootRandomizer;

public class ResourcesTextGameObjectFactory : ITextGameObjectFactory
{
    private TMP_FontAsset _fontAsset = null;
    private Material _fontMaterial = null;

    public GameObject CreateTextGameObject(string text)
    {
        GameObject textObject = new GameObject("Text");
        textObject.AddComponent<RectTransform>();
        textObject.AddComponent<CanvasRenderer>();
        TextMeshProUGUI textComponent = textObject.AddComponent<TextMeshProUGUI>();
        textComponent.text = text;
        textComponent.font = GetFont();
        textComponent.fontMaterial = GetFontMaterial();
        textComponent.UpdateFontAsset();

        return textObject;
    }

    private TMP_FontAsset GetFont()
    {
        if (_fontAsset != null)
        {
            return _fontAsset;
        }

        TMP_FontAsset[] fonts = Resources.FindObjectsOfTypeAll<TMP_FontAsset>();
        foreach (TMP_FontAsset font in fonts)
        {
            if (font.name == "Bombard")
            {
                _fontAsset = font;
                return font;
            }
        }

        throw new Exception("Font not found!");
    }

    private Material GetFontMaterial()
    {
        if (_fontMaterial != null)
        {
            return _fontMaterial;
        }
        
        Material[] materials = Resources.FindObjectsOfTypeAll<Material>();
        foreach (Material material in materials)
        {
            if (material.name == "BOMBARD - OutlineLite")
            {
                _fontMaterial = material;
                return material;
            }
        }

        throw new Exception("Material not found!");
    }
}
