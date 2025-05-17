using System.Collections.Generic;

namespace MinishootRandomizer;

public class CoreRegionLogicChecker : IRegionLogicChecker
{
    private readonly IRegionRepository _regionRepository;
    private readonly ITransitionRepository _transitionRepository;
    private readonly ILogicParser _logicParser;
    private readonly ILogger _logger;

    public CoreRegionLogicChecker(
        IRegionRepository regionRepository,
        ITransitionRepository transitionRepository,
        ILogicParser logicParser,
        ILogger logger = null
    )
    {
        _regionRepository = regionRepository;
        _transitionRepository = transitionRepository;
        _logicParser = logicParser;
        _logger = logger ?? new NullLogger();
    }

    public bool CanReachRegion(Region region, LogicState state)
    {
        List<Region> reachableRegions = CrawlRegions(state);
        return reachableRegions.Contains(region);
    }

    public List<Region> GetReachableRegions(LogicState state)
    {
        List<Region> reachableRegions = CrawlRegions(state);
        return reachableRegions;
    }

    private List<Region> CrawlRegions(LogicState logicState)
    {
        List<Region> reachableRegions = new List<Region>();

        // We start from the starting region
        Region startRegion = _regionRepository.Get(Region.StartingGrottoLake);
        reachableRegions.Add(startRegion);

        List<Transition> traversedTransitions = new List<Transition>();
        int previousReachableCount;
        int maxIterations = 1000; // Prevent infinite loops

        // We perform a breadth-first search to find all reachable regions
        // The loop will stop when no new regions are reachable
        // (this is because some transitions may require yet-to-be-reached regions)
        do
        {
            previousReachableCount = reachableRegions.Count;
            List<Region> newReachableRegions = new List<Region>();

            foreach (Region region in reachableRegions)
            {
                List<Transition> transitions = _transitionRepository.GetFromOriginRegion(region);
                foreach (Transition transition in transitions)
                {
                    if (traversedTransitions.Contains(transition))
                    {
                        continue;
                    }

                    LogicParsingResult parsingResult = _logicParser.ParseLogic(transition.LogicRule, logicState);
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
                reachableRegions.Add(region);
            }
        } while(previousReachableCount < reachableRegions.Count && maxIterations-- > 0);

        if (maxIterations <= 0)
        {
            _logger.LogError("Error: Infinite loop detected in region reachability calculation");
        }

        return reachableRegions;
    }
}
