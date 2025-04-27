namespace MinishootRandomizer;

public class ScanMapAction : IPatchAction
{
    private readonly MapRegion _mapRegion;

    public ScanMapAction(MapRegion mapRegion)
    {
        _mapRegion = mapRegion;
    }

    public void Dispose()
    {
        // no-op
    }

    public void Patch()
    {
        PlayerState.SetMap(_mapRegion, MapRegionState.Scanned);
    }

    public void Unpatch()
    {
        PlayerState.SetMap(_mapRegion, MapRegionState.Locked);
    }
}
