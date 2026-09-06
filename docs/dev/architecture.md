# Runtime architecture

This document is a first draft of the developer architecture documentation. It focuses on the five core concepts that connect the randomizer to the game: service composition, randomizer engines, logic, tracker markers, and messaging.

## High-level view

The project is a BepInEx plugin loaded by `Minishoot.exe`. The plugin creates the application services, applies Harmony patches, and creates a small set of persistent Unity components.

The runtime is split into three broad layers:

```text
Game and Unity API
        |
        |  Unity components and Harmony patches
        v
GameEventDispatcher / Messaging
        |
        v
Randomizer engines, logic, model and progression
```

The main startup sequence is implemented in `MinishootRandomizer/Plugin.cs`:

1. `Awake()` builds the service container.
2. `Start()` applies all Harmony patches.
3. `Start()` creates `SceneCrawlerComponent`, `RandomizerManagerComponent`, `MessageWorkerComponent` and `ImguiContextComponent`.
4. These objects are marked with `DontDestroyOnLoad`, so they remain available across scene changes.

`RandomizerManagerComponent` adapts game events such as save loading, location changes, stat changes and encounters to `GameEventDispatcher`. The rest of the application can subscribe to these events without directly depending on game classes such as `GameManager`, `Player` or `LocationManager`.

## ServiceContainer

### Purpose

`ServiceContainer` provides dependency injection and centralizes runtime composition. The abstraction is defined in `ServiceContainer/`, with the Microsoft implementation in `ServiceContainer/Microsoft/MicrosoftServiceContainer.cs`.

The composition root is `ServiceContainer/ServiceDefinition/InlineServiceDefinitionProvider.cs`. Its class summary is intentional: this is the primary place to register a new service.

### Build process

`Plugin.Awake()` creates an `InlineServiceDefinitionProvider`, passes it to `MicrosoftServiceContainer`, builds the provider, and retrieves the application logger. Service definitions are collected before the provider is built. Post-build actions are then used for wiring that requires already-created services.

The provider groups registrations by responsibility:

- logging;
- game events;
- messaging;
- Unity object finding and creation;
- model factories and CSV repositories;
- cloning and sprite providers;
- pickup management;
- randomizer engines and progression;
- logic checkers;
- message handlers;
- UI services;
- Harmony patchers, spirit logic and DeathLink.

Most services are registered as singletons. This is important for stateful services such as repositories, progression storage, message storage, the Archipelago client and randomizer engines.

### Post-build actions

Use a post-build action when registration alone is not enough. Current examples include:

- subscribing listeners to `GameEventDispatcher`;
- adding cloning passes to `ICloningPassChain`;
- adding sprite extraction strategies;
- registering message handlers;
- connecting cache invalidation handlers.

When adding a service, prefer an interface where the service has a meaningful boundary. Register the concrete type when it is also required directly by another service, then expose the interface as an alias when appropriate.

## RandomizerEngine

### Contract

`IRandomizerEngine` is the common contract for all randomization modes. It exposes:

- settings and location pools;
- randomized locations;
- peeking and checking a location;
- checked-location state;
- goal completion state;
- initialization and disposal;
- context assignment;
- whether the current game is randomized.

The contract allows the game integration and patchers to work with one engine without knowing whether the active mode is Dummy, Vanilla or Archipelago.

### Engine decorators and selection

The engine stack is built from three concepts:

```text
IRandomizerEngine
    |
    +-- EventRandomizerEngine
            |
            +-- ContextualRandomizerEngine
                    |
                    +-- DummyRandomizerEngine
                    +-- VanillaRandomizerEngine
                    +-- ArchipelagoRandomizerEngine
```

`ContextualRandomizerEngine` obtains a `RandomizerContext` from `IRandomizerContextProvider`, then delegates to the matching engine. It selects `ArchipelagoContext` for an Archipelago game and `VanillaContext` for a local game. The dummy engine is selected when `Plugin.UseDummyEngine` is enabled.

