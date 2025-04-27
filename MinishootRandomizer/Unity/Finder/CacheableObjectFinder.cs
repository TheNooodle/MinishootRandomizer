using System.Collections.Generic;
using UnityEngine;

namespace MinishootRandomizer;

// @TODO: implement cache invalidation
public class CacheableObjectFinder : IObjectFinder
{
    private IObjectFinder _innerFinder;
    private Dictionary<ISelector, GameObject> _cache = new Dictionary<ISelector, GameObject>();
    private Dictionary<ISelector, GameObject[]> _cacheArray = new Dictionary<ISelector, GameObject[]>();

    public CacheableObjectFinder(IObjectFinder innerFinder)
    {
        _innerFinder = innerFinder;
    }

    public GameObject FindObject(ISelector selector)
    {
        if (_cache.ContainsKey(selector))
        {
            return _cache[selector];
        }

        GameObject gameObject = _innerFinder.FindObject(selector);
        _cache.Add(selector, gameObject);
        return gameObject;
    }

    public GameObject[] FindObjects(ISelector selector)
    {
        if (_cacheArray.ContainsKey(selector))
        {
            return _cacheArray[selector];
        }

        GameObject[] gameObjects = _innerFinder.FindObjects(selector);
        _cacheArray.Add(selector, gameObjects);
        return gameObjects;
    }
}
