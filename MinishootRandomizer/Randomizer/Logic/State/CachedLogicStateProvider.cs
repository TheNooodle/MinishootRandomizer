namespace MinishootRandomizer;

public class CachedLogicStateProvider : ILogicStateProvider
{
    private readonly ILogicStateProvider _innerProvider;
    private ICachePool<LogicState> _cachePool;

    public CachedLogicStateProvider(ILogicStateProvider innerProvider, ICachePool<LogicState> cachePool)
    {
        _innerProvider = innerProvider;
        _cachePool = cachePool;
    }

    public LogicState GetLogicState()
    {
        return _cachePool.Get("LogicState", () => {
            LogicState logicState = _innerProvider.GetLogicState();
            return new CacheItem<LogicState>(logicState);
        });
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

    public void OnExitingGame()
    {
        _cachePool.Clear();
    }
}
