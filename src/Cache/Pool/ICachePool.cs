using System.Collections.Generic;

namespace MinishootRandomizer;

public interface ICachePool<T>
{
    public void Set(CacheItem<T> item);
    public CacheItem<T> Get(string key);
    public void Clear();
    public void PurgeKey(string key);
    public void PurgeTags(List<string> tags);
}
