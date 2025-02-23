using System.Collections.Generic;

namespace MinishootRandomizer;

public class CoreLogicChecker : ILogicChecker
{
    private readonly ILogicStateProvider _logicStateProvider;
    private readonly ILogicParser _logicParser;
    private readonly ISettingsProvider _settingsProvider;
    private readonly IRegionRepository _regionRepository;
    private readonly ILocationRepository _locationRepository;
    private readonly ILogger _logger = new NullLogger();

    public CoreLogicChecker(ILogicStateProvider logicStateProvider, ILogicParser logicParser, ISettingsProvider settingsProvider, IRegionRepository regionRepository, ILocationRepository locationRepository, ILogger logger = null)
    {
        _logicStateProvider = logicStateProvider;
        _logicParser = logicParser;
        _settingsProvider = settingsProvider;
        _regionRepository = regionRepository;
        _locationRepository = locationRepository;
        _logger = logger ?? new NullLogger();
    }

    public LogicAccessibility CheckLocationLogic(Location location)
    {
        List<ISetting> settings = _settingsProvider.GetSettings();
        LogicState state = _logicStateProvider.GetLogicState();
        Region region = _regionRepository.GetRegionByLocation(location);

        if (!state.CanReach(region))
        {
            // If the location is not reachable, we check if it's out of logic
            LogicState outOfLogicState = _logicStateProvider.GetLogicState(LogicTolerance.Lenient);

            // If the location is reachable out of logic and the location is attainable, it's accessible out of logic.
            return outOfLogicState.CanReach(region) && _logicParser.ParseLogic(location.LogicRule, outOfLogicState, settings).Result
                ? LogicAccessibility.OutOfLogic
                : LogicAccessibility.Inaccessible
            ;
        }

        LogicParsingResult parsingResult = _logicParser.ParseLogic(location.LogicRule, state, settings);
        if (parsingResult.Result)
        {
            return LogicAccessibility.InLogic;
        }
        else
        {
            LogicState outOfLogicState = _logicStateProvider.GetLogicState(LogicTolerance.Lenient);

            // If the location is reachable out of logic and the location is attainable, it's accessible out of logic.
            return _logicParser.ParseLogic(location.LogicRule, outOfLogicState, settings).Result
                ? LogicAccessibility.OutOfLogic
                : LogicAccessibility.Inaccessible
            ;
        }
    }

    public LocationAccessibilitySet CheckAllLocationsLogic()
    {
        List<Location> locations = _locationRepository.GetAll();
        LocationAccessibilitySet accessibilitySet = new LocationAccessibilitySet();

        foreach (Location location in locations)
        {
            LogicAccessibility accessibility = CheckLocationLogic(location);

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
