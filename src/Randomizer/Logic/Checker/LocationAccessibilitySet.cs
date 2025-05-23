using System.Collections.Generic;

namespace MinishootRandomizer;

public class LocationAccessibilitySet
{
    private List<Location> _inLogicLocations = new();
    private List<Location> _outOfLogicLocations = new();
    private List<Location> _inaccessibleLocations = new();

    public void AddInLogicLocation(Location location)
    {
        if (!_inLogicLocations.Contains(location))
        {
            _inLogicLocations.Add(location);
        }
    }

    public bool IsInLogic(Location location)
    {
        return _inLogicLocations.Contains(location);
    }

    public IReadOnlyCollection<Location> GetInLogicLocations()
    {
        return _inLogicLocations.AsReadOnly();
    }

    public void AddInaccessibleLocation(Location location)
    {
        if (!_inaccessibleLocations.Contains(location))
        {
            _inaccessibleLocations.Add(location);
        }
    }

    public bool IsInaccessible(Location location)
    {
        return _inaccessibleLocations.Contains(location);
    }

    public IReadOnlyCollection<Location> GetInaccessibleLocations()
    {
        return _inaccessibleLocations.AsReadOnly();
    }

    public void AddOutOfLogicLocation(Location location)
    {
        if (!_outOfLogicLocations.Contains(location))
        {
            _outOfLogicLocations.Add(location);
        }
    }

    public bool IsOutOfLogic(Location location)
    {
        return _outOfLogicLocations.Contains(location);
    }

    public IReadOnlyCollection<Location> GetOutOfLogicLocations()
    {
        return _outOfLogicLocations.AsReadOnly();
    }
}
