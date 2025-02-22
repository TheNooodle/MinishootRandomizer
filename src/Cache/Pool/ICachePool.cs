using System;
using System.Collections.Generic;

namespace MinishootRandomizer;

public interface ICachePool<T>
{
    T Get(string key, Func<CacheItem<T>> factory);
    void Clear();
    void PurgeKey(string key);
    void PurgeTags(List<string> tags);
}
