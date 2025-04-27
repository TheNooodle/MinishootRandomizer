using System.Collections.Generic;
using UnityEngine;

namespace MinishootRandomizer;

public class XpCrystalRemovalPatcher
{
    private readonly IRandomizerEngine _randomizerEngine;
    private readonly IObjectFinder _objectFinder;
    private readonly ILogger _logger;

    private List<IPatchAction> _patchActions = new();

    public XpCrystalRemovalPatcher(IRandomizerEngine randomizerEngine, IObjectFinder objectFinder, ILogger logger)
    {
        _randomizerEngine = randomizerEngine;
        _objectFinder = objectFinder;
        _logger = logger;
    }

    public void OnEnteringGameLocation(string locationName)
    {
        if (!_randomizerEngine.IsRandomized())
        {
            return;
        }

        ShardSanity shardSanity = _randomizerEngine.GetSetting<ShardSanity>();
        if (!shardSanity.Enabled)
        {
            return;
        }

        Patch();
    }

    public void OnExitingGame()
    {
        foreach (IPatchAction patchAction in _patchActions)
        {
            patchAction.Dispose();
        }
        _patchActions.Clear();
    }

    private void Patch()
    {
        GameObject[] destroyableObjects = _objectFinder.FindObjects(new ByComponent(typeof(CrystalDestroyable)));

        foreach (GameObject destroyableObject in destroyableObjects)
        {
            // Because we want to access a private property, we need to use reflection.
            CrystalDestroyable destroyableComponent = destroyableObject.GetComponent<CrystalDestroyable>();
            ForceDeactivationComponent forceDeactivationComponent = destroyableObject.GetComponent<ForceDeactivationComponent>();
            if (!destroyableComponent || forceDeactivationComponent)
            {
                continue;
            }
            bool dropXp = ReflectionHelper.GetPrivateFieldValue<bool>(destroyableComponent, "dropXp");
            if (dropXp)
            {
                RemoveGameObjectAction removeGameObjectAction = new RemoveGameObjectAction(destroyableObject);
                LoggableAction loggableAction = new LoggableAction(removeGameObjectAction, _logger);
                _patchActions.Add(loggableAction);
                loggableAction.Patch();
            }
        }
    }
}
