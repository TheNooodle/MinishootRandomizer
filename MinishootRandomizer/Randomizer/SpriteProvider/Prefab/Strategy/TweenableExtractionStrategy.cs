using UnityEngine;

namespace MinishootRandomizer;

public class TweenableExtractionStrategy : IPrefabSpriteExtractionStrategy
{
    public Sprite ExtractSprite(string prefabIdentifier, GameObject prefab)
    {
        Transform tweenableTransform = prefab.transform.Find("Tweenable");
        if (tweenableTransform == null)
        {
            throw new CannotExtractSpriteException();
        }

        Transform spriteChildTransform = tweenableTransform.Find("Sprite");
        if (spriteChildTransform == null)
        {
            throw new CannotExtractSpriteException();
        }

        SpriteRenderer spriteRenderer = spriteChildTransform.GetComponent<SpriteRenderer>();

        return spriteRenderer.sprite;
    }
}
