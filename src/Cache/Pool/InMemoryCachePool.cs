using System.Collections.Generic;

namespace MinishootRandomizer;

public class InMemoryCachePool<T> : ICachePool<T>
{
    private readonly ILogger _logger;

    private readonly Dictionary<string, CacheItem<T>> _cache = new Dictionary<string, CacheItem<T>>();

    public InMemoryCachePool(ILogger logger = null)
    {
        _logger = logger ?? new NullLogger();
    }

    public void Clear()
    {
        _logger.LogDebug("Clearing cache.");
        _cache.Clear();
    }

    public CacheItem<T> Get(string key)
    {
        if (_cache.TryGetValue(key, out CacheItem<T> item))
        {
            if (item.IsExpired())
            {
                _logger.LogDebug($"Cache item with key {key} has expired.");
                _cache.Remove(key);
                return null;
            }

            _logger.LogDebug($"Cache hit for key {key}.");
            return item;
        }

        _logger.LogDebug($"Cache miss for key {key}.");
        return null;
    }

    public void PurgeKey(string key)
    {
        _logger.LogDebug($"Purging cache item with key {key}.");
        _cache.Remove(key);
    }

    public void PurgeTags(List<string> tags)
    {
        _logger.LogDebug($"Purging cache items with tags {string.Join(", ", tags)}.");
        List<string> keysToRemove = new List<string>();
        foreach (KeyValuePair<string, CacheItem<T>> pair in _cache)
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
            _cache.Remove(key);
        }
    }

    public void Set(CacheItem<T> item)
    {
        _logger.LogDebug($"Setting cache item with key {item.Key}.");
        _cache[item.Key] = item;
    }
}
