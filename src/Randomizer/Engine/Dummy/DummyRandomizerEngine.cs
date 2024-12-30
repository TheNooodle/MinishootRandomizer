using System;
using System.Collections.Generic;

namespace MinishootRandomizer
{
    public class DummyRandomizerEngine : IRandomizerEngine
    {
        private IItemRepository _itemRepository;
        private ILocationRepository _locationRepository;
        private IProgressionStorage _progressionStorage;

        private Dictionary<Type, ISetting> _settings = new()
        {
            { typeof(NpcSanity), new NpcSanity(true) },
            { typeof(ScarabSanity), new ScarabSanity(true) },
            { typeof(ShardSanity), new ShardSanity(true) },
            { typeof(KeySanity), new KeySanity(true) },
            { typeof(BossKeySanity), new BossKeySanity(true) },
            { typeof(SimpleTempleExit), new SimpleTempleExit(true) },
            { typeof(BlockedForest), new BlockedForest(true) },
            { typeof(CannonLevelLogicalRequirements), new CannonLevelLogicalRequirements(true) },
            { typeof(CompletionGoals), new CompletionGoals(Goals.Both) },
            { typeof(TrackerEnabled), new TrackerEnabled(true) }
        };

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
            if (_settings.TryGetValue(typeof(T), out ISetting setting))
            {
                return (T)setting;
            }

            throw new SettingNotSupported($"Setting {typeof(T).Name} is not supported by DummyRandomizerEngine!");
        }

