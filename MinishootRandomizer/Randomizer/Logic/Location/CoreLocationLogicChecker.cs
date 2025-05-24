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
        LogicAccessibility regionAccessibility = _regionLogicChecker.GetRegionAccessibility(region, logicState);

        if (regionAccessibility == LogicAccessibility.Inaccessible)
        {
            // If the region is inaccessible, the location is also inaccessible.
            return LogicAccessibility.Inaccessible;
        }
        else
        {
            LogicParsingResult parsingResult = _logicParser.ParseLogic(location.LogicRule, logicState);
            if (parsingResult.Result)
            {
                // If the location is reachable in logic from within the region, then it's accessibility is the same as the region's accessibility.
                return regionAccessibility;
            }
            else
            {
                // If the location is not reachable in logic, we check if it's reachable out of logic.
                LogicState outOfLogicState = new OutOfLogicStateDecorator(logicState);
                LogicParsingResult outOfLogicParsingResult = _logicParser.ParseLogic(location.LogicRule, outOfLogicState);

                // If the location is reachable out of logic, then it's accessible out of logic.
                return outOfLogicParsingResult.Result ? LogicAccessibility.OutOfLogic : LogicAccessibility.Inaccessible;
            }
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
