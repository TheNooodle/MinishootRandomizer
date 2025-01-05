using System;
using System.Collections.Generic;

namespace MinishootRandomizer;

public class ArchipelagoRandomizerEngine : IRandomizerEngine
{
    private readonly IArchipelagoClient _client;
    private readonly IItemRepository _itemRepository;
    private readonly ILocationRepository _locationRepository;
    private readonly IProgressionStorage _progressionStorage;
    private readonly IMessageDispatcher _messageDispatcher;
    private readonly ILogger _logger = new NullLogger();

    private ArchipelagoContext _context;
    private Dictionary<Type, ISetting> _settings = new();
    private List<LocationPool> _locationPools = new();
    private List<Location> _locations = new();
    private List<Location> _checkedLocations = new();
    private Dictionary<long, ArchipelagoItem> _archipelagoItems = new();

    public ArchipelagoRandomizerEngine(
        IArchipelagoClient client,
        IItemRepository itemRepository,
        ILocationRepository locationRepository,
        IProgressionStorage progressionStorage,
        IMessageDispatcher messageDispatcher,
        ILogger logger = null
    )
    {
        _client = client;
        _itemRepository = itemRepository;
        _locationRepository = locationRepository;
        _progressionStorage = progressionStorage;
        _messageDispatcher = messageDispatcher;
        _logger = logger ?? new NullLogger();
    }

    public Item CheckLocation(Location location)
    {
        Item item = PeekLocation(location);
        List<Location> locations = new() { location };
        _messageDispatcher.Dispatch(new SendCheckedLocationsMessage(locations));
        _checkedLocations.Add(location);
        _progressionStorage.SetLocationChecked(location);

        return item;
    }

    public List<Location> GetRandomizedLocations()
    {
        return _locations;
    }

    public List<ISetting> GetSettings()
    {
        return new List<ISetting>(_settings.Values);
    }

    public T GetSetting<T>() where T : ISetting
    {
        if (_settings.TryGetValue(typeof(T), out ISetting setting))
        {
            return (T)setting;
        }

        throw new SettingNotSupported($"Setting {typeof(T).Name} is not supported by ArchipelagoRandomizerEngine!");
    }

    public bool IsLocationChecked(Location location)
    {
        return _checkedLocations.Contains(location);
    }

    public Item PeekLocation(Location location)
    {
        ArchipelagoItemData itemData = _client.GetItemData(location.Identifier);
        if (itemData.IsLocal)
        {
            return _itemRepository.Get(itemData.ItemName);
        }
        else
        {
            if (!_archipelagoItems.TryGetValue(itemData.ItemId, out ArchipelagoItem archipelagoItem))
            {
                archipelagoItem = new ArchipelagoItem(
                    itemData.SlotName + "'s " + itemData.ItemName,
                    MapArchipelagoItemCategory(itemData.Category)
                );
                _archipelagoItems.Add(itemData.ItemId, archipelagoItem);
            }

            return archipelagoItem;
        }
    }

    private ItemCategory MapArchipelagoItemCategory(ArchipelagoItemCategory category)
    {
        return category switch
        {
            ArchipelagoItemCategory.Filler => ItemCategory.Filler,
            ArchipelagoItemCategory.Progression => ItemCategory.Progression,
            ArchipelagoItemCategory.Useful => ItemCategory.Helpful,
            ArchipelagoItemCategory.Trap => ItemCategory.Trap,
            ArchipelagoItemCategory.SkipBalancing => ItemCategory.Filler,
            ArchipelagoItemCategory.ProgressionSkipBalancing => ItemCategory.Token,
            _ => throw new ArgumentOutOfRangeException(nameof(category), category, null),
        };
    }

    private void LocalToRemoteSync()
    {
        List<string> remoteCheckedLocationNames = _client.GetCheckedLocationNames();
        List<Location> allLocations = _locationRepository.GetAll();
        List<Location> locationsToSend = new();

        foreach (Location location in allLocations)
        {
            if (_progressionStorage.IsLocationChecked(location) && !remoteCheckedLocationNames.Contains(location.Identifier))
            {
                _checkedLocations.Add(location);
                locationsToSend.Add(location);
            }
        }

        if (locationsToSend.Count > 0)
        {
            _messageDispatcher.Dispatch(new SendCheckedLocationsMessage(locationsToSend));
        }
    }

    private void RemoteToLocalSync()
    {
        List<string> remoteCheckedLocationNames = _client.GetCheckedLocationNames();
        List<Item> itemsToReceive = new();
        foreach (string locationName in remoteCheckedLocationNames)
        {
            try
            {
                Location location = _locationRepository.Get(locationName);
                if (!_progressionStorage.IsLocationChecked(location))
                {
                    _checkedLocations.Add(location);
                    _progressionStorage.SetLocationChecked(location);
                    itemsToReceive.Add(PeekLocation(location));
                }
            }
            catch (LocationNotFoundException e)
            {
                _logger.LogWarning(e.Message);
            }
        }

        foreach (Item item in itemsToReceive)
        {
            _messageDispatcher.Dispatch(new ReceiveItemMessage(item), new List<IStamp> {
                new MustBeInGameStamp()
            });
        }        
    }

    public void OnItemReceived(ArchipelagoItemData itemData)
    {
        _logger.LogInfo($"Received item {itemData.ItemName} at {itemData.SlotName}");
        Item item = _itemRepository.Get(itemData.ItemName);
        _messageDispatcher.Dispatch(new ReceiveItemMessage(item), new List<IStamp> {
            new MustBeInGameStamp()
        });
    }

