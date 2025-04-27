namespace MinishootRandomizer;

public class LoggableAction : IPatchAction
{
    private readonly IPatchAction _action;
    private readonly ILogger _logger;

    public LoggableAction(IPatchAction action, ILogger logger)
    {
        _action = action;
        _logger = logger;
    }

    public void Dispose()
    {
        _logger.LogInfo($"Disposing {_action}");
        _action.Dispose();
    }

    public void Patch()
    {
        _logger.LogInfo($"Patching {_action}");
        _action.Patch();
    }

    public void Unpatch()
    {
        _logger.LogInfo($"Unpatching {_action}");
        _action.Unpatch();
    }
}
