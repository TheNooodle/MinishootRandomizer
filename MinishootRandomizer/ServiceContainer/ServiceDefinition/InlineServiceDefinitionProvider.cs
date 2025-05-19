using System;
using System.Collections.Generic;
using BepInEx.Logging;

namespace MinishootRandomizer;

/// <summary>
/// Provides service definitions defined inline using fluent API.
/// This is the primary class you want to modify to add new services.
/// </summary>
public class InlineServiceDefinitionProvider : IServiceDefinitionProvider
{
    private readonly List<ServiceDefinition> _definitions = new();
    private readonly List<PostBuildAction> _postBuildActions = new();
    private readonly ManualLogSource _pluginLogger;
    
    public InlineServiceDefinitionProvider(ManualLogSource pluginLogger)
    {
        _pluginLogger = pluginLogger;
        ConfigureServices();
    }
    
    public IEnumerable<ServiceDefinition> GetServiceDefinitions() => _definitions;
    public IEnumerable<PostBuildAction> GetPostBuildActions() => _postBuildActions;
    
    private void ConfigureServices()
    {
        // Register core logger first
        AddSingleton<ManualLogSource>(_pluginLogger);
        
        // Configure service groups
        ConfigureLogging();
        ConfigureGameEvents();
        ConfigureMessaging();
        ConfigureObjectFinding();
        ConfigureFactories();
        ConfigureRepositories();
        ConfigureCloning();
        ConfigureSprites();
        ConfigurePickupManagement();
        ConfigureRandomizer();
        ConfigureLogicSystem();
        ConfigureMessageHandlers();
        ConfigureUIServices();
        ConfigurePatchers();
    }
    
    private void ConfigureLogging()
    {
        AddSingleton<BepInExLogger>(sp => new BepInExLogger(sp.Get<ManualLogSource>()));
        AddSingleton<CappedLogger>(sp => new CappedLogger(sp.Get<BepInExLogger>()));
        AddSingleton<ILogger>(sp => sp.Get<CappedLogger>());
    }
    
    private void ConfigureGameEvents()
    {
        AddSingleton<GameEventDispatcher>(sp => new GameEventDispatcher(sp.Get<ILogger>()));
    }
    
    private void ConfigureMessaging()
    {
        AddSingleton<IEnvelopeStorage, InMemoryEnvelopeStorage>();
        AddPostBuildAction(sp => {
            var gameEvents = sp.Get<GameEventDispatcher>();
            var storage = sp.Get<IEnvelopeStorage>();
            gameEvents.ExitingGame += storage.Clear;
        });
        
        AddSingleton<IMessageProcessor, MessageProcessor>();
        
        AddSingleton<IMessageConsumer>(sp => new CoreMessageConsumer(
            sp.Get<IEnvelopeStorage>(),
            sp.Get<IMessageProcessor>(),
            sp.Get<ILogger>()
        ));
        
        AddSingleton<CoreMessageDispatcher>(sp => new CoreMessageDispatcher(
            sp.Get<IEnvelopeStorage>()
        ));
        
        AddSingleton<EventMessageDispatcher>(sp => new EventMessageDispatcher(
            sp.Get<CoreMessageDispatcher>()
        ));
        
        AddSingleton<IMessageDispatcher>(sp => sp.Get<EventMessageDispatcher>());
    }
    
    private void ConfigureObjectFinding()
    {
        AddSingleton<UnityObjectFinder>();
        
        // This service is not finished yet
        // AddSingleton<CacheableObjectFinder>(sp => new CacheableObjectFinder(
        //     sp.Get<UnityObjectFinder>()
        // ));
        
        AddSingleton<IObjectFinder>(sp => sp.Get<UnityObjectFinder>());
    }
    
    private void ConfigureFactories()
    {
        AddSingleton<ILocationFactory, DictionaryLocationFactory>();
        
        AddSingleton<IZoneFactory>(sp => new DictionaryZoneFactory(
            sp.Get<ILogger>()
        ));
        
        AddSingleton<IItemFactory>(sp => new DictionaryItemFactory(
            sp.Get<ILogger>()
        ));
    }
    
