# Runtime architecture

This document is a first draft of the developer architecture documentation.

## High-level view

The project is a BepInEx plugin loaded by `Minishoot.exe`. The plugin creates the application services, applies Harmony patches, and creates a small set of persistent Unity components.

The runtime is split into three broad layers:

```text
Game and Unity API
        |
        v
GameEventDispatcher / Harmony patching
        |
        v
Randomizer engines, logic, model and progression
```

The main startup sequence is implemented in `MinishootRandomizer/Plugin.cs`:

1. `Awake()` builds the service container.
2. `Start()` applies all Harmony patches.
3. `Start()` creates `SceneCrawlerComponent`, `RandomizerManagerComponent`, `MessageWorkerComponent` and `ImguiContextComponent`.
4. These objects are marked with `DontDestroyOnLoad`, so they remain available across scene changes.

`RandomizerManagerComponent` adapts game events such as save loading, scene changes, stat changes and encounters to `GameEventDispatcher`. The rest of the application can subscribe to these events without directly depending on game classes.

To apply changes to the game, the two main ways to do so are via Events, or Harmony patching :

* Adding new GameObjects and reacting to C# Actions from the game code is better done via the Patcher system (DI, events, Unity API...)
* Modifying an existing feature of the game is better done through Harmony patchers.

## ServiceContainer

### Purpose

`ServiceContainer` provides dependency injection and centralizes runtime composition. The abstraction is defined in `ServiceContainer/`, with a custom implementation in `ServiceContainer/InMemory/InMemoryServiceContainer.cs`.

> Past versions were using the Microsoft implementations of a service container. It has been since retired.

The composition root is `ServiceContainer/ServiceDefinition/InlineServiceDefinitionProvider.cs`. This is where all services are registered. If you have to add a new service or post build action, this is where you must look.

### Build process

`Plugin.Awake()` creates an `InlineServiceDefinitionProvider`, passes it to `InMemoryServiceContainer`, and builds the provider. Service definitions are collected before the provider is built. Post-build actions are then used for wiring that requires already-created services.

The provider groups registrations by responsibility:

- logging;
- game events;
- messaging;
- and many more...

Most services are registered as singletons. This is important for stateful services such as repositories, progression storage, message storage, the Archipelago client and randomizer engines.

### Post-build actions

Use a post-build action when registration alone is not enough. Current examples include:

- subscribing listeners to `GameEventDispatcher`;
- adding cloning passes to `ICloningPassChain`;
- adding sprite extraction strategies;
- registering message handlers;
- connecting cache invalidation handlers.

When adding a service, prefer an interface where the service has a meaningful boundary. Register the concrete type when it is also required directly by another service, then expose the interface as an alias when appropriate.

>Interfaces allows for better code decoupling : changing implementations, adding decorators for events or logging, easing unit testing...

## RandomizerEngine

### Contract

`IRandomizerEngine` is the common contract for all randomization modes. It exposes the contract that decouples the way a seed a generated, the features of the randomizer engine, and the mod itself which handles item replacement and other game related features.

> One concrete example is the DummyRandomizerEngine, which allows a developer to test features in the game without booting an Archipelago server or generating a seed. This contract is also made with a future Standalone version in mind.

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

`ContextualRandomizerEngine` obtains a `RandomizerContext` from `IRandomizerContextProvider`, then delegates to the matching engine. Currently :

* The Dummy engine is selected when Plugin.UseDummyEngine is true. Use this to test features on a local, fixed seed.
* The Archipelago engine is selected when a connection is made to an Archipelago server. The next save file loaded will be handled by the AP Client.
* The Vanilla engine is selected as a fallback, disabling all randomizer features to play the vanilla game.

`EventRandomizerEngine` is a decorator that emit events to various services to react to Randomizer specific events. Other services can subscribe to this event without coupling themselves to a concrete engine.

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

Most of Archipelago related features (sending/receiving items, deathlink, connection...) are handled via the Messaging system.

## Messaging

Messaging provides a deferred boundary between event producers and operations that must run later or in a specific game state. It is mainly used for Archipelago.

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

Messages can be processed on two different queues. The desired queue for a message is defined through `IMessage.MessageQueue`. Those are :

* MainThread, to have messages being handled within gameplay. Please note that message handles on the main thread have direct impact on the gameplay. Some actions (such as interacting with the Unity API) are main thread only.
* BackgroundThread, which runs on the `System.Threading.Tasks` system. When possible, prefer using this queue.

Synchronous exceptions add or replace a `RetryStamp` and return a failed processing result.

`MessageWorkerComponent` is the Unity update-side pump. It periodically consumes messages and can trigger processing sooner after relevant game events. Message handlers should therefore be short, deterministic, and safe to retry where applicable.

### Adding a message

To add a message flow:

1. Define an `IMessage` implementation.
2. Define an `IMessageHandler` for the message.
3. Register the handler in `InlineServiceDefinitionProvider.ConfigureMessageHandlers()`.
4. Add stamps if the message has execution constraints.
5. Dispatch the message through `IMessageDispatcher`, not by accessing storage directly.


## Unity object finding

### Purpose

`IObjectFinder` (`Unity/Finder/IObjectFinder.cs`) abstracts GameObject lookup in the Unity scene. Patchers and services use it instead of calling `GameObject.FindObjectsOfType` directly, so that selection criteria are declarative, testable, and cacheable.

### Contract

```csharp
GameObject FindObject(ISelector selector);   // first match, throws ObjectNotFoundException if none
GameObject[] FindObjects(ISelector selector); // all matches, empty array if none
```

`FindObject` throws `ObjectNotFoundException` when nothing matches; use `FindObjects` when an empty result is a valid outcome.

### Selectors

A selector (`Unity/Selector/`) describes *how* to pick objects. All selectors expose `Type` (the component type to look for) and `IncludeInactive` (whether inactive GameObjects are captured) where relevant:

| Selector | Matches |
| --- | --- |
| `ByComponent` | All GameObjects having a component of the given `Type`. |
| `ByName` | GameObjects by name, optionally restricted to a component `Type`. |
| `ByProximity` | GameObjects with a component of `Type` within `Radius` of a `Position`. |
| `ByNull` | Placeholder matching nothing; returns `null` / empty array. |

Default `IncludeInactive` is `true`, which matters for pooled or currently hidden game objects (enemies, pickups...) that are part of the scene but deactivated.

### Implementations

- `UnityObjectFinder` is the real implementation, based on `GameObject.FindObjectsOfType(type, includeInactive)`.
- `CacheableObjectFinder` is an optional decorator that caches results per selector. It currently has no cache invalidation (see the `@TODO` in the source).
- The registered `IObjectFinder` service resolves directly to `UnityObjectFinder`; the cached decorator is available but not wired. Registration lives in `InlineServiceDefinitionProvider`.

### Usage pattern

```csharp
GameObject[] enemies = _objectFinder.FindObjects(new ByComponent(typeof(Enemy)));
```

This is how patchers such as `TrackerPatcher` enumerate game objects without depending on Unity scene queries directly.

## Related documentation

- [Player documentation](../players/index.md)
- [Game internals](../game/index.md)
- [Repository README](../../README.md)
