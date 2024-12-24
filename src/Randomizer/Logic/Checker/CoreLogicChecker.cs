namespace MinishootRandomizer;

public class CoreLogicChecker : ILogicChecker
{
    private readonly ILogicStateProvider _logicStateProvider;
    private readonly ILogicParser _logicParser;
    private readonly ILogger _logger = new NullLogger();

    public CoreLogicChecker(ILogicStateProvider logicStateProvider, ILogicParser logicParser, ILogger logger = null)
    {
        _logicStateProvider = logicStateProvider;
        _logicParser = logicParser;
        _logger = logger ?? new NullLogger();
    }

    public LogicAccessibility CheckLogic(Location location)
    {
        throw new System.NotImplementedException();
    }
}