    private void ConfigureRepositories()
    {
        AddSingleton<CsvLocationRepository>(sp => new CsvLocationRepository(
            "MinishootRandomizer.Resources.locations.csv",
            sp.Get<ILocationFactory>(),
            sp.Get<ILogger>()
        ));
        
        AddSingleton<InMemoryLocationRepository>(sp => new InMemoryLocationRepository(
            sp.Get<CsvLocationRepository>()
        ));
        
        AddSingleton<ILocationRepository>(sp => sp.Get<InMemoryLocationRepository>());
        
        AddSingleton<IItemRepository>(sp => new CsvItemRepository(
            sp.Get<IItemFactory>(),
            "MinishootRandomizer.Resources.items.csv"
        ));
        
        AddSingleton<IZoneRepository>(sp => new CsvZoneRepository(
            "MinishootRandomizer.Resources.zones.csv",
            sp.Get<IZoneFactory>()
        ));
        
        AddSingleton<CsvRegionRepository>(sp => new CsvRegionRepository(
            "MinishootRandomizer.Resources.regions.csv"
        ));
        
        AddSingleton<InMemoryRegionRepository>(sp => new InMemoryRegionRepository(
            sp.Get<CsvRegionRepository>()
        ));
        
        AddSingleton<IRegionRepository>(sp => sp.Get<InMemoryRegionRepository>());
        
        AddSingleton<ITransitionRepository>(sp => new CsvTransitionRepository(
            "MinishootRandomizer.Resources.transitions.csv",
            sp.Get<ILogger>()
        ));
    }
    
    private void ConfigureCloning()
    {
        AddSingleton<ICloningPassChain, CloningPassChain>();
        
        AddPostBuildAction(sp => {
            var chain = sp.Get<ICloningPassChain>();
            chain.AddPass(new PickupPass());
            chain.AddPass(new ColliderPass());
            chain.AddPass(new ChildrenPass());
            chain.AddPass(new MetaCloningPass());
        });
        
        AddSingleton<CloneBasedFactory>(sp => new CloneBasedFactory(
            sp.Get<IObjectFinder>(),
            sp.Get<ICloningPassChain>(),
            sp.Get<ILogger>()
        ));
    }
    
    private void ConfigureSprites()
    {
        AddSingleton<ITranslator, NullTranslator>();
        
        AddSingleton<PrefabSpriteProvider>(sp => new PrefabSpriteProvider(
            sp.Get<IObjectFinder>(),
            sp.Get<ILogger>()
        ));
        
        AddPostBuildAction(sp => {
            var provider = sp.Get<PrefabSpriteProvider>();
            provider.AddStrategy(new RootExtractionStrategy());
            provider.AddStrategy(new SpriteChildExtractionStrategy());
            provider.AddStrategy(new TweenableExtractionStrategy());
        });
        
        AddSingleton<ISpriteProvider>(sp => new FileSpriteProvider(
            sp.Get<PrefabSpriteProvider>(),
            "MinishootRandomizer.Resources.images"
        ));
    }
    
    private void ConfigurePickupManagement()
    {
        AddSingleton<PickupManager>();
        
        AddPostBuildAction(sp => {
            var gameEvents = sp.Get<GameEventDispatcher>();
            var pickupManager = sp.Get<PickupManager>();
            
            gameEvents.LoadingSaveFile += pickupManager.OnLoadingSaveFile;
            gameEvents.NpcFreed += pickupManager.OnNpcFreed;
            gameEvents.PlayerStatsChanged += pickupManager.OnPlayerStatsChanged;
            gameEvents.EnteringEncounter += pickupManager.OnEnteringEncounter;
            gameEvents.ExitingEncounter += pickupManager.OnExitingEncounter;
        });
    }
    
