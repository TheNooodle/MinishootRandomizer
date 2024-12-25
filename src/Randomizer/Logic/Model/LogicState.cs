using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MinishootRandomizer;

public class LogicState
{
    private Dictionary<string, int> _itemCount = new();
    private List<Region> _reachableRegions = new();

    public void AddItemCount(Item item, int count = 1)
    {
        if (_itemCount.TryGetValue(item.Identifier, out int owned))
        {
            _itemCount[item.Identifier] = owned + count;
        }
        else
        {
            _itemCount.Add(item.Identifier, count);
        }
    }

    public void SetItemCount(Item item, int count = 1)
    {
        _itemCount.Add(item.Identifier, count);
    }

    public bool HasItem(Item item, int count = 1)
    {
        return _itemCount.TryGetValue(item.Identifier, out int owned) && owned >= count;
    }

    public void AddReachableRegion(Region region)
    {
        if (!_reachableRegions.Contains(region))
        {
            _reachableRegions.Add(region);
        }
    }

    public bool CanReach(Region region)
    {
        return _reachableRegions.Contains(region);
    }

    public ReadOnlyCollection<Region> GetReachableRegions()
    {
        return _reachableRegions.AsReadOnly();
    }
}
