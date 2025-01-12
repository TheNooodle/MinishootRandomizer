namespace MinishootRandomizer;

public class CachedLogicChecker : ILogicChecker
{
    private readonly ILogicChecker _innerChecker;
    private readonly IMessageDispatcher _messageDispatcher;

    private LocationAccessibilitySet _cachedAccessibilitySet = new LocationAccessibilitySet();

    private bool _isRefreshInProgress = false;
    private bool _isStale = true;

    public CachedLogicChecker(ILogicChecker innerChecker, IMessageDispatcher messageDispatcher)
    {
        _innerChecker = innerChecker;
        _messageDispatcher = messageDispatcher;
    }

    public LocationAccessibilitySet CheckAllLocationsLogic()
    {
        if (_isStale && !_isRefreshInProgress)
        {
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
        _isStale = true;
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

    private void InvalidateCache()
    {
        _isStale = true;
        _messageDispatcher.Dispatch(new RefreshLogicCheckerCacheMessage(this));
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
            throw;
        }
    }
}
