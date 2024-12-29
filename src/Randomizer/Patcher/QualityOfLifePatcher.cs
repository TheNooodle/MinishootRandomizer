using UnityEngine;

namespace MinishootRandomizer;

public class QualityOfLifePatcher
{
    private readonly IRandomizerEngine _randomizerEngine;
    private readonly IObjectFinder _objectFinder;
    private readonly ILogger _logger = new NullLogger();

    private IPatchAction _patchAction = null;

    public QualityOfLifePatcher(IRandomizerEngine randomizerEngine, IObjectFinder objectFinder, ILogger logger = null)
    {
        _randomizerEngine = randomizerEngine;
        _objectFinder = objectFinder;
        _logger = logger ?? new NullLogger();
    }

    public void OnEnteringGameLocation(string location)
    {
        if (!_randomizerEngine.IsRandomized())
        {
            return;
        }

        if (_patchAction == null)
        {
            _patchAction = CreatePatch();
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

    private IPatchAction CreatePatch()
    {
        CompositeAction compositeAction = new CompositeAction("QualityOfLife");

        // Remove the control mod select popup
        GameObject trigger = _objectFinder.FindObject(new ByName("PopUpControlModeTrigger"));
        if (trigger != null)
        {
            compositeAction.Add(new RemoveGameObjectAction(trigger));
        }

        return new LoggableAction(compositeAction, _logger);
    }
}