    private void ConfigureRandomizer()
    {
        AddSingleton<IProgressionStorage, WorldStateProgressionStorage>();
        
        AddSingleton<IRandomizerContextProvider>(sp => new ImguiContextProvider(
            sp.Get<IObjectFinder>(),
            sp.Get<ILogger>()
        ));
        
        AddSingleton<IItemCounter, PlayerStateItemCounter>();
        
        AddSingleton<IArchipelagoClient>(sp => new MultiClient(
            sp.Get<IItemCounter>(),
            sp.Get<ILogger>()
        ));
        
        AddSingleton<ArchipelagoRandomizerEngine>(sp => new ArchipelagoRandomizerEngine(
            sp.Get<IArchipelagoClient>(),
            sp.Get<IItemRepository>(),
            sp.Get<ILocationRepository>(),
            sp.Get<IProgressionStorage>(),
            sp.Get<IMessageDispatcher>(),
            sp.Get<ILogger>()
        ));
        
        AddPostBuildAction(sp => {
            var client = sp.Get<IArchipelagoClient>();
            var engine = sp.Get<ArchipelagoRandomizerEngine>();
            
            if (client is MultiClient multiClient) {
                multiClient.ItemReceived += engine.OnItemReceived;
            }
        });
        
        AddSingleton<VanillaRandomizerEngine>();
        
        AddSingleton<DummyRandomizerEngine>(sp => new DummyRandomizerEngine(
            sp.Get<IItemRepository>(),
            sp.Get<ILocationRepository>(),
            sp.Get<IProgressionStorage>()
        ));
        
        AddSingleton<ContextualRandomizerEngine>(sp => new ContextualRandomizerEngine(
            sp.Get<ArchipelagoRandomizerEngine>(),
            sp.Get<VanillaRandomizerEngine>(),
            sp.Get<DummyRandomizerEngine>(),
            sp.Get<IRandomizerContextProvider>()
        ));
        
        AddSingleton<EventRandomizerEngine>(sp => new EventRandomizerEngine(
            sp.Get<ContextualRandomizerEngine>()
        ));
        
        AddSingleton<IRandomizerEngine>(sp => sp.Get<EventRandomizerEngine>());
        
        AddSingleton<CoreItemPresentationProvider>(sp => new CoreItemPresentationProvider(
            sp.Get<ISpriteProvider>(),
            sp.Get<IRandomizerEngine>()
        ));
        
        AddSingleton<TrapItemPresentationProvider>(sp => new TrapItemPresentationProvider(
            sp.Get<CoreItemPresentationProvider>(),
            sp.Get<IRandomizerEngine>(),
            sp.Get<IItemRepository>()
        ));
        
        AddSingleton<IItemPresentationProvider>(sp => sp.Get<TrapItemPresentationProvider>());
        
        AddSingleton<IGameObjectFactory>(sp => new SpriteBasedFactory(
            sp.Get<CloneBasedFactory>(),
            sp.Get<IItemPresentationProvider>(),
            sp.Get<ILogger>()
        ));
        
        AddSingleton<ILocationVisitor>(sp => new GameObjectCreationVisitor(
            sp.Get<IGameObjectFactory>(),
            sp.Get<IObjectFinder>(),
            sp.Get<PickupManager>(),
            sp.Get<ILogger>()
        ));
        
        AddSingleton<RandomizerEngineManager>(sp => new RandomizerEngineManager(
            sp.Get<IRandomizerEngine>()
        ));
        
        AddPostBuildAction(sp => {
            var gameEvents = sp.Get<GameEventDispatcher>();
            var manager = sp.Get<RandomizerEngineManager>();
            
            gameEvents.LoadingSaveFile += manager.OnLoadingSaveFile;
            gameEvents.ExitingGame += manager.OnExitingGame;
        });
    }
    
