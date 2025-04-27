using System.Collections.Generic;

namespace MinishootRandomizer;

// @deprecated, this class is here to ensure BC until the next major version.
// The next major version will contains those locations directly in the CSV file.
public class InMemoryLocationRepository : ILocationRepository
{
    private readonly ILocationRepository _innerRepository;

    private readonly Dictionary<string, Location> _locations = new()
    {
        { "Dungeon 5 - Beat the boss", new GoalLocation("Dungeon 5 - Beat the boss", "true", LocationPool.Goal, Goals.Dungeon5) },
        { "Snow - Beat the Unchosen", new GoalLocation("Snow - Beat the Unchosen", "true", LocationPool.Goal, Goals.Snow) }
    };

    public InMemoryLocationRepository(ILocationRepository innerRepository)
    {
        _innerRepository = innerRepository;
    }

    public Location Get(string identifier)
    {
        if (_locations.ContainsKey(identifier))
        {
            return _locations[identifier];
        }

        return _innerRepository.Get(identifier);
    }

    public List<Location> GetAll()
    {
        List<Location> locations = new();
        locations.AddRange(_locations.Values);
        locations.AddRange(_innerRepository.GetAll());

        return locations;
    }
}
