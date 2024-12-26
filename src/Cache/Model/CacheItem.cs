using System;
using System.Collections.Generic;

namespace MinishootRandomizer;

public class CacheItem<T>
{
    private readonly string _key;
    private readonly T _value;
    private readonly DateTime _expiration;
    private readonly List<string> _tags;

    public string Key => _key;
    public T Value => _value;
    public DateTime Expiration => _expiration;
    public List<string> Tags => _tags;

    public CacheItem(string key, T value, DateTime expiration, List<string> tags = null)
    {
        _key = key;
        _value = value;
        _expiration = expiration;
        _tags = tags ?? new List<string>();
    }

    public CacheItem(string key, T value, TimeSpan expiration, List<string> tags = null)
    {
        _key = key;
        _value = value;
        _expiration = DateTime.Now.Add(expiration);
        _tags = tags ?? new List<string>();
    }

    public bool IsExpired()
    {
        return DateTime.Now > _expiration;
    }
}
