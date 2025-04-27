using UnityEngine;

namespace MinishootRandomizer;

public interface IGameObjectFactory
{
    GameObject CreateGameObjectForItem(Item item);
    GameObject CreateGameObject(string prefabIdentifier);
}
