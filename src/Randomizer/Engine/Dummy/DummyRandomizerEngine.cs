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

        private string _rawSpoilerLog = @"Abyss - Ambush Island: Scarab
Abyss Church - Unchosen statue: Scarab
Abyss - Backroom item: HP Crystal Shard
Abyss - Near dungeon entrance: Abyss Map
Abyss - South of spinning enemy: XP Crystals x5
Abyss - Near protected enemy: XP Crystals x15
Abyss - Village Entrance: Compass
Abyss Shack - Hidden corridor: Scarab
Abyss Shack - Hidden room: XP Crystals x20
Abyss Shack - Under pot: XP Crystals x15
Abyss - Within Crystal Grove: XP Crystals x15
Abyss North Connector - Under ruins: Surf
Abyss Tower - Top of tower: Small Key (Dungeon 3)
Beach - Coast South hidden alcove: HP Crystal Shard
Beach - Coast Hidden by plants: Boost
Beach - Coconut pile: HP Crystal Shard
Beach - South East Island: Scarab
Beach - Protected item: XP Crystals x45
Beach - East Island: Scarab
Beach - Seashell above dungeon: XP Crystals x65
Beach - Coast North hidden alcove: Lucky Heart
Cemetery - Crying house: HP Crystal Shard
Cemetery - Under enemy: Small Key (Dungeon 2)
Cemetery - West pot: Idol of protection
Cemetery Tower - Top of tower: Academician
Crystal Grove Temple - Dodge the east cannons: Scarab
Crystal Grove Temple - East tunnels: XP Crystals x10
Crystal Grove Temple - North east hidden room: HP Crystal Shard
Crystal Grove Temple - Boss reward: Small Key (Dungeon 3)
Crystal Grove Temple - South West Hidden pond: Progressive Cannon
Crystal Grove Temple - West pot: Explorer
Crystal Grove Tower - Top of tower: Idol of time
Desert - North east platforms: Healer
Desert - Under ruins: HP Crystal Shard
Desert - On the river: Progressive Cannon
Desert Grotto - Both torches lighted: Scarab
Desert Temple - Secret room: HP Crystal Shard
Desert Temple - Boss reward: Scarab
Desert Temple - North East pot: XP Crystals x10
Dungeon 1 - Central item: Supershot
Dungeon 1 - Platform after crystal wall: Beach Map
Dungeon 1 - North West Arena: XP Crystals x20
Dungeon 1 - Dungeon reward: Dungeon 1 Reward
Dungeon 1 - Near boss: XP Crystals x25
Dungeon 1 - Hidden below crystal wall: HP Crystal Shard
Dungeon 1 - Inside the crystal wall: HP Crystal Shard
Dungeon 1 - Entrance after south ramp: XP Crystals x10
Dungeon 1 - Entrance East Arena: XP Crystals x40
Dungeon 1 - Near west armored spinner: Small Key (Dungeon 1)
Dungeon 1 - Far West Arena after spinner: HP Crystal Shard
Dungeon 1 - Hidden in West Arena: Energy Crystal Shard
Dungeon 1 - Entrance west bridge: XP Crystals x10
Dungeon 1 - West bridge hidden item: HP Crystal Shard
Dungeon 1 - South item: XP Crystals x85
Dungeon 1 - Crystal near east armored spinner: XP Crystals x10
Dungeon 1 - Near east armored spinner: XP Crystals x10
Dungeon 2 - Walled arena item: Ancient Tablet
Dungeon 2 - Walled arena extra: XP Crystals x60
Dungeon 2 - Treasure room: Ancient Tablet
Dungeon 2 - Treasure room entrance: Small Key (Dungeon 2)
Dungeon 2 - Central item: XP Crystals x70
Dungeon 2 - South west arena: Ancient Tablet
Dungeon 2 - Hidden by plants: HP Crystal Shard
Dungeon 2 - North item: XP Crystals x20
Dungeon 2 - Dungeon reward: Dungeon 2 Reward
Dungeon 2 - North east beyond arena: HP Crystal Shard
Dungeon 2 - Item after jumps: Enchanted Heart
Dungeon 2 - Secret room: Scarab
Dungeon 2 - West arena: Scarab
Dungeon 2 - West arena extra: Ancient Tablet
Dungeon 2 - North west arena: XP Crystals x75
Dungeon 3 - Over the pit: XP Crystals x20
Dungeon 3 - East wall: Energy Crystal Shard
Dungeon 3 - East rock 1: Restoration Enhancer
Dungeon 3 - Item protected by spikes: HP Crystal Shard
Dungeon 3 - South corridor: XP Crystals x25
Dungeon 3 - East rock 2: Progressive Cannon
Dungeon 3 - East Island: XP Crystals x25
Dungeon 3 - Dungeon reward: Dungeon 3 Reward
Dungeon 3 - Inside middle pot: XP Crystals x50
Dungeon 3 - North arena: Scarab Collector
Dungeon 3 - Race on the water: HP Crystal Shard
Dungeon 3 - Behind North West doors: Power of spirits
Dungeon 3 - Hidden Tunnel: XP Crystals x10
Dungeon 3 - Central Item: HP Crystal Shard
Dungeon 3 - South west of torches: XP Crystals x25
Dungeon 3 - South of torches: XP Crystals x5
Dungeon 3 - Pot Island: Blacksmith
Dungeon 4 - Dungeon reward: Dungeon 4 Reward
Dungeon 5 - Central Item: XP Crystals x50
Family House Cave - Near tree: XP Crystals x20
Family House Cave - Near button: Energy Crystal Shard
Family House Cave - Reunited Family: XP Crystals x20
Family House Cave - Before shortcut: HP Crystal Shard
Family House Cave - Sewers: XP Crystals x55
Family House Cave - Hidden Tunnel: Advanced Energy
Forest - Enemy tree near Beach: XP Crystals x5
Forest - Secret within Secret: Scarab
Forest - Secret pond behind bushes: XP Crystals x35
Forest - Secret pond bush: XP Crystals x20
Forest - Hidden east tunnel: Energy Crystal Shard
Forest - Far South East: Boss Key (Dungeon 2)
Forest - Boss Reward: HP Crystal Shard
Forest - Tunnel below big tree enemy: XP Crystals x60
Forest - Faraway island item: Family Parent 2
Forest - Faraway island extra: XP Crystals x20
Forest - Pot: XP Crystals x10
Forest - Bush behind tree: XP Crystals x5
Forest Grotto - After ramp: HP Crystal Shard
Forest Shop 1: Scarab
Forest Shop 2: XP Crystals x55
Forest Shop 3: Scarab
Green - Grove under ruins: Small Key (Dungeon 1)
Green - Grove near button: Dark Key
Green - River near Forest Entrance: Scarab
Green - Shortcut to Town Pillars: HP Crystal Shard
Green - Island Arena Item: Small Key (Dungeon 1)
Green - Outside Dungeon 1 Cave: HP Crystal Shard
Green - Forest Entrance: Energy Crystal Shard
Town - Blacksmith Item 1: XP Crystals x10
Town - Blacksmith Item 2: Small Key (Dungeon 3)
Town - Blacksmith Item 3: XP Crystals x10
Town - Blacksmith Item 4: Small Key (Dungeon 3)
Town - Mercant Item 1: Desert Map
Town - Mercant Item 2: XP Crystals x30
Town - Mercant Item 3: XP Crystals x20
Green - Closed Arena Item: Progressive Cannon
Green - Behind Closed Arena: Ancient Astrolabe
Green - Crossroad Arena Item: XP Crystals x65
Green - Bridge Shortcut: XP Crystals x5
Green - Button Item: XP Crystals x15
Town - Plaza: Energy Crystal Shard
Town - Scarab Collector Item 2: XP Crystals x5
Town - Scarab Collector Item 1: XP Crystals x15
Town - Scarab Collector Item 3: XP Crystals x100
Town - Scarab Collector Item 4: XP Crystals x20
Town - Scarab Collector Item 5: Primordial Crystal
Town - Scarab Collector Item 6: Vengeful Talisman
Green - Hidden before Island Arena: XP Crystals x10
Town Pillars - Hidden Pond: XP Crystals x30
Town Pillars - Hidden below bridge: Ancient Tablet
Green Grotto - Corner: XP Crystals x10
Green Grotto - Before race: XP Crystals x30
Green Grotto - Drop: Small Key (Dungeon 1)
Jar Shop 1: XP Crystals x20
Jar Shop 2: Family Child
Jar Shop 3: XP Crystals x10
Junkyard - East pond: Crystal Bullet
Junkyard - South East: Junkyard Map
Junkyard - Inside Sunken City: XP Crystals x35
Junkyard East Shack - Item: Scarab
Junkyard West Shack - Item: HP Crystal Shard
Primordial Cave - Meet the Primordial Scarab: Enchanted Powers
Abyss Ruined shop - Item: Overcharge
Scarab Temple - After race 1: XP Crystals x20
Scarab Temple - After race 2: XP Crystals x10
Scarab Temple - After race 3: XP Crystals x15
Scarab Temple - Backroom: Sunken City Map
Scarab Temple - Bottom Left Torch Item: HP Crystal Shard
Scarab Temple - Central Item: Energy Crystal Shard
Scarab Temple - Middle Entrance: Dash
Scarab Temple - East side: Spirit Dash
Sewers - Central room corner: HP Crystal Shard
Sewers - Central room boss reward: XP Crystals x35
Sewers - Near Family House Cave: XP Crystals x30
Sewers - Behind West Entrance: Swamp Map
Sewers - North pot room: Scarab
Sewers - South pot room: Small Key (Dungeon 2)
Sewers - Drop: Boss Key (Dungeon 3)
Spirit Tower - Item: Scarab
Starting Grotto - North Corridor: XP Crystals x65
Starting Grotto - Secret Wall: Small Key (Dungeon 3)
Starting Grotto - Entrance: XP Crystals x25
Starting Grotto - West Item: Progressive Cannon
Sunken City - North West pot: XP Crystals x15
Sunken City - North bridge: XP Crystals x75
Sunken City - West bridge: Village Star
Sunken City - Below West bridge: Bard
Sunken City - West bridge pot: Scarab
Sunken City - Inside the walls: HP Crystal Shard
Sunken City - North East district: Power of protection
Sunken City - South West building item: Idol of spirits
Sunken City - Near West entrance: Scarab Key
Sunken City Building - Drop: XP Crystals x10
Sunken Temple - Boss reward: XP Crystals x20
Sunken Temple - Entrance: Energy Crystal Shard
Sunken Temple - South West tunnel: Blue Forest Map
Sunken Temple - Secret tunnel: Mercant
Sunken Temple - Under the lilypad: Boss Key (Dungeon 1)
Swamp - Hidden Before Big Enemy: HP Crystal Shard
Swamp - Near cracked wall: Wounded Heart
Swamp - North item: Family Parent 1
Swamp - South West Island Hidden in trees Item: XP Crystals x15
Swamp - Under rocks: XP Crystals x140
Swamp - Blocked tunnel: XP Crystals x15
Swamp - Hidden in plant: Dark Heart
Swamp Jumps Grotto - Drop: Green Map
Swamp Shop 1: XP Crystals x70
Swamp Shop 2: Small Key (Dungeon 2)
Swamp Shop 3: HP Crystal Shard
Swamp Shop Extra: HP Crystal Shard
Swamp Tower - Top of tower: XP Crystals x5
Town Pillars Grotto - Reward: XP Crystals x25
Zelda 1 Grotto - Behind the closed doors: Power of time";
    }
}
