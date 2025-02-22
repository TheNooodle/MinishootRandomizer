using System.Collections.Generic;

namespace MinishootRandomizer;

public class SingleCacheStorage<T> : ICacheStorage<T>
{
    private CacheItem<T> _item = null;

    public CacheItem<T> Get(string key)
    {
        return _item;
    }

    public IEnumerable<KeyValuePair<string, CacheItem<T>>> GetAll()
    {
        return new List<KeyValuePair<string, CacheItem<T>>>
        {
            new KeyValuePair<string, CacheItem<T>>("key", _item)
        };
    }

    public void Set(string key, CacheItem<T> item)
    {
        _item = item;
    }

    public void Remove(string key)
    {
        Clear();
    }

    public void Clear()
    {
        _item = null;
    }
}
