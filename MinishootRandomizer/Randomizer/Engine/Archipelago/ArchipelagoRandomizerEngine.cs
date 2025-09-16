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
            return new ArchipelagoItem(
                itemData.ItemName,
                MapArchipelagoItemCategory(itemData.Category),
                itemData.SlotName
            );
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
            else if (remoteCheckedLocationNames.Contains(location.Identifier) && !_progressionStorage.IsLocationChecked(location))
            {
                // If the remote server considers the location checked, but we don't, we need to check it locally.
                // This can happen if a another game used the "collect" command to get all items from other games, including ours.
                _checkedLocations.Add(location);
                _progressionStorage.SetLocationChecked(location);
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
        ReceivedItem item = ReceivedItem.Create(
            _itemRepository.Get(itemData.ItemName),
            itemData.SlotName
        );
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
            { typeof(SpiritSanity), new SpiritSanity(GetBooleanSettingValue("spirit_sanity")) },
            { typeof(KeySanity), new KeySanity(GetBooleanSettingValue("key_sanity")) },
            { typeof(BossKeySanity), new BossKeySanity(GetBooleanSettingValue("boss_key_sanity")) },
            { typeof(TrapItemsAppearance), new TrapItemsAppearance(GetEnumSettingValue("trap_items_appearance", TrapItemsAppearanceValue.Anything)) },
            { typeof(ShowArchipelagoItemCategory), new ShowArchipelagoItemCategory(GetBooleanSettingValue("show_archipelago_item_category")) },
            { typeof(ShopCostModifier), new ShopCostModifier(GetNumericSettingValue("shop_cost_modifier", 100)) },
            { typeof(ScarabItemsCost), new ScarabItemsCost(GetNumericSettingValue("scarab_items_cost", 3)) },
            { typeof(SpiritTowerRequirement), new SpiritTowerRequirement(GetNumericSettingValue("spirit_tower_requirement", 8)) },
            { typeof(BlockedForest), new BlockedForest(GetBooleanSettingValue("blocked_forest")) },
            { typeof(IgnoreCannonLevelRequirements), new IgnoreCannonLevelRequirements(GetBooleanSettingValue("ignore_cannon_level_requirements")) },
            { typeof(BoostlessSpringboards), new BoostlessSpringboards(GetBooleanSettingValue("boostless_springboards")) },
            { typeof(BoostlessSpiritRaces), new BoostlessSpiritRaces(GetBooleanSettingValue("boostless_spirit_races")) },
            { typeof(BoostlessTorchRaces), new BoostlessTorchRaces(GetBooleanSettingValue("boostless_torch_races")) },
            { typeof(EnablePrimordialCrystalLogic), new EnablePrimordialCrystalLogic(GetBooleanSettingValue("enable_primordial_crystal_logic")) },
            { typeof(DashlessGaps), new DashlessGaps(GetEnumSettingValue("dashless_gaps", DashlessGapsValue.NeedsDash)) },
            { typeof(CompletionGoals), new CompletionGoals(GetEnumSettingValue("completion_goals", Goals.Dungeon5)) },
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

    private T GetEnumSettingValue<T>(string dataStorageKey, T defaultValue) where T : Enum
    {
        try
        {
            object value = _client.GetDataStorageValue(dataStorageKey);
            long enumValue = (long)value;
            if (Enum.IsDefined(typeof(T), (int)enumValue))
            {
                return (T)Enum.ToObject(typeof(T), (int)enumValue);
            }
            else
            {
                throw new ArgumentOutOfRangeException(
                    nameof(dataStorageKey),
                    $"Value {(int)enumValue} is not defined in enum {typeof(T).Name}"
                );
            }
        }
        catch (ArchipelagoLogicException e)
        {
            _logger.LogWarning(e.Message);

            return defaultValue;
        }
            
    }

    private int GetNumericSettingValue(string dataStorageKey, int defaultValue)
    {
        try
        {
            object value = _client.GetDataStorageValue(dataStorageKey);

            return (int)(long)value;
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
        CompletionGoals completionGoalSetting = GetSetting<CompletionGoals>();
        bool sendCompletion;
        switch (completionGoalSetting.Goal)
        {
            case Goals.Dungeon5:
                sendCompletion = goal == Goals.Dungeon5 && _progressionStorage.IsGoalCompleted(Goals.Dungeon5);
                break;
            case Goals.Snow:
                sendCompletion = goal == Goals.Snow && _progressionStorage.IsGoalCompleted(Goals.Snow);
                break;
            case Goals.Dungeon5AndSnow:
                sendCompletion = (goal == Goals.Dungeon5 || goal == Goals.Snow) && _progressionStorage.IsGoalCompleted(Goals.Dungeon5) && _progressionStorage.IsGoalCompleted(Goals.Snow);
                break;
            case Goals.SpiritTower:
                sendCompletion = goal == Goals.SpiritTower && _progressionStorage.IsGoalCompleted(Goals.SpiritTower);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        SendGoalMessage message = new(goal, sendCompletion);
        _messageDispatcher.Dispatch(message);
    }

    public bool IsGoalCompleted(Goals goal)
    {
        return _progressionStorage.IsGoalCompleted(goal);
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
    }
}
