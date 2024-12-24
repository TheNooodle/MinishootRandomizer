using System.Collections.Generic;

namespace MinishootRandomizer;

public class LogicState
{
    private List<Item> _items = new();
    private List<Region> _reachableRegions = new();

    public void AddItem(Item item)
    {
        _items.Add(item);
    }

    public bool HasItem(Item item, int count = 1)
    {
        if (count == 1)
        {
            return _items.Contains(item);
        }
        else
        {
            return _items.FindAll(i => i == item).Count >= count;
        }
    }

    public void AddReachableRegion(Region region)
    {
        _reachableRegions.Add(region);
    }

    public bool CanReach(Region region)
    {
        return _reachableRegions.Contains(region);
    }
}
