using System.Collections.Generic;

namespace MinishootRandomizer
{
    public class DictionaryZoneFactory : IZoneFactory
    {
        private readonly ILogger _logger;

        private static readonly Dictionary<string, string> _zoneToGameLocationName = new()
        {
            {"Abyss Village Shack", "Cave"},
            {"Abyss Church", "Cave"},
            {"Abyss Church backroom", "Cave"},
            {"Abyss Connector", "Cave"},
            {"Abyss Race", "CaveExtra"},
            {"Abyss Ruined shop", "Cave"},
            {"Abyss Tower", "Tower"},
            {"Beach Race", "CaveExtra"},
            {"Cemetery Tower", "Tower"},
            {"Crystal Grove Temple", "Temple1"},
            {"Crystal Grove Tower", "Tower"},
            {"Desert Grotto", "Cave"},
            {"Desert Race", "CaveExtra"},
            {"Desert Temple", "Temple2"},
            {"Dungeon 1", "Dungeon1"},
            {"Dungeon 1 Cave", "Cave"},
            {"Dungeon 2", "Dungeon2"},
            {"Dungeon 3", "Dungeon3"},
            {"Dungeon 4", "Dungeon4"},
            {"Dungeon 5", "Dungeon5"},
            {"Family House Cave", "Cave"},
            {"Forest Grotto", "Cave"},
            {"Forest Shop", "Cave"},
            {"Forest Shop Race", "CaveExtra"},
            {"Forest Waterways", "Cave"},
            {"Green Grotto", "Cave"},
            {"Inside the tree", "Snow"},
            {"Jar Shop", "Cave"},
            {"Junkyard East Shack", "Cave"},
            {"Junkyard Waterways", "Cave"},
            {"Junkyard West Shack", "Cave"},
            {"Overworld", "Overworld"},
            {"Primordial Cave", "Cave"},
            {"Primordial Cave Entrance", "Cave"},
            {"Scarab Temple", "Cave"},
            {"Sewers", "Cave"},
            {"Snow", "Snow"},
            {"Spirit Tower", "Tower"},
            {"Starting Grotto", "Cave"},
            {"Sunken City Building", "Tower"},
            {"Sunken City Hall", "Tower"},
            {"Sunken City Race", "CaveExtra"},
            {"Sunken Temple", "Temple3"},
            {"Swamp Jumps Grotto", "CaveExtra"},
            {"Swamp Race", "CaveExtra"},
            {"Swamp Shop", "Cave"},
            {"Swamp Tower", "Tower"},
            {"Town Pillars Grotto", "Cave"},
            {"Zelda 1 Grotto", "Cave"},
        };

        public DictionaryZoneFactory(ILogger logger = null)
        {
            _logger = logger ?? new NullLogger();
        }

        public Zone Create(string name, string regionsString)
        {
            Zone zone = new(name);
            foreach (string regionName in regionsString.Split(','))
            {
                zone.AddRegionName(regionName);
            }
            if (_zoneToGameLocationName.ContainsKey(name))
            {
                zone.GameLocationName = _zoneToGameLocationName[name];
            } else {
                _logger.LogWarning($"No game location name found for zone {name}");
                zone.GameLocationName = null;
            }
            return zone;
        }
    }
}