    private void ConfigureLogicSystem()
    {
        AddSingleton<CoreLogicParser>(sp => new CoreLogicParser(
            sp.Get<IItemRepository>(),
            sp.Get<IRegionRepository>()
        ));
        
        AddSingleton<ILogicParser>(sp => sp.Get<CoreLogicParser>());
        
        AddSingleton<LocalLogicStateProvider>(sp => new LocalLogicStateProvider(
            sp.Get<ILogicParser>(),
            sp.Get<IRegionRepository>(),
            sp.Get<ITransitionRepository>(),
            sp.Get<IItemRepository>(),
            sp.Get<IRandomizerEngine>(),
            new StandardCachePool<LogicState>(new DictionaryCacheStorage<LogicState>(), sp.Get<ILogger>()),
            sp.Get<ILogger>()
        ));
        
        AddPostBuildAction(sp => {
            var gameEvents = sp.Get<GameEventDispatcher>();
            var randomizerEngine = sp.Get<EventRandomizerEngine>();
            var logicStateProvider = sp.Get<LocalLogicStateProvider>();
            
            gameEvents.ExitingGame += logicStateProvider.OnExitingGame;
            gameEvents.ItemCollected += logicStateProvider.OnItemCollected;
            gameEvents.NpcFreed += logicStateProvider.OnNpcFreed;
            gameEvents.PlayerCurrencyChanged += logicStateProvider.OnPlayerCurrencyChanged;
            gameEvents.EnteringGameLocation += logicStateProvider.OnEnteringGameLocation;
            randomizerEngine.GoalCompleted += logicStateProvider.OnGoalCompleted;
        });
        
        AddSingleton<ILogicStateProvider>(sp => sp.Get<LocalLogicStateProvider>());
        
        AddSingleton<CoreLocationLogicChecker>(sp => new CoreLocationLogicChecker(
            sp.Get<ILogicStateProvider>(),
            sp.Get<ILogicParser>(),
            sp.Get<IRandomizerEngine>(),
            sp.Get<IRegionRepository>(),
            sp.Get<ILocationRepository>()
        ));
        
        AddSingleton<CachedLocationLogicChecker>(sp => new CachedLocationLogicChecker(
            sp.Get<CoreLocationLogicChecker>(),
            sp.Get<IMessageDispatcher>()
        ));
        
        AddPostBuildAction(sp => {
            var gameEvents = sp.Get<GameEventDispatcher>();
            var randomizerEngine = sp.Get<EventRandomizerEngine>();
            var logicChecker = sp.Get<CachedLocationLogicChecker>();
            
            gameEvents.ExitingGame += logicChecker.OnExitingGame;
            gameEvents.ItemCollected += logicChecker.OnItemCollected;
            gameEvents.NpcFreed += logicChecker.OnNpcFreed;
            gameEvents.PlayerCurrencyChanged += logicChecker.OnPlayerCurrencyChanged;
            gameEvents.EnteringGameLocation += logicChecker.OnEnteringGameLocation;
            randomizerEngine.GoalCompleted += logicChecker.OnGoalCompleted;
        });
        
        AddSingleton<ILocationLogicChecker>(sp => sp.Get<CachedLocationLogicChecker>());
    }
    
    private void ConfigureMessageHandlers()
    {
        AddSingleton<RefreshLogicCheckerCacheHandler>();
        AddSingleton<SendCheckedLocationsHandler>(sp => new SendCheckedLocationsHandler(
            sp.Get<IArchipelagoClient>()
        ));
        AddSingleton<SendGoalHandler>(sp => new SendGoalHandler(
            sp.Get<IArchipelagoClient>()
        ));
        AddSingleton<ReceiveItemHandler>(sp => new ReceiveItemHandler(
            sp.Get<GameEventDispatcher>()
        ));
        AddSingleton<ShowItemNotificationHandler>();
        AddSingleton<TriggerTrapDialogHandler>(sp => new TriggerTrapDialogHandler(
            new InMemoryTrapDialogProvider()
        ));
        
        AddPostBuildAction(sp => {
            var consumer = sp.Get<IMessageConsumer>() as CoreMessageConsumer;
            if (consumer != null) {
                consumer.AddHandler<RefreshLogicCheckerCacheMessage>(
                    sp.Get<RefreshLogicCheckerCacheHandler>());
                consumer.AddHandler<SendCheckedLocationsMessage>(
                    sp.Get<SendCheckedLocationsHandler>());
                consumer.AddHandler<SendGoalMessage>(
                    sp.Get<SendGoalHandler>());
                consumer.AddHandler<ReceiveItemMessage>(
                    sp.Get<ReceiveItemHandler>());
                consumer.AddHandler<ShowItemNotificationMessage>(
                    sp.Get<ShowItemNotificationHandler>());
                consumer.AddHandler<TriggerTrapDialogMessage>(
                    sp.Get<TriggerTrapDialogHandler>());
            }
        });
    }
    
