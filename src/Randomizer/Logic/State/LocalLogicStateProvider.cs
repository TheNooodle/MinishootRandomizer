using System;
using System.Collections.Generic;

namespace MinishootRandomizer;

public class LocalLogicStateProvider : ILogicStateProvider
{
    private readonly ILogicParser _logicParser;
    private readonly IRegionRepository _regionRepository;
    private readonly ITransitionRepository _transitionRepository;
    private readonly IItemRepository _itemRepository;
    private readonly ISettingsProvider _settingsProvider;
    private readonly ICachePool<LogicState> _cachePool;
    private readonly ILogger _logger = new NullLogger();

    private Dictionary<string, int> _outOfLogicItemCounts = new Dictionary<string, int>()
    {
        {Item.ProgressiveCannon, 5},
        // We omit the D1 small keys because it's a special case
        {Item.D2SmallKey, 4},
        {Item.D3SmallKey, 5},
    };

    private Dictionary<Type, ISetting> _outOfLogicSettings = new Dictionary<Type, ISetting>()
    {
        {typeof(CannonLevelLogicalRequirements), new CannonLevelLogicalRequirements(false)},
        {typeof(BoostlessSpringboards), new BoostlessSpringboards(true)},
        {typeof(BoostlessSpiritRaces), new BoostlessSpiritRaces(true)},
        {typeof(BoostlessTorchRaces), new BoostlessTorchRaces(true)},
    };

    public LocalLogicStateProvider(
        ILogicParser logicParser,
        IRegionRepository regionRepository,
        ITransitionRepository transitionRepository,
        IItemRepository itemRepository,
        ISettingsProvider settingsProvider,
        ICachePool<LogicState> cachePool,
        ILogger logger = null
    ) {
        _logicParser = logicParser;
        _regionRepository = regionRepository;
        _transitionRepository = transitionRepository;
        _itemRepository = itemRepository;
        _settingsProvider = settingsProvider;
        _cachePool = cachePool;
        _logger = logger ?? new NullLogger();
    }

    public LogicState GetLogicState(LogicTolerance tolerance = LogicTolerance.Strict)
    {
        return _cachePool.Get(tolerance.Str(), () => {
            LogicState logicState = new LogicState();
            ItemsPass(logicState, tolerance);
            RegionsPass(logicState, tolerance);

            return new CacheItem<LogicState>(logicState);
        });
    }

    private void ItemsPass(LogicState logicState, LogicTolerance tolerance)
    {
        List<Item> items = _itemRepository.GetAll();
        Item dashItem = _itemRepository.Get(Item.Dash);
        foreach (Item item in items)
        {
            int count = item.GetOwnedQuantity();
            if (count > 0)
            {
                if (tolerance == LogicTolerance.Strict)
                {
                    logicState.SetItemCount(item, count);
                }
                else
                {
                    // Edge-case : when the player have the Dash, we must also consider the 2 consecutive doors at the south of D1.
                    // If the player only has 1 key, they can't open both doors. If they have 2 keys, they can open both.
                    // These doors require 4 key in logic, so we must set the key count to 4 if the player has the Dash.
                    if (item.Identifier == Item.D1SmallKey && count > 0)
                    {
                        if (dashItem.GetOwnedQuantity() < 1)
                        {
                            logicState.SetItemCount(item, 4);
                        }
                        else
                        {
                            logicState.SetItemCount(item, count == 1 ? 1 : 4);
                        }
                        continue;
                    }

                    // If the player have at least one of the item, we pretend they have all of them.
                    // This is done to consider certain logic rules that require multiple items, such as locked doors.
                    if (_outOfLogicItemCounts.ContainsKey(item.Identifier) && count > 0)
                    {
                        logicState.SetItemCount(item, _outOfLogicItemCounts[item.Identifier]);
                    }
                    else
                    {
                        logicState.SetItemCount(item, count);
                    }
                }
            }
        }
    }

    private void RegionsPass(LogicState logicState, LogicTolerance tolerance)
    {
        // We start from the starting region
        Region startRegion = _regionRepository.Get(Region.StartingGrottoLake);
        logicState.AddReachableRegion(startRegion);

        List<Transition> traversedTransitions = new List<Transition>();
        List<ISetting> settings = new List<ISetting>();
        List<ISetting> providedSettings = _settingsProvider.GetSettings();
        foreach (ISetting setting in providedSettings)
        {
            if (tolerance == LogicTolerance.Strict)
            {
                settings.Add(setting);
            }
            else
            {
                // We override the settings with the out-of-logic settings
                if (_outOfLogicSettings.ContainsKey(setting.GetType()))
                {
                    settings.Add(_outOfLogicSettings[setting.GetType()]);
                }
                else
                {
                    settings.Add(setting);
                }
            }
        }
        int previousReachableCount;
        int maxIterations = 1000; // Prevent infinite loops

        // We perform a breadth-first search to find all reachable regions
        // The loop will stop when no new regions are reachable
        // (this is because some transitions may require yet-to-be-reached regions)
        do
        {
            previousReachableCount = logicState.GetReachableRegions().Count;
            List<Region> newReachableRegions = new List<Region>();

            foreach (Region region in logicState.GetReachableRegions())
            {
                List<Transition> transitions = _transitionRepository.GetFromOriginRegion(region);
                foreach (Transition transition in transitions)
                {
                    if (traversedTransitions.Contains(transition))
                    {
                        continue;
                    }

                    LogicParsingResult parsingResult = _logicParser.ParseLogic(transition.LogicRule, logicState, settings);
                    if (parsingResult.Result)
                    {
                        traversedTransitions.Add(transition);
                        Region destinationRegion = _regionRepository.Get(transition.To);
                        newReachableRegions.Add(destinationRegion);
                    }
                }
            }

            foreach (Region region in newReachableRegions)
            {
                logicState.AddReachableRegion(region);
            }
        } while(previousReachableCount < logicState.GetReachableRegions().Count && maxIterations-- > 0);

        if (maxIterations <= 0)
        {
            _logger.LogError("Error: Infinite loop detected in region reachability calculation");
        }
    }

    public void OnItemCollected(Item item)
    {
        _cachePool.Clear();
    }

    public void OnNpcFreed()
    {
        _cachePool.Clear();
    }

    public void OnEnteringGameLocation(string locationName)
    {
        _cachePool.Clear();
    }

    public void OnPlayerCurrencyChanged(Currency currency)
    {
        if (currency == Currency.Scarab)
        {
            _cachePool.Clear();
        }
    }

    public void OnGoalCompleted(Goals goal)
    {
        _cachePool.Clear();
    }

    public void OnExitingGame()
    {
        _cachePool.Clear();
    }
}
