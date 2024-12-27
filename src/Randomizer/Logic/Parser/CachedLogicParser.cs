using System;
using System.Collections.Generic;

namespace MinishootRandomizer;

public class CachedLogicParser : ILogicParser
{
    private readonly ILogicParser _innerParser;
    private readonly ICachePool<LogicParsingResult> _cachePool;

    public CachedLogicParser(ILogicParser innerParser, ICachePool<LogicParsingResult> cachePool)
    {
        _innerParser = innerParser;
        _cachePool = cachePool;
    }

    public LogicParsingResult ParseLogic(string logicRule, LogicState state, List<ISetting> settings)
    {
        CacheItem<LogicParsingResult> result = _cachePool.Get(logicRule);
        if (result != null)
        {
            return result.Value;
        }

        LogicParsingResult parsed = _innerParser.ParseLogic(logicRule, state, settings);
        CacheItem<LogicParsingResult> cacheItem = new CacheItem<LogicParsingResult>(
            logicRule,
            parsed,
            new TimeSpan(1, 0, 0),
            !parsed.Result ? parsed.UsedItemNames : new List<string>() // Only tag failed results
        );

        _cachePool.Set(cacheItem);

        return parsed;
    }

    public void OnExitingGame()
    {
        _cachePool.Clear();
    }

    public void OnItemCollected(Item item)
    {
        _cachePool.PurgeTags(new List<string> { item.Identifier, LogicParsingResult.AnyItemName });
    }
}