    private void ConfigureUIServices()
    {
        AddSingleton<IPrefabCollector, ChainPrefabCollector>();
        
        AddPostBuildAction(sp => {
            var collector = sp.Get<IPrefabCollector>() as ChainPrefabCollector;
            var factory = sp.Get<CloneBasedFactory>() as IPrefabCollector;
            var spriteProvider = sp.Get<PrefabSpriteProvider>() as IPrefabCollector;
            
            if (collector != null) {
                if (factory != null) collector.AddCollector(factory);
                if (spriteProvider != null) collector.AddCollector(spriteProvider);
            }
        });
        
        AddSingleton<IMarkerDataProvider, InMemoryMarkerDataProvider>();
        
        AddSingleton<IMarkerFactory>(sp => new CoreMarkerFactory(
            sp.Get<IMarkerDataProvider>(),
            sp.Get<IObjectFinder>(),
            sp.Get<ILocationRepository>(),
            sp.Get<ILogger>()
        ));
        
        AddSingleton<INotificationObjectFactory>(sp => new CoreNotificationObjectFactory(
            sp.Get<IObjectFinder>()
        ));
        
        AddSingleton<NotificationManager>(sp => new NotificationManager(
            sp.Get<IObjectFinder>(),
            sp.Get<IMessageDispatcher>()
        ));
        
        AddPostBuildAction(sp => {
            var gameEvents = sp.Get<GameEventDispatcher>();
            var notificationManager = sp.Get<NotificationManager>();
            
            gameEvents.ItemCollected += notificationManager.OnItemCollected;
            gameEvents.ExitingGame += notificationManager.OnExitingGame;
        });
        
        AddSingleton<TrapManager>(sp => new TrapManager(
            sp.Get<IMessageDispatcher>()
        ));
        
        AddPostBuildAction(sp => {
            var gameEvents = sp.Get<GameEventDispatcher>();
            var trapManager = sp.Get<TrapManager>();
            
            gameEvents.ItemCollected += trapManager.OnItemCollected;
        });
    }
    
    private void ConfigurePatchers()
    {
        ConfigurePatcher<ShopReplacementPatcher>(sp => new ShopReplacementPatcher(
            sp.Get<IRandomizerEngine>(),
            sp.Get<IObjectFinder>(),
            sp.Get<ILogger>()
        ));
        
        ConfigurePatcher<ItemReplacementPatcher>(sp => new ItemReplacementPatcher(
            sp.Get<IRandomizerEngine>(),
            sp.Get<ILocationVisitor>(),
            sp.Get<IZoneRepository>(),
            sp.Get<IRegionRepository>(),
            sp.Get<ILocationRepository>(),
            sp.Get<ILogger>()
        ));
        
        ConfigurePatcher<XpCrystalRemovalPatcher>(sp => new XpCrystalRemovalPatcher(
            sp.Get<IRandomizerEngine>(),
            sp.Get<IObjectFinder>(),
            sp.Get<ILogger>()
        ));
        
        ConfigurePatcher<BlockedForestPatcher>(sp => new BlockedForestPatcher(
            sp.Get<IRandomizerEngine>(),
            sp.Get<IObjectFinder>(),
            sp.Get<IGameObjectFactory>(),
            sp.Get<ILogger>()
        ));
        
        ConfigurePatcher<SimpleTempleExitPatcher>(sp => new SimpleTempleExitPatcher(
            sp.Get<IRandomizerEngine>(),
            sp.Get<IObjectFinder>(),
            sp.Get<ILogger>()
        ));
        
        ConfigurePatcher<FamilyReunitedPatcher>(sp => new FamilyReunitedPatcher(
            sp.Get<IRandomizerEngine>(),
            sp.Get<IObjectFinder>(),
            sp.Get<ILogger>()
        ));
        
        ConfigurePatcher<BossPatcher>(sp => new BossPatcher(
            sp.Get<IRandomizerEngine>(),
            sp.Get<IObjectFinder>(),
            sp.Get<ILogger>()
        ));
        
        ConfigurePatcher<ScarabRemovalPatcher>(sp => new ScarabRemovalPatcher(
            sp.Get<IRandomizerEngine>(),
            sp.Get<IObjectFinder>(),
            sp.Get<ILogger>()
        ));
        
        ConfigurePatcher<GiveInitialItemsPatcher>(sp => new GiveInitialItemsPatcher(
            sp.Get<IRandomizerEngine>(),
            sp.Get<ILogger>()
        ));
        
        ConfigurePatcher<TrackerPatcher>(sp => new TrackerPatcher(
            sp.Get<IRandomizerEngine>(),
            sp.Get<IObjectFinder>(),
            sp.Get<IMarkerFactory>(),
            sp.Get<ILogger>()
        ), hasItemCollected: true);
        
        ConfigurePatcher<NotificationPatcher>(sp => new NotificationPatcher(
            sp.Get<IRandomizerEngine>(),
            sp.Get<INotificationObjectFactory>(),
            sp.Get<ILogger>()
        ));
        
        ConfigurePatcher<QualityOfLifePatcher>(sp => new QualityOfLifePatcher(
            sp.Get<IRandomizerEngine>(),
            sp.Get<IObjectFinder>(),
            sp.Get<ILogger>()
        ));
        
        ConfigurePatcher<DungeonRewardPatcher>(sp => new DungeonRewardPatcher(
            sp.Get<IRandomizerEngine>(),
            sp.Get<IObjectFinder>(),
            sp.Get<ILocationRepository>(),
            sp.Get<IItemRepository>(),
            sp.Get<ILogger>()
        ));
        
        ConfigurePatcher<DoorUnlockPatcher>(sp => new DoorUnlockPatcher(
            sp.Get<IRandomizerEngine>(),
            sp.Get<IObjectFinder>(),
            sp.Get<ILogger>()
        ));
    }
    
