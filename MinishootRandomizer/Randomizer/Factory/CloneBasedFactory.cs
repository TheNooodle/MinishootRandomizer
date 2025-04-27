using System.Collections.Generic;
using UnityEngine;

namespace MinishootRandomizer;

public class CloneBasedFactory : IGameObjectFactory, IPrefabCollector
{
    private readonly IObjectFinder _objectFinder;
    private readonly ICloningPassChain _cloningPassChain;
    private readonly ILogger _logger = new NullLogger();

    private Dictionary<string, GameObject> _prefabs = new Dictionary<string, GameObject>();

    public CloneBasedFactory(IObjectFinder objectFinder, ICloningPassChain cloningPassChain, ILogger logger)
    {
        _objectFinder = objectFinder;
        _cloningPassChain = cloningPassChain;
        _logger = logger ?? new NullLogger();
    }

    public void AddPrefab(string prefabIdentifier, ISelector selector, CloningType cloningType = CloningType.Recreate)
    {
        if (_prefabs.ContainsKey(prefabIdentifier))
        {
            return;
        }

        try 
        {
            GameObject existingObject = _objectFinder.FindObject(selector);
            if (existingObject.GetComponent<Npc>() != null)
            {
                _logger.LogInfo($"Prefab {prefabIdentifier} is an NPC. Skipping cloning.");
                return;
            }
            GameObject prefab;
            switch (cloningType)
            {
                case CloningType.Recreate:
                    prefab = new GameObject();
                    break;
                case CloningType.Copy:
                default:
                    prefab = GameObject.Instantiate(existingObject);
                    break;
            }
            _cloningPassChain.ApplyPasses(existingObject, prefab, cloningType);
            prefab.transform.position = new Vector3(-3000, -3000, -3000); // Hide the prefab.
            UnityEngine.Object.DontDestroyOnLoad(prefab);
            _prefabs[prefabIdentifier] = prefab;
        }
        catch (ObjectNotFoundException)
        {
            throw new InvalidPrefabException($"Selector {selector} not found for {prefabIdentifier}");
        }
    }

    public GameObject CreateGameObjectForItem(Item item)
    {
        if (_prefabs.ContainsKey(item.Identifier))
        {
            GameObject clonedObject = GameObject.Instantiate(_prefabs[item.Identifier]);
            RandomizerPickup pickup = clonedObject.GetComponent<RandomizerPickup>();
            if (pickup != null)
            {
                pickup.Item = item;
                pickup.IsPrefab = false;
            }

            return clonedObject;
        }
        else if (item is SmallKeyItem && _prefabs.ContainsKey("Small Key"))
        {
            GameObject clonedObject = GameObject.Instantiate(_prefabs["Small Key"]);
            RandomizerPickup pickup = clonedObject.GetComponent<RandomizerPickup>();
            if (pickup != null)
            {
                pickup.Item = item;
                pickup.IsPrefab = false;
            }

            return clonedObject;
        }
        else if (item is BossKeyItem && _prefabs.ContainsKey("Boss Key"))
        {
            GameObject clonedObject = GameObject.Instantiate(_prefabs["Boss Key"]);
            RandomizerPickup pickup = clonedObject.GetComponent<RandomizerPickup>();
            if (pickup != null)
            {
                pickup.Item = item;
                pickup.IsPrefab = false;
            }

            return clonedObject;
        }
        else
        {
            if (_prefabs.ContainsKey("HP Crystal Shard"))
            {
                // temporary
                GameObject clonedObject = GameObject.Instantiate(_prefabs["HP Crystal Shard"]);
                RandomizerPickup pickup = clonedObject.GetComponent<RandomizerPickup>();
                if (pickup != null)
                {
                    pickup.Item = item;
                    pickup.IsPrefab = false;
                }

                return clonedObject;
            }
            throw new NoCorrespondingPrefabException($"No prefab found for {item.Identifier}");
        }
    }

    public GameObject CreateGameObject(string prefabIdentifier)
    {
        if (_prefabs.ContainsKey(prefabIdentifier))
        {
            GameObject clonedObject = GameObject.Instantiate(_prefabs[prefabIdentifier]);

            return clonedObject;
        }

        throw new NoCorrespondingPrefabException($"No prefab found for {prefabIdentifier}");
    }
}

public class NoCorrespondingPrefabException : System.Exception
{
    public NoCorrespondingPrefabException(string message) : base(message) { }
}

public class InvalidPrefabException : System.Exception
{
    public InvalidPrefabException(string message) : base(message) { }
}
