using System.Collections.Generic;

namespace MinishootRandomizer;

public class DictionnaryProgressionStorage : IProgressionStorage
{
    private Dictionary<Location, bool> _progression = new();

    public void SetLocationChecked(Location location, bool isChecked = true)
    {
        _progression[location] = isChecked;
    }

    public bool IsLocationChecked(Location location)
    {
        return _progression.TryGetValue(location, out var isChecked) && isChecked;
    }

    public bool IsGoalCompleted(Goals goal)
    {
        throw new System.NotImplementedException();
    }
}