        public List<ISetting> GetSettings()
        {
            return new List<ISetting>(_settings.Values);
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

        private string _rawSpoilerLog = @"Abyss - Ambush Island: HP Crystal Shard
Abyss Church - Unchosen statue: Progressive Cannon
Abyss - Backroom item: Desert Map
Abyss - Near dungeon entrance: Power of time
Abyss - South of spinning enemy: Small Key (Dungeon 1)
Abyss - Near protected enemy: XP Crystals x15
Abyss - Village Entrance: XP Crystals x35
Abyss Shack - Hidden corridor: HP Crystal Shard
Abyss Shack - Hidden room: Family Child
Abyss Shack - Under pot: HP Crystal Shard
Abyss - Within Crystal Grove: Energy Crystal Shard
Abyss North Connector - Under ruins: Advanced Energy
Abyss Tower - Top of tower: Green Map
Beach - Coast South hidden alcove: Sunken City Map
Beach - Coast Hidden by plants: XP Crystals x25
Beach - Coconut pile: HP Crystal Shard
Beach - South East Island: Small Key (Dungeon 3)
Beach - Protected item: HP Crystal Shard
Beach - East Island: Abyss Map
Beach - Seashell above dungeon: Junkyard Map
Beach - Coast North hidden alcove: XP Crystals x20
Cemetery - Crying house: XP Crystals x30
Cemetery - Under enemy: XP Crystals x10
Cemetery - West pot: Energy Crystal Shard
Cemetery Tower - Top of tower: HP Crystal Shard
Crystal Grove Temple - Dodge the east cannons: XP Crystals x10
Crystal Grove Temple - East tunnels: XP Crystals x15
Crystal Grove Temple - North east hidden room: Boss Key (Dungeon 1)
Crystal Grove Temple - Boss reward: Village Star
Crystal Grove Temple - South West Hidden pond: Energy Crystal Shard
Crystal Grove Temple - West pot: Scarab
Crystal Grove Tower - Top of tower: HP Crystal Shard
Desert - North east platforms: Small Key (Dungeon 3)
Desert - Under ruins: Power of protection
Desert - On the river: Idol of time
Desert Grotto - Both torches lighted: Small Key (Dungeon 2)
Desert Temple - Secret room: Small Key (Dungeon 2)
Desert Temple - Boss reward: Mercant
Desert Temple - North East pot: HP Crystal Shard
Dungeon 1 - Central item: HP Crystal Shard
Dungeon 1 - Platform after crystal wall: XP Crystals x10
Dungeon 1 - North West Arena: XP Crystals x10
Dungeon 1 - Dungeon reward: Dungeon 1 Reward
Dungeon 1 - Near boss: Ancient Tablet
Dungeon 1 - Hidden below crystal wall: Enchanted Heart
Dungeon 1 - Inside the crystal wall: XP Crystals x35
Dungeon 1 - Entrance after south ramp: Scarab
Dungeon 1 - Entrance East Arena: XP Crystals x20
Dungeon 1 - Near west armored spinner: XP Crystals x10
Dungeon 1 - Far West Arena after spinner: XP Crystals x10
Dungeon 1 - Hidden in West Arena: Idol of spirits
Dungeon 1 - Entrance west bridge: Scarab
Dungeon 1 - West bridge hidden item: Energy Crystal Shard
Dungeon 1 - South item: Ancient Tablet
Dungeon 1 - Crystal near east armored spinner: XP Crystals x50
Dungeon 1 - Near east armored spinner: Crystal Bullet
Dungeon 2 - Walled arena item: HP Crystal Shard
Dungeon 2 - Walled arena extra: Boost
Dungeon 2 - Treasure room: XP Crystals x25
Dungeon 2 - Treasure room entrance: Scarab
Dungeon 2 - Central item: Scarab
Dungeon 2 - South west arena: XP Crystals x15
Dungeon 2 - Hidden by plants: Progressive Cannon
Dungeon 2 - North item: Energy Crystal Shard
Dungeon 2 - Dungeon reward: Dungeon 2 Reward
Dungeon 2 - North east beyond arena: XP Crystals x10
Dungeon 2 - Item after jumps: HP Crystal Shard
Dungeon 2 - Secret room: Compass
Dungeon 2 - West arena: Scarab
Dungeon 2 - West arena extra: Academician
Dungeon 2 - North west arena: XP Crystals x10
Dungeon 3 - Over the pit: Supershot
Dungeon 3 - East wall: XP Crystals x30
Dungeon 3 - East rock 1: XP Crystals x55
Dungeon 3 - Item protected by spikes: Scarab
Dungeon 3 - South corridor: Scarab
Dungeon 3 - East rock 2: XP Crystals x10
Dungeon 3 - East Island: Small Key (Dungeon 2)
Dungeon 3 - Dungeon reward: Dungeon 3 Reward
Dungeon 3 - Inside middle pot: XP Crystals x5
Dungeon 3 - North arena: XP Crystals x30
Dungeon 3 - Race on the water: Beach Map
Dungeon 3 - Behind North West doors: XP Crystals x5
Dungeon 3 - Hidden Tunnel: Family Parent 2
Dungeon 3 - Central Item: Ancient Astrolabe
Dungeon 3 - South west of torches: HP Crystal Shard
Dungeon 3 - South of torches: Power of spirits
Dungeon 3 - Pot Island: Scarab
Dungeon 4 - Dungeon reward: Dungeon 4 Reward
Dungeon 5 - Central Item: HP Crystal Shard
Family House Cave - Near tree: Boss Key (Dungeon 2)
Family House Cave - Near button: HP Crystal Shard
Family House Cave - Reunited Family: Scarab
Family House Cave - Before shortcut: XP Crystals x15
Family House Cave - Sewers: Small Key (Dungeon 3)
Family House Cave - Hidden Tunnel: Idol of protection
Forest - Enemy tree near Beach: XP Crystals x10
Forest - Secret within Secret: XP Crystals x20
Forest - Secret pond behind bushes: Ancient Tablet
Forest - Secret pond bush: XP Crystals x5
Forest - Hidden east tunnel: Energy Crystal Shard
Forest - Far South East: HP Crystal Shard
Forest - Boss Reward: XP Crystals x35
Forest - Tunnel below big tree enemy: XP Crystals x10
Forest - Faraway island item: XP Crystals x10
Forest - Faraway island extra: XP Crystals x75
Forest - Pot: Small Key (Dungeon 1)
Forest - Bush behind tree: Surf
Forest Grotto - After ramp: Progressive Cannon
Forest Shop 1: XP Crystals x25
Forest Shop 2: HP Crystal Shard
Forest Shop 3: XP Crystals x15
Green - Grove under ruins: XP Crystals x20
Green - Grove near button: Scarab
Green - River near Forest Entrance: XP Crystals x25
Green - Shortcut to Town Pillars: Scarab
Green - Island Arena Item: Small Key (Dungeon 1)
Green - Outside Dungeon 1 Cave: Small Key (Dungeon 3)
Green - Forest Entrance: XP Crystals x65
Town - Blacksmith Item 1: HP Crystal Shard
Town - Blacksmith Item 2: XP Crystals x15
Town - Blacksmith Item 3: XP Crystals x60
Town - Blacksmith Item 4: Scarab
Town - Mercant Item 1: XP Crystals x100
Town - Mercant Item 2: HP Crystal Shard
Town - Mercant Item 3: Small Key (Dungeon 3)
Green - Closed Arena Item: Small Key (Dungeon 2)
Green - Behind Closed Arena: Overcharge
Green - Crossroad Arena Item: Spirit Dash
Green - Bridge Shortcut: Progressive Cannon
Green - Button Item: XP Crystals x25
Town - Plaza: HP Crystal Shard
Town - Scarab Collector Item 2: XP Crystals x5
Town - Scarab Collector Item 1: XP Crystals x70
Town - Scarab Collector Item 3: XP Crystals x40
Town - Scarab Collector Item 4: Scarab
Town - Scarab Collector Item 5: XP Crystals x20
Town - Scarab Collector Item 6: Restoration Enhancer
Green - Hidden before Island Arena: Swamp Map
Town Pillars - Hidden Pond: Small Key (Dungeon 1)
Town Pillars - Hidden below bridge: Scarab
Green Grotto - Corner: XP Crystals x60
Green Grotto - Before race: HP Crystal Shard
Green Grotto - Drop: Scarab
Jar Shop 1: Lucky Heart
Jar Shop 2: HP Crystal Shard
Jar Shop 3: Energy Crystal Shard
Junkyard - East pond: XP Crystals x20
Junkyard - South East: XP Crystals x10
Junkyard - Inside Sunken City: XP Crystals x75
Junkyard East Shack - Item: XP Crystals x55
Junkyard West Shack - Item: Bard
Primordial Cave - Meet the Primordial Scarab: XP Crystals x140
Abyss Ruined shop - Item: XP Crystals x5
Scarab Temple - After race 1: Family Parent 1
Scarab Temple - After race 2: Scarab Key
Scarab Temple - After race 3: HP Crystal Shard
Scarab Temple - Backroom: Ancient Tablet
Scarab Temple - Bottom Left Torch Item: XP Crystals x30
Scarab Temple - Central Item: Blue Forest Map
Scarab Temple - Middle Entrance: Primordial Crystal
Scarab Temple - East side: HP Crystal Shard
Sewers - Central room corner: HP Crystal Shard
Sewers - Central room boss reward: HP Crystal Shard
Sewers - Near Family House Cave: HP Crystal Shard
Sewers - Behind West Entrance: XP Crystals x85
Sewers - North pot room: XP Crystals x50
Sewers - South pot room: Boss Key (Dungeon 3)
Sewers - Drop: Healer
Spirit Tower - Item: Dark Key
Starting Grotto - North Corridor: XP Crystals x20
Starting Grotto - Secret Wall: XP Crystals x5
Starting Grotto - Entrance: HP Crystal Shard
Starting Grotto - West Item: Progressive Cannon
Sunken City - North West pot: Scarab
Sunken City - North bridge: XP Crystals x25
Sunken City - West bridge: Explorer
Sunken City - Below West bridge: XP Crystals x15
Sunken City - West bridge pot: Wounded Heart
Sunken City - Inside the walls: Scarab Collector
Sunken City - North East district: Energy Crystal Shard
Sunken City - South West building item: Dark Heart
Sunken City - Near West entrance: Ancient Tablet
Sunken City Building - Drop: XP Crystals x20
Sunken Temple - Boss reward: Enchanted Powers
Sunken Temple - Entrance: XP Crystals x15
Sunken Temple - South West tunnel: XP Crystals x20
Sunken Temple - Secret tunnel: XP Crystals x65
Sunken Temple - Under the lilypad: HP Crystal Shard
Swamp - Hidden Before Big Enemy: Dash
Swamp - Near cracked wall: XP Crystals x20
Swamp - North item: XP Crystals x70
Swamp - South West Island Hidden in trees Item: XP Crystals x20
Swamp - Under rocks: XP Crystals x15
Swamp - Blocked tunnel: Blacksmith
Swamp - Hidden in plant: XP Crystals x25
Swamp Jumps Grotto - Drop: Vengeful Talisman
Swamp Shop 1: XP Crystals x65
Swamp Shop 2: Scarab
Swamp Shop 3: XP Crystals x10
Swamp Shop Extra: XP Crystals x5
Swamp Tower - Top of tower: XP Crystals x45
Town Pillars Grotto - Reward: XP Crystals x20
Zelda 1 Grotto - Behind the closed doors: XP Crystals x20";
    }
}
