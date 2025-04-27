using System.Collections.Generic;

namespace MinishootRandomizer;

// @deprecated, this class is here to ensure BC until the next major version.
// The next major version will contains those locations directly in the CSV file.
public class InMemoryRegionRepository : IRegionRepository
{
    private readonly IRegionRepository _innerRepository;

    private readonly Dictionary<string, List<string>> _newLocationsToRegionsMap = new()
    {
        { "Dungeon 5 - Boss", new List<string> { "Dungeon 5 - Beat the boss" } },
        { "Snow", new List<string> { "Snow - Beat the Unchosen" } }
    };

    public InMemoryRegionRepository(IRegionRepository innerRepository)
    {
        _innerRepository = innerRepository;
    }

    public Region Get(string identifier)
    {
        Region region = _innerRepository.Get(identifier);
        if (_newLocationsToRegionsMap.ContainsKey(identifier))
        {
            foreach (string locationName in _newLocationsToRegionsMap[identifier])
            {
                region.AddLocationName(locationName);
            }
        }

        return region;
    }

    public Region GetRegionByLocation(Location location)
    {
        foreach (string regionName in _newLocationsToRegionsMap.Keys)
        {
            if (_newLocationsToRegionsMap[regionName].Contains(location.Identifier))
            {
                return Get(regionName);
            }
        }

        return _innerRepository.GetRegionByLocation(location);
    }
}
