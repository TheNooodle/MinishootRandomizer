using System;
using System.Collections.Generic;

namespace MinishootRandomizer;

public class CacheItem<T>
{
    private readonly T _value;
    private readonly bool _hasExpiration;
    private readonly DateTime _expiration;
    private readonly List<string> _tags;

    public T Value => _value;
    public bool HasExpiration => _hasExpiration;
    public DateTime Expiration => _expiration;
    public List<string> Tags => _tags;

    public CacheItem(T value, List<string> tags = null)
    {
        _value = value;
        _hasExpiration = false;
        _expiration = DateTime.MinValue;
        _tags = tags ?? new List<string>();
    }

    public CacheItem(T value, DateTime expiration, List<string> tags = null)
    {
        _value = value;
        _hasExpiration = true;
        _expiration = expiration;
        _tags = tags ?? new List<string>();
    }

    public CacheItem(T value, TimeSpan expiration, List<string> tags = null)
    {
        _value = value;
        _hasExpiration = true;
        _expiration = DateTime.Now.Add(expiration);
        _tags = tags ?? new List<string>();
    }

    public bool IsExpired()
    {
        if (!_hasExpiration)
        {
            return false;
        }

        return DateTime.Now > _expiration;
    }
}
