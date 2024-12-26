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
            return LogicAccessibility.Inaccessible;
        }

        LogicParsingResult parsingResult = _logicParser.ParseLogic(location.LogicRule, state, settings);

        return parsingResult.Result ? LogicAccessibility.InLogic : LogicAccessibility.Inaccessible;
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
            else
            {
                accessibilitySet.AddInaccessibleLocation(location);
            }
        }

        return accessibilitySet;
    }
}
