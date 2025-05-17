using System.Collections.Generic;

namespace MinishootRandomizer;

public class CoreLocationLogicChecker : ILocationLogicChecker
{
    private readonly ILogicParser _logicParser;
    private readonly IRegionLogicChecker _regionLogicChecker;
    private readonly IRegionRepository _regionRepository;
    private readonly ILocationRepository _locationRepository;

    public CoreLocationLogicChecker(
        ILogicParser logicParser,
        IRegionLogicChecker regionLogicChecker,
        IRegionRepository regionRepository,
        ILocationRepository locationRepository
    ) {
        _logicParser = logicParser;
        _regionLogicChecker = regionLogicChecker;
        _regionRepository = regionRepository;
        _locationRepository = locationRepository;
    }

    public LogicAccessibility CheckLocationLogic(LogicState logicState, Location location)
    {
        Region region = _regionRepository.GetRegionByLocation(location);

        if (!_regionLogicChecker.CanReachRegion(region, logicState))
        {
            // If the location is not reachable, we check if it's out of logic
            LogicState outOfLogicState = new OutOfLogicStateDecorator(logicState);

            // If the location is reachable out of logic and the location is attainable, it's accessible out of logic.
            return !_regionLogicChecker.CanReachRegion(region, outOfLogicState) && _logicParser.ParseLogic(location.LogicRule, outOfLogicState).Result
                ? LogicAccessibility.OutOfLogic
                : LogicAccessibility.Inaccessible
            ;
        }

        LogicParsingResult parsingResult = _logicParser.ParseLogic(location.LogicRule, logicState);
        if (parsingResult.Result)
        {
            return LogicAccessibility.InLogic;
        }
        else
        {
            LogicState outOfLogicState = new OutOfLogicStateDecorator(logicState);

            // If the location is reachable out of logic and the location is attainable, it's accessible out of logic.
            return _logicParser.ParseLogic(location.LogicRule, outOfLogicState).Result
                ? LogicAccessibility.OutOfLogic
                : LogicAccessibility.Inaccessible
            ;
        }
    }

    public LocationAccessibilitySet CheckAllLocationsLogic(LogicState logicState)
    {
        List<Location> locations = _locationRepository.GetAll();
        LocationAccessibilitySet accessibilitySet = new LocationAccessibilitySet();

        foreach (Location location in locations)
        {
            LogicAccessibility accessibility = CheckLocationLogic(logicState, location);

            if (accessibility == LogicAccessibility.InLogic)
            {
                accessibilitySet.AddInLogicLocation(location);
            }
            else if (accessibility == LogicAccessibility.OutOfLogic)
            {
                accessibilitySet.AddOutOfLogicLocation(location);
            }
            else
            {
                accessibilitySet.AddInaccessibleLocation(location);
            }
        }

        return accessibilitySet;
    }
}
