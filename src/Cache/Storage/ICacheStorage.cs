using System.Collections.Generic;

namespace MinishootRandomizer;

public interface ICacheStorage<T>
{
    CacheItem<T> Get(string key);
    IEnumerable<KeyValuePair<string, CacheItem<T>>> GetAll();
    void Set(string key, CacheItem<T> item);
    void Remove(string key);
    void Clear();
}