`EventRandomizerEngine` is a decorator that forwards the interface and raises `GoalCompleted` after the inner engine completes a goal. Other services can subscribe to this event without coupling themselves to a concrete engine.

### Archipelago flow

`ArchipelagoRandomizerEngine` owns the local randomizer view of an Archipelago session. It uses `IArchipelagoClient` for network operations and `IProgressionStorage` for local checked-location state.

During initialization it loads settings from Archipelago data storage, determines the active location pools, initializes location state, and synchronizes local and remote checked locations.

The main flows are:

```text
CheckLocation()
    -> PeekLocation()
    -> dispatch SendCheckedLocationsMessage
    -> persist local checked state
```

```text
Archipelago item received
    -> ArchipelagoRandomizerEngine.OnItemReceived()
    -> dispatch ReceiveItemMessage
    -> apply item when the game is in a valid state
```

Remote items and resynchronized items use `MustBeInGameStamp`, because applying them may require active Unity game state. DeathLink follows the same messaging boundary through `DeathLinkManager`.

## Logic

The logic subsystem determines whether regions and locations are reachable with the current inventory and settings. It is data-driven at the world level: regions, transitions and locations are loaded from CSV resources, while their rules are parsed at runtime.

### LogicState

`Randomizer/Logic/Model/LogicState.cs` contains the inventory counts and active settings used during evaluation. Items are stored by identifier and can be queried with a minimum count. Settings are stored by type and replaced when a setting of the same type is added.

`OutOfLogicStateDecorator` presents a relaxed view of the state for calculating locations that are reachable but not currently expected by the intended progression.

### Rule parsing

`CoreLogicParser` parses a rule as a set of `or` alternatives, each containing `and` conditions. Conditions can have an integer argument, for example `have_d1_keys(2)`. Rules are implemented in the parser's rule table and can call helper methods for fights, gaps, surfing, springboards, races and setting-dependent behavior.

The parser returns both the boolean result and the item names used by the rule. The latter can be used by presentation or debugging code to explain why a location is accessible.

An unknown rule raises `UnknownRuleException`. Therefore, changing a rule in a CSV file requires a corresponding parser rule unless the rule already exists.

### Accessibility calculation

`CoreRegionLogicChecker` starts at `Region.StartingGrottoLake` and crawls transitions until no new regions can be reached. It performs two passes:

1. Evaluate transitions with the actual `LogicState` and mark reachable regions as `InLogic`.
2. Evaluate remaining transitions with `OutOfLogicStateDecorator` and mark reachable regions as `OutOfLogic`.

All remaining regions are marked `Inaccessible`. A maximum iteration count protects the crawl from an accidental infinite loop.

`CoreLocationLogicChecker` first checks the location's region, then evaluates the location rule. A location inherits the region accessibility when its rule succeeds. If the normal rule fails, it is evaluated against the out-of-logic state.

Cached checkers and state providers exist to avoid recalculating the same results. Event handlers invalidate these caches when inventory, settings or world state changes.

### Testing logic

Logic tests are data-driven. `MinishootRandomizer.Tests/Randomizer/Logic/LogicTests.cs` loads the model repositories, parses `logic_tests.yaml`, and checks both region and location assertions. New logic rules should normally be accompanied by YAML cases covering the required inventory, settings and expected accessibility.

## Tracker

The tracker displays contextual markers in the game world. It is not the source of accessibility decisions; it consumes the randomizer engine and logic services.

### Marker model

Marker data is represented under `Randomizer/Tracker/`:

- `MarkerData` contains the serialized or configured marker information;
- `Model/Marker` contains marker behavior and visibility rules;
- `MarkerFactory` creates the appropriate marker type;
- `MarkerDataProvider` supplies marker data.

Marker variants include locations, objectives, NPCs, scarabs and spirits. Each category also has an out-of-logic variant so the UI can distinguish a currently expected location from one that is reachable only by taking an unintended route.

