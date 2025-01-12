using System.Collections.Generic;

namespace MinishootRandomizer;

public class GiveInitialItemsPatcher
{
    private readonly IRandomizerEngine _randomizerEngine;
    private readonly ILogger _logger = new NullLogger();

    private IPatchAction _patchAction = null;

    private List<MapRegion> _trackerMapRegions = new() {
        MapRegion.Abyss,
        MapRegion.Beach,
        MapRegion.Desert,
        MapRegion.Forest,
        MapRegion.Green,
        MapRegion.Junkyard,
        MapRegion.SunkenCity,
        MapRegion.Swamp
    };

    private List<Modules> _trackerModules = new() {
        Modules.Compass,
        Modules.CollectableScan
    };

    private List<NpcIds> _freedNpcs = new() {
        NpcIds.Explorer,
    };

    public GiveInitialItemsPatcher(IRandomizerEngine randomizerEngine, ILogger logger = null)
    {
        _randomizerEngine = randomizerEngine;
        _logger = logger ?? new NullLogger();
    }

    public void OnEnteringGameLocation(string locationName)
    {
        if (!_randomizerEngine.IsRandomized())
        {
            return;
        }

        if (_patchAction == null)
        {
            _patchAction = CreatePatchAction();
            _patchAction.Patch();
        }
    }

    public void OnExitingGame()
    {
        if (_patchAction != null)
        {
            _patchAction.Dispose();
            _patchAction = null;
        }
    }

    private IPatchAction CreatePatchAction()
    {
        CompositeAction compositeAction = new CompositeAction("GiveInitialItems");

        if (Plugin.IsDebug)
        {
            compositeAction.Add(new GiveDebugItemsAction());
        }
        else
        {
            // @TODO: reactivate when the AP server no longer adds the bullet number itself.
            // compositeAction.Add(new GiveStatsAction(Stats.BulletNumber, 1));
        }

        foreach (MapRegion mapRegion in _trackerMapRegions)
        {
            compositeAction.Add(new ScanMapAction(mapRegion));
        }

        foreach (Modules module in _trackerModules)
        {
            compositeAction.Add(new GiveModuleAction(module));
        }

        foreach (NpcIds npcId in _freedNpcs)
        {
            compositeAction.Add(new FreeNpcAction(npcId));
        }

        return new LoggableAction(compositeAction, _logger);
    }
}
