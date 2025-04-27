using System.Collections.Generic;

namespace MinishootRandomizer;

public class ItemReplacementPatcher
{
    private readonly IRandomizerEngine _randomizerEngine;
    private readonly ILocationVisitor _locationVisitor;
    private readonly IZoneRepository _zoneRepository;
    private readonly IRegionRepository _regionRepository;
    private readonly ILocationRepository _locationRepository;
    private readonly ILogger _logger;

    private readonly Dictionary<string, IPatchAction> _patchActions = new Dictionary<string, IPatchAction>();

    public string CurrentGameLocation { get; set; }

    public ItemReplacementPatcher(
        IRandomizerEngine randomizerEngine,
        ILocationVisitor locationVisitor,
        IZoneRepository zoneRepository,
        IRegionRepository regionRepository,
        ILocationRepository locationRepository,
        ILogger logger = null
    ) {
        _randomizerEngine = randomizerEngine;
        _locationVisitor = locationVisitor;
        _zoneRepository = zoneRepository;
        _regionRepository = regionRepository;
        _locationRepository = locationRepository;
        _logger = logger ?? new NullLogger();
    }

    public void OnEnteringGameLocation(string gameLocationName)
    {
        Unpatch();
        CurrentGameLocation = gameLocationName;
        Patch();
    }

    public void OnExitingGame()
    {
        foreach (IPatchAction patchAction in _patchActions.Values)
        {
            patchAction.Dispose();
        }
        _patchActions.Clear();
    }

    private void Patch()
    {
        if (!_randomizerEngine.IsRandomized())
        {
            return;
        }

        _logger.LogInfo("ItemReplacementPatcher is patching!");
        List<Location> randomizedLocations = _randomizerEngine.GetRandomizedLocations();
        List<Zone> zones = _zoneRepository.GetByGameLocationName(CurrentGameLocation);
        foreach (Zone zone in zones)
        {
            foreach (string regionName in zone.GetRegionNames())
            {
                Region region = _regionRepository.Get(regionName);
                foreach (string locationName in region.GetLocationNames())
                {
                    try {
                        Location location = _locationRepository.Get(locationName);
                        if (randomizedLocations.Contains(location))
                        {
                            if (!_patchActions.ContainsKey(location.Identifier)) {
                                Item item = _randomizerEngine.PeekLocation(location);
                                IPatchAction patchAction = location.Accept(_locationVisitor, item);
                                _patchActions.Add(location.Identifier, patchAction);
                                patchAction.Patch();
                            } else {
                                IPatchAction patchAction = _patchActions[location.Identifier];
                                patchAction.Patch();
                            }
                        }
                    } catch (LocationNotFoundException e) {
                        _logger.LogWarning(e.Message);
                    }
                }
            }
        }

        _logger.LogInfo("ItemReplacementPatcher is done patching!");
    }

    private void Unpatch()
    {
        _logger.LogInfo("ItemReplacementPatcher is unpatching!");
        foreach (IPatchAction patchAction in _patchActions.Values)
        {
            patchAction.Unpatch();
        }
    }
}
