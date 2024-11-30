using UnityEngine;

namespace MinishootRandomizer;

public class SpriteChildExtractionStrategy : IPrefabSpriteExtractionStrategy
{
    public Sprite ExtractSprite(string prefabIdentifier, GameObject prefab)
    {
        Transform spriteTransform = prefab.transform.Find("Sprite");
        if (spriteTransform == null)
        {
            throw new CannotExtractSpriteException();
        }

        SpriteRenderer spriteRenderer = spriteTransform.GetComponent<SpriteRenderer>();

        return spriteRenderer.sprite;
    }
}
