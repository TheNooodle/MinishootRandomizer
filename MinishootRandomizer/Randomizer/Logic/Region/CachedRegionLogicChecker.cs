using System.Collections.Generic;

namespace MinishootRandomizer;

public class CachedRegionLogicChecker : IRegionLogicChecker
{
    private readonly IRegionLogicChecker _innerLogicChecker;
    private readonly ICachePool<List<Region>> _cachePool;

    public CachedRegionLogicChecker(IRegionLogicChecker innerLogicChecker, ICachePool<List<Region>> cachePool)
    {
        _innerLogicChecker = innerLogicChecker;
        _cachePool = cachePool;
    }

    public bool CanReachRegion(Region region, LogicState state)
    {
        List<Region> regions = GetReachableRegions(state);

        return regions.Contains(region);
    }

    public List<Region> GetReachableRegions(LogicState state)
    {
        return _cachePool.Get("Regions", () =>
        {
            List<Region> regions = _innerLogicChecker.GetReachableRegions(state);
            return new CacheItem<List<Region>>(regions);
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
