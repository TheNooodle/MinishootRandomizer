using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using CsvHelper;

namespace MinishootRandomizer
{
    public class CsvLocationRepository : ILocationRepository
    {
        private readonly string _csvPath;
        private readonly ILocationFactory _locationFactory;
        private readonly ILogger _logger;

        private Dictionary<string, Location> _locations = null;

        public CsvLocationRepository(string csvPath, ILocationFactory locationFactory, ILogger logger = null)
        {
            _csvPath = csvPath;
            _locationFactory = locationFactory;
            _logger = logger ?? new NullLogger();
        }

        public Location Get(string identifier)
        {
            if (_locations == null)
            {
                LoadLocations();
            }

            if (!_locations.ContainsKey(identifier))
            {
                throw new LocationNotFoundException(identifier);
            }

            return _locations[identifier];
        }

        public List<Location> GetAll()
        {
            if (_locations == null)
            {
                LoadLocations();
            }

            return new List<Location>(_locations.Values);
        }

        private void LoadLocations()
        {
            var assembly = Assembly.GetExecutingAssembly();
            _locations = new Dictionary<string, Location>();

            using Stream stream = assembly.GetManifestResourceStream(_csvPath);
            using StreamReader reader = new(stream);
            using CsvReader csv = new(reader, CultureInfo.InvariantCulture);
            csv.Read();
            csv.ReadHeader();
            while (csv.Read())
            {
                try {
                    Location location = _locationFactory.CreateLocation(
                        csv.GetField("Name"),
                        csv.GetField("Logic rule"),
                        MapLocationPool(csv.GetField("Location pool"))
                    );
                    _locations[location.Identifier] = location;
                } catch (InvalidLocationException e) {
                    _logger.LogWarning(e.Message);
                }
            }
        }

        private LocationPool MapLocationPool(string pool)
        {
            return pool switch
            {
                "Default" => LocationPool.Default,
                "XP Crystals" => LocationPool.XpCrystals,
                "NPC" => LocationPool.Npc,
                "Scarab" => LocationPool.Scarab,
                "Spirit" => LocationPool.Spirit,
                "Dungeon Small Key" => LocationPool.DungeonSmallKey,
                "Dungeon Big Key" => LocationPool.DungeonBigKey,
                "Dungeon Reward" => LocationPool.DungeonReward,
                "Goal" => LocationPool.Goal,
                _ => throw new InvalidLocationException($"Invalid location pool: {pool}")
            };
        }
    }
}
