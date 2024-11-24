namespace MinishootRandomizer;

public class MapItem : Item
{
    private MapRegion _region;

    public MapItem(string identifier, ItemCategory category, MapRegion region) : base(identifier, category)
    {
        _region = region;
    }

    public override void Collect()
    {
        PlayerState.SetMap(_region, MapRegionState.Unlocked);
    }
}
