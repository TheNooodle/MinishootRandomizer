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
                { typeof(NpcSanity), new NpcSanity(true) },
                { typeof(ScarabSanity), new ScarabSanity(true) },
                { typeof(ShardSanity), new ShardSanity(true) },
                { typeof(KeySanity), new KeySanity(true) },
                { typeof(BossKeySanity), new BossKeySanity(true) },
                { typeof(SimpleTempleExit), new SimpleTempleExit(true) },
                { typeof(BlockedForest), new BlockedForest(true) },
                { typeof(CannonLevelLogicalRequirements), new CannonLevelLogicalRequirements(true) },
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

        private string _rawSpoilerLog = @"Abyss - Ambush Island: XP Crystals x15
Abyss Church - Unchosen statue: Family Parent 1
Abyss - Backroom item: HP Crystal Shard
Abyss - Near dungeon entrance: Ancient Tablet
Abyss - South of spinning enemy: Surf
Abyss - Near protected enemy: Small Key (Dungeon 3)
Abyss - Village Entrance: XP Crystals x25
Abyss Shack - Hidden corridor: XP Crystals x35
Abyss Shack - Hidden room: Small Key (Dungeon 3)
Abyss - Within Crystal Grove: Progressive Cannon
Abyss Tower - Top of tower: Progressive Cannon
Beach - Coast South hidden alcove: Ancient Tablet
Beach - Coast Hidden by plants: XP Crystals x25
Beach - South East Island: HP Crystal Shard
Beach - Protected item: Small Key (Dungeon 1)
Beach - East Island: XP Crystals x30
Beach - Coast North hidden alcove: Idol of spirits
Cemetery - Crying house: Ancient Tablet
Cemetery - Under enemy: Primordial Crystal
Cemetery Tower - Top of tower: XP Crystals x65
Crystal Grove Temple - Dodge the east cannons: XP Crystals x20
Crystal Grove Temple - East tunnels: XP Crystals x15
Crystal Grove Temple - North east hidden room: XP Crystals x50
Crystal Grove Temple - Boss reward: XP Crystals x5
Crystal Grove Temple - South West Hidden pond: XP Crystals x20
Crystal Grove Tower - Top of tower: XP Crystals x25
Desert - North east platforms: HP Crystal Shard
Desert - Under ruins: Small Key (Dungeon 1)
Desert - On the river: XP Crystals x30
Desert Grotto - Both torches lighted: Power of spirits
Desert Temple - Secret room: XP Crystals x5
Desert Temple - Boss reward: Healer
Dungeon 1 - Central item: XP Crystals x20
Dungeon 1 - Debris wall item: XP Crystals x5
Dungeon 1 - North West Arena: HP Crystal Shard
Dungeon 1 - Dungeon reward: Dungeon 1 Reward
Dungeon 1 - Near boss: XP Crystals x10
Dungeon 1 - Hidden below debris wall: Dark Heart
Dungeon 1 - Entrance after south ramp: XP Crystals x60
Dungeon 1 - Entrance East Arena: XP Crystals x65
Dungeon 1 - Near west spinning enemy: XP Crystals x25
Dungeon 1 - Far West Arena after spinning enemy: HP Crystal Shard
Dungeon 1 - Hidden in West Arena: XP Crystals x35
Dungeon 1 - Entrance west bridge: XP Crystals x50
Dungeon 1 - West bridge hidden item: XP Crystals x10
Dungeon 1 - South item: XP Crystals x15
Dungeon 1 - East behind debris: Beach Map
Dungeon 1 - Near east spinning enemy: Explorer
Dungeon 2 - Walled arena item: Wounded Heart
Dungeon 2 - Walled arena extra: XP Crystals x20
Dungeon 2 - Treasure room: Bard
Dungeon 2 - Treasure room entrance: Boss Key (Dungeon 3)
Dungeon 2 - Central item: XP Crystals x55
Dungeon 2 - South west arena: XP Crystals x20
Dungeon 2 - Hidden by plants: HP Crystal Shard
Dungeon 2 - North item: Enchanted Powers
Dungeon 2 - Dungeon reward: Dungeon 2 Reward
Dungeon 2 - North east beyond arena: XP Crystals x40
Dungeon 2 - Item after jumps: XP Crystals x15
Dungeon 2 - West arena: HP Crystal Shard
Dungeon 2 - West arena extra: HP Crystal Shard
Dungeon 2 - North west arena: XP Crystals x20
Dungeon 3 - Over the pit: HP Crystal Shard
Dungeon 3 - East wall: XP Crystals x15
Dungeon 3 - East rock 1: Blacksmith
Dungeon 3 - Item protected by spikes: HP Crystal Shard
Dungeon 3 - South corridor: HP Crystal Shard
Dungeon 3 - East Island: XP Crystals x10
Dungeon 3 - Dungeon reward: Dungeon 3 Reward
Dungeon 3 - Inside middle pot: XP Crystals x30
Dungeon 3 - North arena: Small Key (Dungeon 2)
Dungeon 3 - Race on the water: Lucky Heart
Dungeon 3 - Behind North West doors: Energy Crystal Shard
Dungeon 3 - Hidden Tunnel: XP Crystals x10
Dungeon 3 - Central Item: Energy Crystal Shard
Dungeon 3 - South west of torches: Progressive Cannon
Dungeon 3 - South of torches: Abyss Map
Dungeon 4 - Dungeon reward: Dungeon 4 Reward
Dungeon 5 - Central Item: XP Crystals x20
Family House Cave - Near tree: XP Crystals x5
Family House Cave - Near button: Family Parent 2
Family House Cave - Reunited Family: Energy Crystal Shard
Family House Cave - Before shortcut: Energy Crystal Shard
Family House Cave - Sewers: XP Crystals x35
Family House Cave - Hidden Tunnel: Desert Map
Forest - Enemy tree near Beach: Scarab Collector
Forest - Secret within Secret: HP Crystal Shard
Forest - Secret pond behind bushes: XP Crystals x20
Forest - Hidden east tunnel: HP Crystal Shard
Forest - Far South East: XP Crystals x15
Forest - Boss Reward: Vengeful Talisman
Forest - Tunnel below big tree enemy: XP Crystals x60
Forest - Faraway island item: HP Crystal Shard
Forest - Faraway island extra: Energy Crystal Shard
Forest - Bush behind tree: Dash
Forest Grotto - After ramp: Academician
Forest Shop 1: Crystal Bullet
Forest Shop 2: XP Crystals x30
Forest Shop 3: XP Crystals x85
Green - Grove under ruins: Small Key (Dungeon 3)
Green - Grove near button: XP Crystals x45
Green - River near Forest Entrance: XP Crystals x70
Green - Shortcut to Town Pillars: XP Crystals x5
Green - Island Arena Item: Progressive Cannon
Green - Outside Dungeon 1 Cave: XP Crystals x10
Green - Forest Entrance: XP Crystals x75
Town - Blacksmith Item 1: Mercant
Town - Blacksmith Item 2: XP Crystals x20
Town - Blacksmith Item 3: Energy Crystal Shard
Town - Blacksmith Item 4: Boss Key (Dungeon 2)
Town - Mercant Item 1: Restoration Enhancer
Town - Mercant Item 2: XP Crystals x10
Town - Mercant Item 3: HP Crystal Shard
Green - Closed Arena Item: XP Crystals x10
Green - Behind Closed Arena: HP Crystal Shard
Green - Crossroad Arena Item: Ancient Tablet
Green - Bridge Shortcut: Boost
Green - Button Item: Power of protection
Town - Plaza: XP Crystals x10
Town - Scarab Collector Item 2: XP Crystals x10
Town - Scarab Collector Item 1: HP Crystal Shard
Town - Scarab Collector Item 3: Advanced Energy
Town - Scarab Collector Item 4: XP Crystals x15
Town - Scarab Collector Item 5: Compass
Town - Scarab Collector Item 6: XP Crystals x20
Green - Hidden before Island Arena: XP Crystals x10
Town Pillars - Hidden Pond: XP Crystals x10
Town Pillars - Hidden below bridge: XP Crystals x70
Green Grotto - Corner: HP Crystal Shard
Green Grotto - Before race: XP Crystals x15
Green Grotto - Drop: Boss Key (Dungeon 1)
Jar Shop 1: Family Child
Jar Shop 2: Small Key (Dungeon 3)
Jar Shop 3: Blue Forest Map
Junkyard - East pond: Green Map
Junkyard - South East: Overcharge
Junkyard - Inside Sunken City: Village Star
Junkyard East Shack - Item: XP Crystals x25
Junkyard West Shack - Item: XP Crystals x10
Primordial Cave - Meet the Primordial Scarab: HP Crystal Shard
Abyss Ruined shop - Item: XP Crystals x25
Scarab Temple - After race 1: Scarab Key
Scarab Temple - After race 2: Idol of protection
Scarab Temple - After race 3: Small Key (Dungeon 2)
Scarab Temple - Backroom: HP Crystal Shard
Scarab Temple - Bottom Left Torch Item: Ancient Tablet
Scarab Temple - Central Item: Energy Crystal Shard
Scarab Temple - Middle Entrance: Supershot
Scarab Temple - East side: HP Crystal Shard
Sewers - Central room corner: XP Crystals x20
Sewers - Central room boss reward: HP Crystal Shard
Sewers - Near Family House Cave: XP Crystals x55
Sewers - Behind West Entrance: XP Crystals x10
Sewers - Drop: XP Crystals x25
Spirit Tower - Item: Spirit Dash
Starting Grotto - North Corridor: XP Crystals x65
Starting Grotto - Secret Wall: XP Crystals x65
Starting Grotto - Entrance: Small Key (Dungeon 1)
Starting Grotto - West Item: Progressive Cannon
Sunken City - North bridge: XP Crystals x15
Sunken City - West bridge: HP Crystal Shard
Sunken City - Below West bridge: XP Crystals x20
Sunken City - Inside the walls: HP Crystal Shard
Sunken City - North East district: Idol of time
Sunken City - South West building item: Small Key (Dungeon 2)
Sunken City - Near West entrance: XP Crystals x75
Sunken City Building - Drop: XP Crystals x140
Sunken Temple - Boss reward: Small Key (Dungeon 3)
Sunken Temple - Entrance: Dark Key
Sunken Temple - South West tunnel: Power of time
Sunken Temple - Secret tunnel: HP Crystal Shard
Swamp - Hidden Before Big Enemy: XP Crystals x20
Swamp - Near cracked wall: Swamp Map
Swamp - North item: HP Crystal Shard
Swamp - South West Island Hidden in trees Item: Small Key (Dungeon 2)
Swamp - Under rocks: HP Crystal Shard
Swamp - Blocked tunnel: Junkyard Map
Swamp Jumps Grotto - Drop: Energy Crystal Shard
Swamp Shop 1: Ancient Astrolabe
Swamp Shop 2: Enchanted Heart
Swamp Shop 3: XP Crystals x10
Swamp Shop Extra: HP Crystal Shard
Swamp Tower - Top of tower: XP Crystals x5
Town Pillars Grotto - Reward: Sunken City Map
Abyss North Connector - Under debris: XP Crystals x65
Abyss Shack - Under pot: XP Crystals x65
Beach - Coconut pile: XP Crystals x65
Beach - Seashell above dungeon: XP Crystals x65
Cemetery - West pot: XP Crystals x65
Crystal Grove Temple - West pot: XP Crystals x65
Desert Temple - North East pot: XP Crystals x65
Dungeon 2 - Secret room: XP Crystals x65
Dungeon 3 - East rock 2: XP Crystals x65
Dungeon 3 - Pot Island: XP Crystals x65
Forest - Pot: XP Crystals x65
Forest - Secret pond bush: XP Crystals x65
Sewers - North pot room: XP Crystals x65
Sewers - South pot room: XP Crystals x65
Sunken City - North West pot: XP Crystals x65
Sunken City - West bridge pot: XP Crystals x65
Sunken Temple - Under the lilypad: XP Crystals x65
Swamp - Hidden in plant: XP Crystals x65
Zelda 1 Grotto - Behind the closed doors: Small Key (Dungeon 1)";
    }
}
