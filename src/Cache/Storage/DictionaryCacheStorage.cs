using System.Collections.Generic;

namespace MinishootRandomizer;

public class DictionaryCacheStorage<T> : ICacheStorage<T>
{
    private readonly Dictionary<string, CacheItem<T>> _cache = new();

    public CacheItem<T> Get(string key)
    {
        if (_cache.TryGetValue(key, out var item))
        {
            return item;
        }

        return null;
    }

    public IEnumerable<KeyValuePair<string, CacheItem<T>>> GetAll()
    {
        return _cache;
    }

    public void Set(string key, CacheItem<T> item)
    {
        _cache[key] = item;
    }

    public void Remove(string key)
    {
        _cache.Remove(key);
    }

    public void Clear()
    {
        _cache.Clear();
    }
}
