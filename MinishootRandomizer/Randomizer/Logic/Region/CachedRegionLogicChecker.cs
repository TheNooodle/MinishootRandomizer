using System.Collections.Generic;

namespace MinishootRandomizer;

public class CachedRegionLogicChecker : IRegionLogicChecker
{
    private readonly IRegionLogicChecker _innerLogicChecker;
    private readonly ICachePool<Dictionary<Region, LogicAccessibility>> _cachePool;

    public CachedRegionLogicChecker(IRegionLogicChecker innerLogicChecker, ICachePool<Dictionary<Region, LogicAccessibility>> cachePool)
    {
        _innerLogicChecker = innerLogicChecker;
        _cachePool = cachePool;
    }

    public LogicAccessibility GetRegionAccessibility(Region region, LogicState state)
    {
        Dictionary<Region, LogicAccessibility> regions = GetRegionsAccessibility(state);

        return regions.TryGetValue(region, out LogicAccessibility accessibility)
            ? accessibility
            : LogicAccessibility.OutOfLogic;
    }

    public Dictionary<Region, LogicAccessibility> GetRegionsAccessibility(LogicState state)
    {
        return _cachePool.Get("Regions", () =>
        {
            Dictionary<Region, LogicAccessibility> regions = _innerLogicChecker.GetRegionsAccessibility(state);
            return new CacheItem<Dictionary<Region, LogicAccessibility>>(regions);
        });
    }

    public void OnExitingGame()
    {
        _cachePool.Clear();
    }

    public void OnItemCollected(Item item)
    {
        _cachePool.Clear();
    }

    public void OnNpcFreed()
    {
        _cachePool.Clear();
    }

    public void OnEnteringGameLocation(string locationName)
    {
        _cachePool.Clear();
    }

    public void OnPlayerCurrencyChanged(Currency currency)
    {
        if (currency == Currency.Scarab)
        {
            _cachePool.Clear();
        }
    }

    public void OnGoalCompleted(Goals goal)
    {
        _cachePool.Clear();
    }
}
