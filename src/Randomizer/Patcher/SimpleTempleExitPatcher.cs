using System.Collections.Generic;
using UnityEngine;

namespace MinishootRandomizer;

public class SimpleTempleExitPatcher
{
    private readonly IRandomizerEngine _randomizerEngine;
    private readonly IObjectFinder _objectFinder;
    private readonly ILogger _logger = new NullLogger();

    private Dictionary<string, IPatchAction> _patchActions = new();
    private Dictionary<string, List<ISelector>> _objectsToDeactivate = new()
    {
        {
            "Temple1",
            new List<ISelector>
            {
                new ByName("Temple1Checkpoint1"),
                new ByName("Temple1EncounterClose5"),
                new ByName("InputPromptPopInPowerBomb"),
            }
        },
        {
            "Temple2",
            new List<ISelector>
            {
                new ByName("Temple2Checkpoint1"),
                new ByName("InputPromptPopInPowerSlow"),
                new ByName("Temple2 027 ShooterT1 S2 Solid"),
                new ByName("Temple2 028 ShooterT1 S2 Solid"),
                new ByName("Temple2 031 ShooterT1 S2 Solid"),
                new ByName("Temple2 032 ShooterT1 S2 Solid"),
                new ByName("Temple2 035 ShooterT1 S2 Solid"),
                new ByName("Temple2 036 ShooterT1 S2 Solid"),
                new ByName("Temple2 039 ShooterT1 S2 Solid"),
                new ByName("Temple2 040 ShooterT1 S2 Solid"),
                new ByName("Temple2 043 ShooterT1 S2 Solid"),
                new ByName("Temple2 044 ShooterT1 S2 Solid"),
            }
        },
        {
            "Temple3",
            new List<ISelector>
            {
                new ByName("Temple3Checkpoint1"),
                new ByName("InputPromptPopInPowerAlly"),
                new ByName("Temple3DoorLocked4"),
                new ByName("Temple3 038 ShooterT2 S2"),
                new ByName("Temple3 039 ShooterT2 S2"),
            }
        }
    };

    public SimpleTempleExitPatcher(IRandomizerEngine randomizerEngine, IObjectFinder objectFinder, ILogger logger = null)
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

        if (!_patchActions.ContainsKey(locationName) && _objectsToDeactivate.ContainsKey(locationName))
        {
            IPatchAction patchAction = CreatePatchAction(locationName);
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

    private IPatchAction CreatePatchAction(string locationName)
    {
        CompositeAction compositeAction = new("SimpleTempleExitPatcher for " + locationName);
        foreach (ISelector selector in _objectsToDeactivate[locationName])
        {
            GameObject gameObject = _objectFinder.FindObject(selector);
            compositeAction.Add(new RemoveGameObjectAction(gameObject));
        }

        return new LoggableAction(compositeAction, _logger);
    }
}
