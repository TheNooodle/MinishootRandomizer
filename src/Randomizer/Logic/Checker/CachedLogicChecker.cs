namespace MinishootRandomizer;

public class CachedLogicChecker : ILogicChecker
{
    private readonly ILogicChecker _innerChecker;
    private readonly IMessageDispatcher _messageDispatcher;

    private LocationAccessibilitySet _cachedAccessibilitySet = new LocationAccessibilitySet();

    // isRefreshInProgress means that the cache is being refreshed right now.
    private bool _isRefreshInProgress = false;
    // isStale means that the cache is outdated and, while we still serve the cached data, a refresh 
    // task is being scheduled to update the cache.
    private bool _isStale = false;
    // isInitialized means that the cache has been initialized at least once.
    private bool _isInitialized = false;

    public CachedLogicChecker(ILogicChecker innerChecker, IMessageDispatcher messageDispatcher)
    {
        _innerChecker = innerChecker;
        _messageDispatcher = messageDispatcher;
    }

    public LocationAccessibilitySet CheckAllLocationsLogic()
    {
        if (!_isInitialized)
        {
            _isInitialized = true;
            InvalidateCache();
        }

        return _cachedAccessibilitySet;
    }

    public LogicAccessibility CheckLocationLogic(Location location)
    {
        LocationAccessibilitySet accessibilitySet = CheckAllLocationsLogic();

        if (accessibilitySet.IsInLogic(location))
        {
            return LogicAccessibility.InLogic;
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

    private void InvalidateCache()
    {
        // If the cache is already stale, we don't need to do anything, as a refresh task is already scheduled.
        if (!_isStale)
        {
            _isStale = true;
            _messageDispatcher.Dispatch(new RefreshLogicCheckerCacheMessage(this));
        }
    }

    public void RefreshCache()
    {
        if (_isRefreshInProgress)
        {
            return;
        }

        try
        {
            _isRefreshInProgress = true;
            _cachedAccessibilitySet = _innerChecker.CheckAllLocationsLogic();
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
