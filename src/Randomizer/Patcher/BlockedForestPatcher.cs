namespace MinishootRandomizer;

public class BlockedForestPatcher
{
    private readonly IRandomizerEngine _randomizerEngine;
    private readonly IObjectFinder _objectFinder;
    private readonly IGameObjectFactory _gameObjectFactory;
    private readonly ILogger _logger = new NullLogger();

    private IPatchAction _patchAction = null;

    public BlockedForestPatcher(
        IRandomizerEngine randomizerEngine,
        IObjectFinder objectFinder,
        IGameObjectFactory gameObjectFactory,
        ILogger logger = null
    ) {
        _randomizerEngine = randomizerEngine;
        _objectFinder = objectFinder;
        _gameObjectFactory = gameObjectFactory;
        _logger = logger ?? new NullLogger();
    }

    public void OnEnteringGameLocation(string locationName)
    {
        if (!_randomizerEngine.IsRandomized())
        {
            return;
        }

        BlockedForest blockedForest = _randomizerEngine.GetSetting<BlockedForest>();
        if (!blockedForest.Enabled)
        {
            return;
        }

        if (_patchAction == null)
        {
            _patchAction = CreatePatchAction();
        }

        if (locationName == "Overworld")
        {
            _patchAction.Patch();
        }
        else if (_patchAction != null)
        {
            _patchAction.Unpatch();
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
        CompositeAction action = new CompositeAction("Blocked Forest Patch");
        UnityEngine.Vector3 bushPosition = new UnityEngine.Vector3(-2f, -49f, 0f);
        ISelector selector = new ByProximity(typeof(PlantDestroyable), bushPosition, 3f);
        UnityEngine.GameObject[] bushes = _objectFinder.FindObjects(selector);
        for (int i = 0; i < bushes.Length; i++)
        {
            UnityEngine.GameObject bush = bushes[i];
            UnityEngine.GameObject replacement = _gameObjectFactory.CreateGameObject("Rock Destroyable");
            replacement.AddComponent<WorldStateChecker>();
            replacement.transform.position = bush.transform.position;
            replacement.transform.rotation = bush.transform.rotation;
            replacement.name = "ForestDestroyableReplacement" + i;

            IPatchAction replaceAction = new ReplaceGameObjectAction(bush, replacement);
            action.Add(replaceAction);
        }

        return new LoggableAction(action, _logger);
    }
}
