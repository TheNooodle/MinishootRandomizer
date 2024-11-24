using System;
using System.Collections.Generic;

namespace MinishootRandomizer
{
    public class DummyRandomizerEngine : IRandomizerEngine
    {
        private IItemRepository _itemRepository;
        private ILocationRepository _locationRepository;
        private IProgressionStorage _progressionStorage;

        public DummyRandomizerEngine(
            IItemRepository itemRepository,
            ILocationRepository locationRepository,
            IProgressionStorage progressionStorage
        ) {
            _itemRepository = itemRepository;
            _locationRepository = locationRepository;
            _progressionStorage = progressionStorage;
        }

        public List<Location> GetRandomizedLocations()
        {
            List<Location> locations = new();
            if (_dummySpoilerLog == null)
            {
                GenerateSpoilerLog();
            }
            foreach (string locationIdentifier in _dummySpoilerLog.Keys)
            {
                locations.Add(_locationRepository.Get(locationIdentifier));
            }

            return locations;
        }

        public Item PeekLocation(Location location)
        {
            if (_dummySpoilerLog == null)
            {
                GenerateSpoilerLog();
            }

            if (!_dummySpoilerLog.ContainsKey(location.Identifier))
            {
                throw new System.Exception($"Location {location.Identifier} not found in dummy spoiler log!");
            }
            Item item = _itemRepository.Get(_dummySpoilerLog[location.Identifier]);

            return item;
        }

        public Item CheckLocation(Location location)
        {
            Item item = PeekLocation(location);
            _progressionStorage.SetLocationChecked(location);

            return item;
        }

        public bool IsLocationChecked(Location location)
        {
            return _progressionStorage.IsLocationChecked(location);
        }

        public T GetSetting<T>() where T : ISetting
        {
            Dictionary<Type, ISetting> settings = new()
            {
                { typeof(ShardSanity), new ShardSanity(true) },
                { typeof(NpcSanity), new NpcSanity(true) },
                { typeof(SimpleTempleExit), new SimpleTempleExit(true) },
                { typeof(BlockedForest), new BlockedForest(true) },
                { typeof(CompletionGoals), new CompletionGoals(Goals.Both) },
            };

            if (settings.TryGetValue(typeof(T), out ISetting setting))
            {
                return (T)setting;
            }

            throw new SettingNotSupported($"Setting {typeof(T).Name} is not supported by DummyRandomizerEngine!");
        }

        private void GenerateSpoilerLog()
        {
            _dummySpoilerLog = new();
            string[] lines = _rawSpoilerLog.Split("\n");
            foreach (string line in lines)
            {
                string[] parts = line.Split(": ");
                _dummySpoilerLog.Add(parts[0], parts[1]);
            }
        }

        public void CompleteGoal(Goals goal)
        {
            // no op
        }

        public bool IsRandomized()
        {
            return true;
        }

        public void SetContext(RandomizerContext context)
        {
            // no op
        }

        public void Initialize()
        {
            // no op
        }

        public void Dispose()
        {
            // no op
        }

        private Dictionary<string, string> _dummySpoilerLog = null;

