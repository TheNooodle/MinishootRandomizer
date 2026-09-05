using System.Collections.Generic;

namespace MinishootRandomizer;

public class DictionnaryProgressionStorage : IProgressionStorage
{
    private Dictionary<Location, bool> _progression = new();
    private int _locationCheckedCount = 0;

    public void SetLocationChecked(Location location, bool isChecked = true)
    {
        _progression[location] = isChecked;
        if (isChecked)
        {
            _locationCheckedCount++;
        }
    }

    public bool IsLocationChecked(Location location)
    {
        return _progression.TryGetValue(location, out var isChecked) && isChecked;
    }

    public bool IsGoalCompleted(Goals goal)
    {
        return false;
    }

    public int GetLocationCheckedCount()
    {
        return _locationCheckedCount;
    }
}
