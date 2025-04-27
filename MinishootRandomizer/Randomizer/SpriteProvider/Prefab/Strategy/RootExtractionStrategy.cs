using UnityEngine;

namespace MinishootRandomizer;

public class RootExtractionStrategy : IPrefabSpriteExtractionStrategy
{
    public Sprite ExtractSprite(string prefabIdentifier, GameObject prefab)
    {
        SpriteRenderer spriteRenderer = prefab.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            return spriteRenderer.sprite;
        }
        else
        {
            throw new CannotExtractSpriteException();
        }
    }
}
