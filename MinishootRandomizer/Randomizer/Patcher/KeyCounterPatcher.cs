namespace MinishootRandomizer;

public class KeyCounterPatcher
{
    private readonly IRandomizerEngine _randomizerEngine;
    private readonly IKeyCounterObjectFactory _keyCounterObjectFactory;
    private readonly ILogger _logger;

    private IPatchAction _patchAction = null;

    public KeyCounterPatcher(IRandomizerEngine randomizerEngine, IKeyCounterObjectFactory keyCounterObjectFactory, ILogger logger)
    {
        _randomizerEngine = randomizerEngine;
        _keyCounterObjectFactory = keyCounterObjectFactory;
        _logger = logger;
    }

    public void OnEnteringGameLocation(string gameLocationName)
    {
        if (!_randomizerEngine.IsRandomized())
        {
            return;
        }

        if (_patchAction == null)
        {
            Patch();
        }
    }

    private void Patch()
    {
        IPatchAction patchAction = new LoggableAction(
            new CreateKeyCounterViewAction(_keyCounterObjectFactory),
            _logger
        );

        patchAction.Patch();
        _patchAction = patchAction;
    }

    public void OnExitingGame()
    {
        if (_patchAction != null)
        {
            _patchAction.Dispose();
            _patchAction = null;
        }
    }
}
