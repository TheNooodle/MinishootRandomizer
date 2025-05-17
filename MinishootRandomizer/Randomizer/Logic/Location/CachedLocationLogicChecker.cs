namespace MinishootRandomizer;

public class CachedLocationLogicChecker : ILocationLogicChecker
{
    private readonly ILocationLogicChecker _innerChecker;
    private readonly ILogicStateProvider _logicStateProvider; // used when refreshing the cache with events.
    private readonly IMessageDispatcher _messageDispatcher;

    private LocationAccessibilitySet _cachedAccessibilitySet = new LocationAccessibilitySet();

    // isRefreshInProgress means that the cache is being refreshed right now.
    private bool _isRefreshInProgress = false;
    // isStale means that the cache is outdated and, while we still serve the cached data, a refresh 
    // task is being scheduled to update the cache.
    private bool _isStale = false;
    // isInitialized means that the cache has been initialized at least once.
    private bool _isInitialized = false;

    public CachedLocationLogicChecker(ILocationLogicChecker innerChecker, IMessageDispatcher messageDispatcher)
    {
        _innerChecker = innerChecker;
        _messageDispatcher = messageDispatcher;
    }

    public LocationAccessibilitySet CheckAllLocationsLogic(LogicState logicState)
    {
        if (!_isInitialized)
        {
            _isInitialized = true;
            InvalidateCache(logicState);
        }

        return _cachedAccessibilitySet;
    }

    public LogicAccessibility CheckLocationLogic(LogicState logicState, Location location)
    {
        LocationAccessibilitySet accessibilitySet = CheckAllLocationsLogic(logicState);

        if (accessibilitySet.IsInLogic(location))
        {
            return LogicAccessibility.InLogic;
        }
        else if (accessibilitySet.IsOutOfLogic(location))
        {
            return LogicAccessibility.OutOfLogic;
        }

        return LogicAccessibility.Inaccessible;
    }

    public void OnExitingGame()
    {
        _cachedAccessibilitySet = new LocationAccessibilitySet();
        _isRefreshInProgress = false;
        _isStale = false;
        _isInitialized = false;
    }

    public void OnItemCollected(Item item)
    {
        InvalidateCache();
    }

    public void OnNpcFreed()
    {
        InvalidateCache();
    }

    public void OnEnteringGameLocation(string locationName)
    {
        InvalidateCache();
    }

    public void OnPlayerCurrencyChanged(Currency currency)
    {
        if (currency == Currency.Scarab)
        {
            InvalidateCache();
        }
    }

    public void OnGoalCompleted(Goals goal)
    {
        InvalidateCache();
    }

    private void InvalidateCache(LogicState logicState = null)
    {
        // If the cache is already stale, we don't need to do anything, as a refresh task is already scheduled.
        if (!_isStale)
        {
            _isStale = true;
            _messageDispatcher.Dispatch(new RefreshLogicCheckerCacheMessage(this, logicState));
        }
    }

    public void RefreshCache(LogicState logicState = null)
    {
        if (_isRefreshInProgress)
        {
            return;
        }

        if (logicState == null)
        {
            // If the logic state is not provided, we need to get it from the provider.
            // This is used when the cache is being refreshed with events.
            logicState = _logicStateProvider.GetLogicState();
        }

        try
        {
            _isRefreshInProgress = true;
            _cachedAccessibilitySet = _innerChecker.CheckAllLocationsLogic(logicState);
            _isStale = false;
            _isRefreshInProgress = false;
        }
        catch
        {
            _isRefreshInProgress = false;
            _isStale = false;
            throw;
        }
    }
}
