namespace MinishootRandomizer;

public class CachedLogicChecker : ILogicChecker
{
    private readonly ILogicChecker _innerChecker;

    private LocationAccessibilitySet _cachedAccessibilitySet = null;
    private bool _isStale = true;

    public CachedLogicChecker(ILogicChecker innerChecker)
    {
        _innerChecker = innerChecker;
    }

    public LocationAccessibilitySet CheckAllLocationsLogic()
    {
        if (_cachedAccessibilitySet == null || _isStale)
        {
            _cachedAccessibilitySet = _innerChecker.CheckAllLocationsLogic();
            _isStale = false;
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
        _cachedAccessibilitySet = null;
        _isStale = true;
    }

    public void OnItemCollected(Item item)
    {
        _isStale = true;
    }
}
