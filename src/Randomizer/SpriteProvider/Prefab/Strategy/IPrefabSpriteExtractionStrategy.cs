using UnityEngine;

namespace MinishootRandomizer;

public interface IPrefabSpriteExtractionStrategy
{
    Sprite ExtractSprite(string prefabIdentifier, GameObject prefab);
}