        private string _rawSpoilerLog = @"Abyss - Ambush Island: XP Crystals x10
Abyss Church - Unchosen statue: XP Crystals x20
Abyss Ruined shop - Item: HP Crystal Shard
Abyss - Backroom item: XP Crystals x35
Abyss - Near dungeon entrance: XP Crystals x70
Abyss - South of spinning enemy: XP Crystals x15
Abyss - Near protected enemy: XP Crystals x10
Abyss - Village Entrance: XP Crystals x100
Abyss Shack - Hidden corridor: Lucky Heart
Abyss Shack - Hidden room: Boost
Abyss - Within Crystal Grove: XP Crystals x20
Abyss Tower - Top of tower: Progressive Cannon
Beach - Coast South hidden alcove: HP Crystal Shard
Beach - Coast Hidden by plants: XP Crystals x35
Beach - South East Island: XP Crystals x140
Beach - Protected item: XP Crystals x25
Beach - East Island: XP Crystals x65
Beach - Coast North hidden alcove: XP Crystals x20
Cemetery - Crying house: XP Crystals x5
Cemetery Tower - Top of tower: Ancient Tablet
Crystal Grove Temple - Dodge the east cannons: Enchanted Heart
Crystal Grove Temple - East tunnels: XP Crystals x10
Crystal Grove Temple - North east hidden room: Energy Crystal Shard
Crystal Grove Temple - Boss reward: XP Crystals x75
Crystal Grove Temple - South West Hidden pond: XP Crystals x50
Crystal Grove Tower - Top of tower: XP Crystals x20
Desert - North east platforms: HP Crystal Shard
Desert - Under ruins: Scarab Key
Desert - On the river: XP Crystals x20
Desert Grotto - Both torches lighted: XP Crystals x25
Desert Temple - Secret room: XP Crystals x5
Desert Temple - Boss reward: Energy Crystal Shard
Dungeon 1 - Central item: XP Crystals x30
Dungeon 1 - Debris wall item: XP Crystals x65
Dungeon 1 - North West Arena: XP Crystals x30
Dungeon 1 - Dungeon reward: Dungeon 1 Reward
Dungeon 1 - Near boss: Boss Key (Dungeon 1)
Dungeon 1 - Hidden below debris wall: Energy Crystal Shard
Dungeon 1 - Entrance after south ramp: Small Key (Dungeon 1)
Dungeon 1 - Entrance East Arena: Small Key (Dungeon 1)
Dungeon 1 - Near west spinning enemy: Sunken City Map
Dungeon 1 - Far West Arena after spinning enemy: Small Key (Dungeon 1)
Dungeon 1 - Hidden in West Arena: XP Crystals x20
Dungeon 1 - Entrance west bridge: XP Crystals x15
Dungeon 1 - West bridge hidden item: HP Crystal Shard
Dungeon 1 - South item: Vengeful Talisman
Dungeon 1 - East behind debris: XP Crystals x20
Dungeon 1 - Near east spinning enemy: Small Key (Dungeon 1)
Dungeon 2 - Walled arena item: Small Key (Dungeon 2)
Dungeon 2 - Walled arena extra: Small Key (Dungeon 2)
Dungeon 2 - Treasure room: HP Crystal Shard
Dungeon 2 - Treasure room entrance: Small Key (Dungeon 2)
Dungeon 2 - Central item: Ancient Tablet
Dungeon 2 - South west arena: Green Map
Dungeon 2 - Hidden by plants: Supershot
Dungeon 2 - North item: Boss Key (Dungeon 2)
Dungeon 2 - Dungeon reward: Dungeon 2 Reward
Dungeon 2 - North east beyond arena: XP Crystals x25
Dungeon 2 - Item after jumps: Family Parent 1
Dungeon 2 - West arena: XP Crystals x60
Dungeon 2 - West arena extra: Village Star
Dungeon 2 - North west arena: Small Key (Dungeon 2)
Dungeon 3 - Over the pit: XP Crystals x20
Dungeon 3 - East wall: Boss Key (Dungeon 3)
Dungeon 3 - East rock 1: Family Child
Dungeon 3 - Item protected by spikes: Small Key (Dungeon 3)
Dungeon 3 - South corridor: XP Crystals x25
Dungeon 3 - East Island: Blacksmith
Dungeon 3 - Dungeon reward: Dungeon 3 Reward
Dungeon 3 - Inside middle pot: Small Key (Dungeon 3)
Dungeon 3 - North arena: Small Key (Dungeon 3)
Dungeon 3 - Race on the water: Small Key (Dungeon 3)
Dungeon 3 - Behind North West doors: XP Crystals x85
Dungeon 3 - Hidden Tunnel: Small Key (Dungeon 3)
Dungeon 3 - Central Item: Healer
Dungeon 3 - South west of torches: XP Crystals x10
Dungeon 3 - South of torches: HP Crystal Shard
Dungeon 4 - Dungeon reward: Dungeon 4 Reward
Dungeon 5 - Central Item: XP Crystals x30
Family House Cave - Near tree: XP Crystals x10
Family House Cave - Near button: Progressive Cannon
Family House Cave - Reunited Family: XP Crystals x10
Family House Cave - Before shortcut: XP Crystals x20
Family House Cave - Sewers: Overcharge
Family House Cave - Hidden Tunnel: Dash
Forest - Enemy tree near Beach: Abyss Map
Forest - Secret within Secret: Bard
Forest - Secret pond behind bushes: XP Crystals x5
Forest - Hidden east tunnel: Power of time
Forest - Far South East: Ancient Tablet
Forest - Boss Reward: HP Crystal Shard
Forest - Tunnel below big tree enemy: HP Crystal Shard
Forest - Faraway island item: HP Crystal Shard
Forest - Faraway island extra: XP Crystals x15
Forest - Bush behind tree: XP Crystals x75
Forest Grotto - After ramp: XP Crystals x55
Forest Shop 1: XP Crystals x10
Forest Shop 2: XP Crystals x15
Forest Shop 3: Desert Map
Green - Grove under ruins: HP Crystal Shard
Green - Grove near button: XP Crystals x50
Green - River near Forest Entrance: XP Crystals x10
Green - Shortcut to Town Pillars: XP Crystals x40
Green - Island Arena Item: Crystal Bullet
Green - Outside Dungeon 1 Cave: HP Crystal Shard
Green - Forest Entrance: XP Crystals x10
Town - Blacksmith Item 1: XP Crystals x25
Town - Blacksmith Item 2: XP Crystals x25
Town - Blacksmith Item 3: XP Crystals x10
Town - Blacksmith Item 4: HP Crystal Shard
Town - Mercant Item 1: HP Crystal Shard
Town - Mercant Item 2: HP Crystal Shard
Town - Mercant Item 3: XP Crystals x10
Green - Closed Arena Item: HP Crystal Shard
Green - Behind Closed Arena: Power of spirits
Green - Crossroad Arena Item: Ancient Tablet
Green - Bridge Shortcut: Energy Crystal Shard
Green - Button Item: Beach Map
Town - Plaza: HP Crystal Shard
Town - Scarab Collector Item 2: HP Crystal Shard
Town - Scarab Collector Item 1: Energy Crystal Shard
Town - Scarab Collector Item 3: Blue Forest Map
Town - Scarab Collector Item 4: Explorer
Town - Scarab Collector Item 5: XP Crystals x15
Town - Scarab Collector Item 6: XP Crystals x20
Green - Hidden before Island Arena: Energy Crystal Shard
Town Pillars - Hidden Pond: HP Crystal Shard
Town Pillars - Hidden below bridge: XP Crystals x45
Green Grotto - Corner: Compass
Green Grotto - Before race: XP Crystals x65
Green Grotto - Drop: XP Crystals x15
Jar Shop 1: HP Crystal Shard
Jar Shop 2: Ancient Astrolabe
Jar Shop 3: XP Crystals x10
Junkyard - East pond: Energy Crystal Shard
Junkyard - South East: Mercant
Junkyard - Inside Sunken City: XP Crystals x15
Junkyard East Shack - Item: Energy Crystal Shard
Junkyard West Shack - Item: XP Crystals x70
Primordial Cave - Meet the Primordial Scarab: Idol of time
Scarab Temple - After race 1: XP Crystals x25
Scarab Temple - After race 2: Advanced Energy
Scarab Temple - After race 3: Wounded Heart
Scarab Temple - Backroom: Primordial Crystal
Scarab Temple - Bottom Left Torch Item: Dark Key
Scarab Temple - Central Item: HP Crystal Shard
Scarab Temple - Middle Entrance: HP Crystal Shard
Scarab Temple - East side: XP Crystals x5
Sewers - Central room corner: XP Crystals x60
Sewers - Central room boss reward: HP Crystal Shard
Sewers - Near Family House Cave: XP Crystals x5
Sewers - Behind West Entrance: Family Parent 2
Sewers - Drop: Swamp Map
Spirit Tower - Item: Scarab Collector
Starting Grotto - North Corridor: Idol of spirits
Starting Grotto - Secret Wall: XP Crystals x20
Starting Grotto - Entrance: XP Crystals x30
Starting Grotto - West Item: Progressive Cannon
Sunken City - North bridge: Ancient Tablet
Sunken City - West bridge: Progressive Cannon
Sunken City - Below West bridge: XP Crystals x35
Sunken City - Inside the walls: Junkyard Map
Sunken City - North East district: Progressive Cannon
Sunken City - South West building item: XP Crystals x10
Sunken City - Near West entrance: Power of protection
Sunken City Building - Drop: Idol of protection
Sunken Temple - Boss reward: XP Crystals x15
Sunken Temple - Entrance: XP Crystals x55
Sunken Temple - South West tunnel: HP Crystal Shard
Sunken Temple - Secret tunnel: Academician
Swamp - Hidden Before Big Enemy: HP Crystal Shard
Swamp - Near cracked wall: XP Crystals x15
Swamp - North item: HP Crystal Shard
Swamp - South West Island Hidden in trees Item: HP Crystal Shard
Swamp - Under rocks: Dark Heart
Swamp - Blocked tunnel: Restoration Enhancer
Swamp Jumps Grotto - Drop: HP Crystal Shard
Swamp Shop 1: Enchanted Powers
Swamp Shop 2: XP Crystals x5
Swamp Shop 3: Spirit Dash
Swamp Shop Extra: XP Crystals x20
Swamp Tower - Top of tower: XP Crystals x10
Town Pillars Grotto - Reward: Surf
Zelda 1 Grotto - Behind the closed doors: HP Crystal Shard";
    }
}