    private void InitializeSettings()
    {
        _settings = new()
        {
            { typeof(NpcSanity), new NpcSanity(GetBooleanSettingValue("npc_sanity")) },
            { typeof(ScarabSanity), new ScarabSanity(GetBooleanSettingValue("scarab_sanity")) },
            { typeof(ShardSanity), new ShardSanity(GetBooleanSettingValue("shard_sanity")) },
            { typeof(SpiritSanity), new SpiritSanity(false) },
            { typeof(KeySanity), new KeySanity(GetBooleanSettingValue("key_sanity")) },
            { typeof(BossKeySanity), new BossKeySanity(GetBooleanSettingValue("boss_key_sanity")) },
            { typeof(SimpleTempleExit), new SimpleTempleExit(GetBooleanSettingValue("simple_temple_exit")) },
            { typeof(BlockedForest), new BlockedForest(GetBooleanSettingValue("blocked_forest")) },
            { typeof(CannonLevelLogicalRequirements), new CannonLevelLogicalRequirements(GetBooleanSettingValue("cannon_level_logical_requirements")) },
            { typeof(CompletionGoals), new CompletionGoals(GetGoalsSettingValue()) },
        };
    }

    private bool GetBooleanSettingValue(string dataStorageKey, bool defaultValue = false)
    {
        try
        {
            object value = _client.GetDataStorageValue(dataStorageKey);

            return (long)value == 1;
        }
        catch (ArchipelagoLogicException e)
        {
            _logger.LogWarning(e.Message);

            return defaultValue;
        }
    }

    private Goals GetGoalsSettingValue(Goals defaultValue = Goals.Both)
    {
        try
        {
            object value = _client.GetDataStorageValue("completion_goals");

            switch ((long)value)
            {
                case 0:
                    return Goals.Dungeon5;
                case 1:
                    return Goals.Snow;
                case 2:
                    return Goals.Both;
                default:
                    return defaultValue;
            }
        }
        catch (ArchipelagoLogicException e)
        {
            _logger.LogWarning(e.Message);

            return defaultValue;
        }
    }

    private void InitializeLocations()
    {
        List<string> locationNames = _client.GetAllLocationNames();
        foreach (string locationName in locationNames)
        {
            try
            {
                Location location = _locationRepository.Get(locationName);
                if (_locationPools.Contains(location.Pool))
                {
                    _locations.Add(location);
                }
            }
            catch (InvalidLocationException e)
            {
                _logger.LogWarning(e.Message);
            }
            catch (LocationNotFoundException e)
            {
                _logger.LogWarning(e.Message);
            }
        }

        List<string> checkedLocationNames = _client.GetCheckedLocationNames();
        foreach (string locationName in checkedLocationNames)
        {
            try
            {
                Location location = _locationRepository.Get(locationName);
                if (_locationPools.Contains(location.Pool))
                {
                    _checkedLocations.Add(location);
                }
            }
            catch (InvalidLocationException e)
            {
                _logger.LogWarning(e.Message);
            }
            catch (LocationNotFoundException e)
            {
                _logger.LogWarning(e.Message);
            }
        }
    }

    private void InitializePools()
    {
        _locationPools = new()
        {
            LocationPool.Default,
            LocationPool.DungeonSmallKey,
            LocationPool.DungeonBigKey,
            LocationPool.DungeonReward,
            LocationPool.Goal
        };

        if (GetSetting<NpcSanity>().Enabled)
        {
            _locationPools.Add(LocationPool.Npc);
        }
        if (GetSetting<ScarabSanity>().Enabled)
        {
            _locationPools.Add(LocationPool.Scarab);
        }
        if (GetSetting<ShardSanity>().Enabled)
        {
            _locationPools.Add(LocationPool.XpCrystals);
        }
        if (GetSetting<SpiritSanity>().Enabled)
        {
            _locationPools.Add(LocationPool.Spirit);
        }
    }

    public List<LocationPool> GetLocationPools()
    {
        return _locationPools;
    }

    public void CompleteGoal(Goals goal)
    {
        CompletionGoals completionGoals = GetSetting<CompletionGoals>();
        bool sendCompletion = 
            (completionGoals.Goal == Goals.Snow || _progressionStorage.IsGoalCompleted(Goals.Dungeon5)) &&
            (completionGoals.Goal == Goals.Dungeon5 || _progressionStorage.IsGoalCompleted(Goals.Snow));

        SendGoalMessage message = new(goal, sendCompletion);
        _messageDispatcher.Dispatch(message);
    }

    public bool IsRandomized()
    {
        return true;
    }

    public void SetContext(RandomizerContext context)
    {
        if (context is ArchipelagoContext archipelagoContext)
        {
            _context = archipelagoContext;
        }
        else
        {
            throw new ArgumentException("ContextualRandomizerEngine does not support the provided context.");
        }
    }

    public void Initialize()
    {
        if (_context == null)
        {
            throw new ArgumentException("ArchipelagoRandomizerEngine has not been initialized.");
        }

        try
        {
            _client.ReceiveItems();
            InitializeSettings();
            InitializePools();
            InitializeLocations();
            LocalToRemoteSync();
            RemoteToLocalSync();
        }
        catch (ArchipelagoLoginException e)
        {
            _logger.LogError(e.Message);
        }
    }

    public void Dispose()
    {
        _locationPools.Clear();
        _locations.Clear();
        _checkedLocations.Clear();
        _archipelagoItems.Clear();
    }
}
