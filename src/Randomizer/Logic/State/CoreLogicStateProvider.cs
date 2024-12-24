namespace MinishootRandomizer;

public class CoreLogicStateProvider : ILogicStateProvider
{
    private readonly ILogicParser _logicParser;
    private readonly IRegionRepository _regionRepository;
    private readonly ITransitionRepository _transitionRepository;
    private readonly ILogger _logger = new NullLogger();

    public CoreLogicStateProvider(ILogicParser logicParser, IRegionRepository regionRepository, ITransitionRepository transitionRepository, ILogger logger = null)
    {
        _logicParser = logicParser;
        _regionRepository = regionRepository;
        _transitionRepository = transitionRepository;
        _logger = logger ?? new NullLogger();
    }

    public LogicState GetLogicState()
    {
        throw new System.NotImplementedException();
    }
}
