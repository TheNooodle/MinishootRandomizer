using System.Collections.Generic;

namespace MinishootRandomizer;

public class LocalLogicStateProvider : ILogicStateProvider
{
    private readonly ILogicParser _logicParser;
    private readonly IRegionRepository _regionRepository;
    private readonly ITransitionRepository _transitionRepository;
    private readonly IItemRepository _itemRepository;
    private readonly ISettingsProvider _settingsProvider;
    private readonly ILogger _logger = new NullLogger();

    private LogicState _logicState = null;
    private List<Transition> _traversedTransitions = new List<Transition>();

    public LocalLogicStateProvider(ILogicParser logicParser, IRegionRepository regionRepository, ITransitionRepository transitionRepository, IItemRepository itemRepository, ISettingsProvider settingsProvider, ILogger logger = null)
    {
        _logicParser = logicParser;
        _regionRepository = regionRepository;
        _transitionRepository = transitionRepository;
        _itemRepository = itemRepository;
        _settingsProvider = settingsProvider;
        _logger = logger ?? new NullLogger();
    }

    public LogicState GetLogicState()
    {
        LogicState logicState = _logicState ?? new LogicState();
        ItemsPass(logicState);
        RegionsPass(logicState);
        _logicState = logicState;

        return logicState;
    }

    private void ItemsPass(LogicState logicState)
    {
        List<Item> items = _itemRepository.GetAll();
        foreach (Item item in items)
        {
            int count = item.GetOwnedQuantity();
            if (count > 0)
            {
                logicState.SetItemCount(item, count);
            }
        }
    }

    private void RegionsPass(LogicState logicState)
    {
        // We start from the starting region
        Region startRegion = _regionRepository.Get(Region.StartingGrottoLake);
        logicState.AddReachableRegion(startRegion);

        List<Transition> traversedTransitions = _traversedTransitions.Count > 0 ? _traversedTransitions : new List<Transition>();
        List<ISetting> settings = _settingsProvider.GetSettings();
        int previousReachableCount;
        int maxIterations = 1000; // Prevent infinite loops

        // We perform a breadth-first search to find all reachable regions
        // The loop will stop when no new regions are reachable
        // (this is because some transitions may require yet-to-be-reached regions)
        do
        {
            previousReachableCount = logicState.GetReachableRegions().Count;
            List<Region> newReachableRegions = new List<Region>();

            foreach (Region region in logicState.GetReachableRegions())
            {
                List<Transition> transitions = _transitionRepository.GetFromOriginRegion(region);
                foreach (Transition transition in transitions)
                {
                    if (traversedTransitions.Contains(transition))
                    {
                        continue;
                    }

                    LogicParsingResult parsingResult = _logicParser.ParseLogic(transition.LogicRule, logicState, settings);
                    if (parsingResult.Result)
                    {
                        traversedTransitions.Add(transition);
                        Region destinationRegion = _regionRepository.Get(transition.To);
                        newReachableRegions.Add(destinationRegion);
                    }
                }
            }

            foreach (Region region in newReachableRegions)
            {
                logicState.AddReachableRegion(region);
            }
        } while(previousReachableCount < logicState.GetReachableRegions().Count && maxIterations-- > 0);

        if (maxIterations <= 0)
        {
            _logger.LogError("Error: Infinite loop detected in region reachability calculation");
        }

        _traversedTransitions = traversedTransitions;
    }

    public void OnExitingGame()
    {
        _logicState = null;
        _traversedTransitions.Clear();
    }
}
