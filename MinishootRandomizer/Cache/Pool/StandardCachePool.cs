using System;
using System.Collections.Generic;

namespace MinishootRandomizer;

public class StandardCachePool<T> : ICachePool<T>
{
    private readonly ILogger _logger;
    private readonly ICacheStorage<T> _cacheStorage;

    public StandardCachePool(ICacheStorage<T> cacheStorage, ILogger logger = null)
    {
        _cacheStorage = cacheStorage;
        _logger = logger ?? new NullLogger();
    }

    public T Get(string key, Func<CacheItem<T>> factory)
    {
        CacheItem<T> item = _cacheStorage.Get(key);
        if (item != null)
        {
            if (item.IsExpired())
            {
                _logger.LogDebug($"Cache item with key {key} has expired.");
                _cacheStorage.Remove(key);
            }
            else
            {
                _logger.LogDebug($"Cache hit for key {key}.");
                return item.Value;
            }
        }

        _logger.LogDebug($"Cache miss for key {key}.");
        CacheItem<T> newItem = factory();
        _cacheStorage.Set(key, newItem);
        return newItem.Value;
    }

    public void Clear()
    {
        _logger.LogDebug("Clearing cache.");
        _cacheStorage.Clear();
    }

    public void PurgeKey(string key)
    {
        _logger.LogDebug($"Purging cache item with key {key}.");
        _cacheStorage.Remove(key);
    }

    public void PurgeTags(List<string> tags)
    {
        _logger.LogDebug($"Purging cache items with tags {string.Join(", ", tags)}.");
        List<string> keysToRemove = new List<string>();
        foreach (var pair in _cacheStorage.GetAll())
        {
            CacheItem<T> item = pair.Value;
            foreach (string cacheItemTag in item.Tags)
            {
                if (tags.Contains(cacheItemTag))
                {
                    keysToRemove.Add(pair.Key);
                    break;
                }
            }
        }

        foreach (string key in keysToRemove)
        {
            _logger.LogDebug($"Purging cache item with key {key}.");
            _cacheStorage.Remove(key);
        }
    }
}
