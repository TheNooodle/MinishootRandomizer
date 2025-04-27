using UnityEngine;

namespace MinishootRandomizer;

public class FamilyReunitedPatcher
{
    private readonly IRandomizerEngine _randomizerEngine;
    private readonly IObjectFinder _objectFinder;
    private readonly ILogger _logger = new NullLogger();

    private IPatchAction _patchAction = null;

    public FamilyReunitedPatcher(IRandomizerEngine randomizerEngine, IObjectFinder objectFinder, ILogger logger = null)
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

        if (locationName == "Cave" && _patchAction == null)
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
        GameObject gameObject = _objectFinder.FindObject(new ByName("FamillyReunitedCheck", typeof(Transform)));
        RemoveGameObjectAction removeGameObjectAction = new RemoveGameObjectAction(gameObject);

        return new LoggableAction(removeGameObjectAction, _logger);
    }
}