    private void ConfigurePatcher<T>(Func<IServiceContainer, T> factory, bool hasItemCollected = false) where T : class
    {
        AddSingleton<T>(factory);
        
        AddPostBuildAction(sp => {
            var gameEvents = sp.Get<GameEventDispatcher>();
            var patcher = sp.Get<T>();
            
            // Standard event method for all patchers
            if (patcher.GetType().GetMethod("OnEnteringGameLocation") != null) {
                gameEvents.EnteringGameLocation += (GameEventDispatcher.EnteringGameLocationHandler)Delegate.CreateDelegate(
                    typeof(GameEventDispatcher.EnteringGameLocationHandler), patcher, "OnEnteringGameLocation");
            }
            
            if (patcher.GetType().GetMethod("OnExitingGame") != null) {
                gameEvents.ExitingGame += (GameEventDispatcher.ExitingGameHandler)Delegate.CreateDelegate(
                    typeof(GameEventDispatcher.ExitingGameHandler), patcher, "OnExitingGame");
            }
            
            // Optional event for patchers that handle item collection
            if (hasItemCollected && patcher.GetType().GetMethod("OnItemCollected") != null) {
                gameEvents.ItemCollected += (GameEventDispatcher.ItemCollectedHandler)Delegate.CreateDelegate(
                    typeof(GameEventDispatcher.ItemCollectedHandler), patcher, "OnItemCollected");
            }
        });
    }
    
    #region Registration Methods
    public InlineServiceDefinitionProvider AddSingleton<TService, TImplementation>()
        where TService : class
        where TImplementation : class, TService
    {
        _definitions.Add(ServiceDefinition.FromType(typeof(TService), typeof(TImplementation)));
        return this;
    }
    
    public InlineServiceDefinitionProvider AddSingleton<TService>(Func<IServiceContainer, TService> factory)
        where TService : class
    {
        _definitions.Add(ServiceDefinition.FromFactory(typeof(TService), sp => factory(sp)));
        return this;
    }
    
    public InlineServiceDefinitionProvider AddSingleton<TService>(TService instance)
        where TService : class
    {
        _definitions.Add(ServiceDefinition.FromInstance(typeof(TService), instance));
        return this;
    }
    
    public InlineServiceDefinitionProvider AddSingleton<TService>()
        where TService : class
    {
        return AddSingleton<TService, TService>();
    }
    
    public InlineServiceDefinitionProvider AddPostBuildAction(Action<IServiceContainer> action)
    {
        _postBuildActions.Add(new PostBuildAction(action));
        return this;
    }
    #endregion
}
