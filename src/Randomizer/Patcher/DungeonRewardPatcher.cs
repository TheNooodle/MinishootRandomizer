using System.Collections.Generic;
using UnityEngine;

namespace MinishootRandomizer;

public class DungeonRewardPatcher
{
    private class DungeonRewardPatcherData
    {
        public string LocationIdentifier { get; }
        public ISelector Selector { get; }

        public DungeonRewardPatcherData(string locationIdentifier, ISelector selector)
        {
            LocationIdentifier = locationIdentifier;
            Selector = selector;
        }
    }

    private readonly IRandomizerEngine _randomizerEngine;
    private readonly IObjectFinder _objectFinder;
    private readonly ILocationRepository _locationRepository;
    private readonly ILogger _logger;

    private Dictionary<string, DungeonRewardPatcherData> _dungeonRewardPatcherData = new()
    {
        { "Dungeon1", new DungeonRewardPatcherData("Dungeon 1 - Dungeon reward", new ByName("Dungeon1CrystalBoss", typeof(CrystalBoss))) },
        { "Dungeon2", new DungeonRewardPatcherData("Dungeon 2 - Dungeon reward", new ByName("Dungeon2CrystalBoss", typeof(CrystalBoss))) },
        { "Dungeon3", new DungeonRewardPatcherData("Dungeon 3 - Dungeon reward", new ByName("Dungeon3CrystalBoss", typeof(CrystalBoss))) },
        { "Dungeon4", new DungeonRewardPatcherData("Dungeon 4 - Dungeon reward", new ByName("Dungeon4CrystalBoss", typeof(CrystalBoss))) }
    };

    private Dictionary<string, IPatchAction> _patchActions = new();

    public DungeonRewardPatcher(IRandomizerEngine randomizerEngine, IObjectFinder objectFinder, ILocationRepository locationRepository, ILogger logger)
    {
        _randomizerEngine = randomizerEngine;
        _objectFinder = objectFinder;
        _locationRepository = locationRepository;
        _logger = logger ?? new NullLogger();
    }

    public void OnEnteringGameLocation(string locationName)
    {
        if (!_randomizerEngine.IsRandomized())
        {
            return;
        }

        if (!_patchActions.ContainsKey(locationName) && _dungeonRewardPatcherData.TryGetValue(locationName, out DungeonRewardPatcherData dungeonRewardPatcherData))
        {
            IPatchAction patchAction = PatchDungeonReward(dungeonRewardPatcherData);
            _patchActions.Add(locationName, patchAction);
            patchAction.Patch();
        }
    }

    public void OnExitingGame()
    {
        foreach (IPatchAction patchAction in _patchActions.Values)
        {
            patchAction.Dispose();
        }
        _patchActions.Clear();
    }

    private IPatchAction PatchDungeonReward(DungeonRewardPatcherData dungeonRewardPatcherData)
    {
        GameObject rewardObject = _objectFinder.FindObject(dungeonRewardPatcherData.Selector);
        Location location = _locationRepository.Get(dungeonRewardPatcherData.LocationIdentifier);

        AddComponentAction<RandomizerDungeonRewardComponent> addComponentAction = new AddComponentAction<RandomizerDungeonRewardComponent>(rewardObject);
        addComponentAction.OnComponentAdded += (component) =>
        {
            component.Location = location;
        };

        return new LoggableAction(addComponentAction, _logger);
    }
}
