using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MinishootRandomizer;

public class BossPatcher
{
    private readonly IRandomizerEngine _randomizerEngine;
    private readonly IObjectFinder _objectFinder;
    private readonly ILogger _logger = new NullLogger();

    private Dictionary<string, BossPatcherData> _bossPatcherData = new()
    {
        { "Dungeon5", new BossPatcherData(Goals.Dungeon5, new ByName("Dungeon5 167 Boss4 T3 S3", typeof(Boss))) },
        { "Snow", new BossPatcherData(Goals.Snow, new ByName("Snow 003 BossTrueLast T3 S3", typeof(Boss)))}
    };
    private Dictionary<string, IPatchAction> _patchActions = new();

    public BossPatcher(IRandomizerEngine randomizerEngine, IObjectFinder objectFinder, ILogger logger = null)
    {
        _randomizerEngine = randomizerEngine;
        _objectFinder = objectFinder;
        _logger = logger ?? new NullLogger();
    }

    public void OnEnteringGameLocation(string locationName)
    {
        if (!_randomizerEngine.IsRandomized())
        {
            return;
        }

        if (!_patchActions.ContainsKey(locationName) && _bossPatcherData.TryGetValue(locationName, out BossPatcherData bossPatcherData))
        {
            IPatchAction patchAction = PatchBoss(bossPatcherData, locationName);
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

    private IPatchAction PatchBoss(BossPatcherData bossPatcherData, string locationName)
    {
        CompositeAction compositeAction = new("BossPatcher for " + locationName);
        GameObject bossObject = _objectFinder.FindObject(bossPatcherData.Selector);

        compositeAction.Add(new AddComponentAction<EventDestroyableComponent>(bossObject));

        AddComponentAction<RandomizerBossComponent> addComponentAction = new AddComponentAction<RandomizerBossComponent>(bossObject);
        addComponentAction.OnComponentAdded += (component) =>
        {
            component.CorrespondingGoal = bossPatcherData.CorrespondingGoal;
        };
        compositeAction.Add(addComponentAction);

        return new LoggableAction(compositeAction, _logger);
    }
}

public class BossPatcherData
{
    public Goals CorrespondingGoal;
    public ISelector Selector;

    public BossPatcherData(Goals correspondingGoal, ISelector selector)
    {
        CorrespondingGoal = correspondingGoal;
        Selector = selector;
    }
}
