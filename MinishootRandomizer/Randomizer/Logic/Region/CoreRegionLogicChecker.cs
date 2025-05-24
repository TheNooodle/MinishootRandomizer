using System.Collections.Generic;
using System.Linq;

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

    public LogicAccessibility GetRegionAccessibility(Region region, LogicState state)
    {
        Dictionary<Region, LogicAccessibility> regionsAccessibility = CrawlRegions(state);
        return regionsAccessibility.TryGetValue(region, out LogicAccessibility accessibility)
            ? accessibility
            : LogicAccessibility.OutOfLogic;
    }

    public Dictionary<Region, LogicAccessibility> GetRegionsAccessibility(LogicState state)
    {
        return CrawlRegions(state);
    }

    private Dictionary<Region, LogicAccessibility> CrawlRegions(LogicState logicState)
    {
        Dictionary<Region, LogicAccessibility> regionsAccessibility = new Dictionary<Region, LogicAccessibility>();

        // We start from the starting region
        Region startRegion = _regionRepository.Get(Region.StartingGrottoLake);
        regionsAccessibility.Add(startRegion, LogicAccessibility.InLogic);

        List<Transition> traversedTransitions = new List<Transition>();
        int previousReachableCount;
        int maxIterations = 1000; // Prevent infinite loops

        // We perform a breadth-first search to find all reachable regions
        // The loop will stop when no new regions are reachable
        // (this is because some transitions may require yet-to-be-reached regions)
        // First off, we check for logically accessible regions.
        do
        {
            previousReachableCount = regionsAccessibility.Count;
            List<Region> newReachableRegions = new List<Region>();

            foreach (Region region in regionsAccessibility.Keys)
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
                if (!regionsAccessibility.Keys.Any(r => r.Name == region.Name))
                {
                    // If the region is not already in the dictionary, we add it as logically accessible
                    // This means it can be reached with the current logic state
                    regionsAccessibility.Add(region, LogicAccessibility.InLogic);
                }
            }
        } while (previousReachableCount < regionsAccessibility.Count && maxIterations-- > 0);

        // Now we check for out-of-logic regions
        LogicState outOfLogicState = new OutOfLogicStateDecorator(logicState);
        maxIterations = 1000; // Reset max iterations for out-of-logic check
        do
        {
            previousReachableCount = regionsAccessibility.Count;
            List<Region> newReachableRegions = new List<Region>();

            foreach (Region region in regionsAccessibility.Keys)
            {
                List<Transition> transitions = _transitionRepository.GetFromOriginRegion(region);
                foreach (Transition transition in transitions)
                {
                    if (traversedTransitions.Contains(transition))
                    {
                        continue;
                    }

                    LogicParsingResult parsingResult = _logicParser.ParseLogic(transition.LogicRule, outOfLogicState);
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
                if (!regionsAccessibility.Keys.Any(r => r.Name == region.Name))
                {
                    // If the region is not already in the dictionary, we add it as out-of-logic accessible
                    // This means it can be reached with the current out-of-logic state
                    regionsAccessibility.Add(region, LogicAccessibility.OutOfLogic);
                }
            }
        } while (previousReachableCount < regionsAccessibility.Count && maxIterations-- > 0);

        // We finally add the remaining regions as inaccessible
        List<Region> allRegions = _regionRepository.GetAll();
        foreach (Region region in allRegions)
        {
            if (!regionsAccessibility.Keys.Any(r => r.Name == region.Name))
            {
                regionsAccessibility.Add(region, LogicAccessibility.Inaccessible);
            }
        }

        if (maxIterations <= 0)
        {
            _logger.LogError("Error: Infinite loop detected in region reachability calculation");
        }

        return regionsAccessibility;
    }
}
