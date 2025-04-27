
namespace MinishootRandomizer;

public class NotificationPatcher
{
    private readonly IRandomizerEngine _randomizerEngine;
    private readonly INotificationObjectFactory _notificationObjectFactory;
    private readonly ILogger _logger;

    private IPatchAction _patchAction = null;

    public NotificationPatcher(IRandomizerEngine randomizerEngine, INotificationObjectFactory notificationObjectFactory, ILogger logger)
    {
        _randomizerEngine = randomizerEngine;
        _notificationObjectFactory = notificationObjectFactory;
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
            new CreateNotificationViewAction(_notificationObjectFactory),
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
