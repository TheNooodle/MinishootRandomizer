using System;
using System.Collections.Generic;
using UnityEngine;

namespace MinishootRandomizer;

public class UnityObjectFinder : IObjectFinder
{
    public GameObject FindObject(ISelector selector)
    {
        if (selector is ByName byName)
        {
            GameObject[] gameObjects = HandleByName(byName);
            if (gameObjects.Length == 0)
            {
                throw new ObjectNotFoundException("Object not found: " + byName.Name);
            }
            return gameObjects[0];
        }
        if (selector is ByProximity byProximity)
        {
            GameObject[] gameObjects = HandleByProximity(byProximity);
            if (gameObjects.Length == 0)
            {
                throw new ObjectNotFoundException("Object not found: " + byProximity);
            }
            return gameObjects[0];
        }
        if (selector is ByComponent byComponent)
        {
            GameObject[] gameObjects = HandleByComponent(byComponent);
            if (gameObjects.Length == 0)
            {
                throw new ObjectNotFoundException("Object not found: " + byComponent.Type);
            }
            return gameObjects[0];
        }
        if (selector is ByNull)
        {
            return null; // Return null for ByNull selector
        }

        throw new InvalidSelectorException("Selector not supported: " + selector.GetType());
    }

    public GameObject[] FindObjects(ISelector selector)
    {
        if (selector is ByName byName)
        {
            return HandleByName(byName);
        }
        if (selector is ByProximity byProximity)
        {
            return HandleByProximity(byProximity);
        }
        if (selector is ByComponent byComponent)
        {
            return HandleByComponent(byComponent);
        }
        if (selector is ByNull)
        {
            return new GameObject[0]; // Return empty array for ByNull selector
        }

        throw new InvalidSelectorException("Selector not supported: " + selector.GetType());
    }

    private GameObject[] HandleByName(ByName selector)
    {
        if (!selector.IncludeInactive && selector.Type == null)
        {
            GameObject gameObject = GameObject.Find(selector.Name);
            if (gameObject != null)
            {
                return new GameObject[] { gameObject };
            }
            return new GameObject[0];
        }

        Type type = selector.Type ?? typeof(Transform);
        UnityEngine.Object[] unityObjects = GameObject.FindObjectsOfType(type, selector.IncludeInactive);
        if (unityObjects.Length == 0)
        {
            return new GameObject[0];
        }

        List<GameObject> gameObjects = new List<GameObject>();
        foreach (UnityEngine.Object unityObject in unityObjects)
        {
            if (!unityObject.GetType().IsSubclassOf(typeof(Component)))
            {
                continue;
            }
            Component component = (Component) unityObject;
            if (component.gameObject.name == selector.Name)
            {
                gameObjects.Add(component.gameObject);
            }
        }

        return gameObjects.ToArray();
    }

    private GameObject[] HandleByProximity(ByProximity selector)
    {
        UnityEngine.Object[] unityObjects = GameObject.FindObjectsOfType(selector.Type, selector.IncludeInactive);
        if (unityObjects.Length == 0)
        {
            return new GameObject[0];
        }

        List<GameObject> gameObjects = new List<GameObject>();
        foreach (UnityEngine.Object unityObject in unityObjects)
        {
            if (!unityObject.GetType().IsSubclassOf(typeof(Component)))
            {
                continue;
            }
            Component component = (Component) unityObject;
            if (Vector3.Distance(component.transform.position, selector.Position) <= selector.Radius)
            {
                gameObjects.Add(component.gameObject);
            }
        }

        return gameObjects.ToArray();
    }

    private GameObject[] HandleByComponent(ByComponent selector)
    {
        UnityEngine.Object[] unityObjects = GameObject.FindObjectsOfType(selector.Type, selector.IncludeInactive);
        if (unityObjects.Length == 0)
        {
            return new GameObject[0];
        }

        List<GameObject> gameObjects = new List<GameObject>();
        foreach (UnityEngine.Object unityObject in unityObjects)
        {
            if (!unityObject.GetType().IsSubclassOf(typeof(Component)))
            {
                continue;
            }
            Component component = (Component) unityObject;
            gameObjects.Add(component.gameObject);
        }

        return gameObjects.ToArray();
    }
}
