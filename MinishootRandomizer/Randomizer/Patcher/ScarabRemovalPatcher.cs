using System.Collections.Generic;
using UnityEngine;

namespace MinishootRandomizer;

public class ScarabRemovalPatcher
{
    private readonly IRandomizerEngine _randomizerEngine;
    private readonly IObjectFinder _objectFinder;
    private readonly ILogger _logger = new NullLogger();

    private Dictionary<string, IPatchAction> _patchActions = new Dictionary<string, IPatchAction>();

    public ScarabRemovalPatcher(IRandomizerEngine randomizerEngine, IObjectFinder objectFinder, ILogger logger = null)
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

        ScarabSanity scarabSanity = _randomizerEngine.GetSetting<ScarabSanity>();
        if (!scarabSanity.Enabled)
        {
            return;
        }

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
        GameObject[] scarabObjects = _objectFinder.FindObjects(new ByComponent(typeof(ScarabPickup)));

        foreach (GameObject scarabObject in scarabObjects)
        {
            if (!_patchActions.ContainsKey(scarabObject.gameObject.name))
            {
                RemoveGameObjectAction removeGameObjectAction = new RemoveGameObjectAction(scarabObject);
                LoggableAction loggableAction = new LoggableAction(removeGameObjectAction, _logger);
                _patchActions.Add(scarabObject.gameObject.name, loggableAction);
                loggableAction.Patch();
            }
        }
    }
}