### Unity presentation

`Unity/Tracker/RandomizerTrackerMarkerComponent.cs` is the Unity-facing component. At startup it resolves the randomizer engine, location logic checker, logic state provider and sprite provider from the service container.

On every `Update()` it:

1. asks each configured marker to compute its visibility;
2. selects the first visible marker according to marker sort order;
3. loads the marker sprite and scale;
4. updates the floating animation;
5. shows or hides the sprite object.

The component deliberately keeps activation enabled. Its parent controls whether the marker object exists in the scene, while the component controls the visibility of the child sprite based on logic and randomizer state.

When adding a tracker marker, keep game-independent selection rules in the marker model and keep Unity-specific sprite and `GameObject` operations in the Unity component or its factory.

## Messaging

Messaging provides a deferred boundary between event producers and operations that must run later or in a specific game state. It is used for Archipelago communication, received items, notifications, traps and DeathLink.

### Message lifecycle

The core flow is:

```text
IMessageDispatcher.Dispatch()
    -> Envelope
    -> IEnvelopeStorage
    -> MessageWorkerComponent.Update()
    -> IMessageConsumer.Consume()
    -> IMessageProcessor
    -> IMessageHandler.Handle()
```

`CoreMessageDispatcher` wraps a message in an `Envelope` and stores it. `EventMessageDispatcher` adds dispatch lifecycle events around the core dispatcher. The default storage is `InMemoryEnvelopeStorage`, so messages are volatile and are cleared when the game exits.

### Envelopes and stamps

An envelope contains the message and optional `IStamp` instances. Stamps add processing constraints without changing the message type. Examples include:

- `MustBeInGameStamp`, which prevents handling outside active gameplay;
- `RetryStamp`, which records that a synchronous handler failed and should be attempted again;
- feature-specific stamps such as trap-dialog availability constraints.

Handlers should use stamps when processing depends on game state rather than implementing polling inside the handler.

### Consumption and processing

`CoreMessageConsumer` maps the concrete message type to an `IMessageHandler`. It checks every stamp before processing. Successful messages and messages without a registered handler are removed from storage. Failed synchronous messages remain available for a later attempt.

`MessageProcessor` runs `BackgroundThread` messages with `Task.Run`. Other messages run synchronously, which is required for Unity APIs and player-state mutations. Synchronous exceptions add or replace a `RetryStamp` and return a failed processing result.

`MessageWorkerComponent` is the Unity update-side pump. It periodically consumes messages and can trigger processing sooner after relevant game events. Message handlers should therefore be short, deterministic, and safe to retry where applicable.

### Adding a message

To add a message flow:

1. Define an `IMessage` implementation.
2. Define an `IMessageHandler` for the message.
3. Register the handler in `InlineServiceDefinitionProvider.ConfigureMessageHandlers()`.
4. Add stamps if the message has execution constraints.
5. Dispatch the message through `IMessageDispatcher`, not by accessing storage directly.
6. Add tests for successful processing and retry or state-gating behavior.

## Typical cross-component flow

Checking a randomized location illustrates the boundaries between the concepts:

```text
Unity object or Harmony patch
    -> IRandomizerEngine.CheckLocation(location)
    -> ArchipelagoRandomizerEngine
    -> IMessageDispatcher
    -> SendCheckedLocationsHandler
    -> IArchipelagoClient.CheckLocations()
```

Receiving the resulting item follows the reverse direction:

```text
Archipelago client callback
    -> ReceiveItemMessage
    -> MessageWorkerComponent
    -> ReceiveItemHandler
    -> player state update
    -> GameEventDispatcher.ItemCollected
    -> logic cache, tracker and UI listeners
```

This separation is the main architectural rule of the project: game-facing code detects or applies Unity state, while the engine, logic and messaging layers describe what should happen and when it is safe to happen.

## Related documentation

- [Player documentation](../players/index.md)
- [Game internals](../game/index.md)
- [Repository README](../../README.md)
