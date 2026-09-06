# Data model

This document describes the data model used by the randomizer and the relationship between the embedded CSV files and the runtime objects.

## Overview

The world is represented as a graph of regions connected by transitions. Locations belong to regions and represent places where an item can be collected. Zones group regions and locations according to game scenes or larger gameplay areas.

```text
Zone
  +-- Region
        +-- Location
        +-- outgoing Transition -> Region

Location <-> Item
```

Items and locations are identified by strings. The same identifiers are used by CSV data, the randomizer engines, logic rules, Archipelago data and Unity integration. Identifier changes therefore require coordinated changes across the relevant data and code.

>The usage of CSV files is to enforce parity with the APWorld. Both the mod and the Archipelago generator must be on the same page to ensure coherence.

>CSV files are generated using a private Airtable project at the moment.


## Items

### Runtime model

`Model/Item/Item.cs` defines the abstract `Item` type. Every item has:

- an `Identifier`;
- an `ItemCategory` (progression, helpful, filler, token or trap);
- collection behavior;
- a display name and sprite identifier;
- an owned quantity;
- an indication of whether it can be used as a trap.

Concrete item classes model how an item affects the game. Examples include:

- `PickupItem` for values applied to player stats;
- `SkillItem` for abilities such as Dash or Boost;
- `ModuleItem` for player modules;
- `ArchipelagoItem` for items owned by another player;
- `TrapItem` for trap effects.
- and many more...

`DictionaryItemFactory` maps identifiers to concrete item types and game-specific identifiers such as `Stats`, `Skill`, `Modules`, `NpcIds`, `MapRegion` or `KeyUse`. Some identifiers are interpreted by convention (i.e. extracting certain values from the identifier string itself), for example :

* `XP Crystals x20` means a bundle of XP Crystals of 20 units
* `Small Key (Dungeon 1)` means adding small key to the counter of dungeon N°1.

### Item CSV

`Resources/items.csv` has the following columns:

| Column | Meaning |
| --- | --- |
| `Name` | Runtime item identifier. |
| `Category` | Value of `ItemCategory`. |
| `Item pool` | Intended randomizer pool. |
| `Count` | Number of copies in the generated pool. |
| `Notes` | Human-readable or implementation-related notes. |

The current `CsvItemRepository` uses `Name` and `Category` to construct runtime items. `Item pool`, `Count` and `Notes` are source-data metadata and are not currently stored on the `Item` object by that repository.

Items are loaded lazily on the first `Get()` or `GetAll()` call and cached in a dictionary by identifier. An unknown identifier raises `ItemNotFoundException`; an identifier that cannot be mapped by the factory raises `InvalidItemException`.

## Locations

### Runtime model

`Model/Location/Location.cs` defines the abstract location type. A location has:

- an `Identifier`;
- a `LogicRule`;
- a `LocationPool`;
- an `Accept(ILocationVisitor, Item)` method used to apply an item through the appropriate game-specific visitor.

`LocationPool` identifies the randomization group:

- `Default`;
- `XpCrystals`;
- `Goal`.
- and more...

Concrete location types describe how a location is represented in the game:

- `PickupLocation` targets an existing Unity pickup;
- `ShardLocation` represents a group of XP shards at coordinates;
- `CrystalNpcLocation` targets an NPC;
- `LinearShopLocation` and `ChoiceShopLocation` represent shop slots;
- `DestroyableLocation` targets an object such as a pot, bush or rock;
- `SpiritLocation` represents a spirit race;
- `DungeonRewardLocation` represents a dungeon reward;
- `GoalLocation` represents a completion goal.

`DictionaryLocationFactory` selects the concrete type using hardcoded mappings, the location pool and naming conventions. It also contains the Unity selectors, coordinates, shop metadata and goal mappings needed to instantiate the game-facing location.

This means adding a row to `locations.csv` is not always sufficient. New location names usually also need a factory mapping or a supported naming/pool convention.

### Location CSV

`Resources/locations.csv` has the following columns:

| Column | Meaning |
| --- | --- |
| `Name` | Unique location identifier. |
| `Vanilla Item` | Item found at this location in the unmodified game. |
| `Location pool` | Randomization pool, mapped to `LocationPool`. |
| `Region` | Region containing the location. |
| `Logic rule` | Rule evaluated by the logic parser. |

`CsvLocationRepository` currently reads `Name`, `Location pool` and `Logic rule`. The `Vanilla Item` and `Region` columns are used as source data by other model repositories or factory-related code paths, but are not stored as properties on the base `Location` object by this repository.

Locations are loaded lazily and cached by identifier. Invalid location pools or unsupported location definitions are logged and skipped during loading.

## Regions

`Model/Region/Region.cs` represents a logical area in the world. A region has a name and a list of location identifiers. The logic checker uses the region as a graph node and determines its accessibility by evaluating outgoing transitions.

### Region CSV

`Resources/regions.csv` has these columns:

| Column | Meaning |
| --- | --- |
| `Name` | Unique region name. |
| `Locations` | Comma-separated locations belonging to the region. |
| `Outgoing Transitions` | Human-readable list of outgoing transition names. |
| `Ingoing Transitions` | Human-readable list of incoming transition names. |

`CsvRegionRepository` currently creates regions from `Name` and populates their location names from `Locations`. The transition columns are descriptive CSV data; the executable transition graph is loaded from `transitions.csv`.

`GetRegionByLocation()` resolves a location by scanning the region location lists. A location must therefore appear in the correct `Locations` field or logic evaluation will fail with `RegionNotFoundException`.

## Transitions

`Model/Transition/Transition.cs` represents a directed edge between two regions. It contains:

- a transition name;
- an origin region name;
- a destination region name;
- a logic rule.

`Resources/transitions.csv` has the following columns:

| Column | Meaning |
| --- | --- |
| `Name` | Unique transition identifier. |
| `Origin Region` | Region from which the transition starts. |
| `Destination Region` | Region reached when the transition is usable. |
| `Logic rule` | Requirement for traversing the transition. |

`CsvTransitionRepository` loads transitions lazily and provides `GetFromOriginRegion()` for graph traversal. The region checker starts from `Starting Grotto - Lake`, evaluates the transition rules, and builds the reachable region set.

Both origin and destination names must match region identifiers exactly. A typo creates a broken graph relationship rather than a typed reference error at compile time.

## Zones

`Model/Zone/Zone.cs` groups regions under a game-facing area. A zone has:

- a name;
- a `GameLocationName`, corresponding to a Unity scene or game location;
- a list of region names.

### Zone CSV

`Resources/zones.csv` contains:

| Column | Meaning |
| --- | --- |
| `Name` | Zone identifier. |
| `Regions` | Comma-separated regions in the zone. |
| `Locations` | Listed locations associated with the zone. |
| `Outgoing transitions` | Descriptive zone-level transitions. |
| `Ingoing transitions` | Descriptive zone-level transitions. |
| `Is Dungeon` | Dungeon marker used as source metadata. |
| `Game Location Name` | Unity/game location name. |

`CsvZoneRepository` currently constructs zones from `Name` and `Regions`, and reads `GameLocationName` for scene lookup. The other columns are not represented as properties on `Zone` at present.

## Repositories and factories

Repositories provide lookup and lazy loading, while factories translate data identifiers into runtime objects.

```text
Embedded CSV resource
        |
        v
Csv repository
        |
        +-- Item factory ------> concrete Item
        +-- Location factory --> concrete Location
        +-- Zone factory ------> Zone
        +-- direct parsing ----> Region / Transition
```

The main interfaces are:

- `IItemRepository` and `IItemFactory`;
- `ILocationRepository` and `ILocationFactory`;
- `IRegionRepository`;
- `IZoneRepository` and `IZoneFactory`;
- `ITransitionRepository`.

The repositories are registered as singletons by `InlineServiceDefinitionProvider`. Their data is loaded on demand and retained for the lifetime of the service.

`StreamFactory` abstracts resource access so the same repository code can read embedded resources in the plugin and file paths in tests. The production service registrations use resource names such as `MinishootRandomizer.Resources.items.csv`; logic tests use filesystem paths to load test data.

## Relationships and invariants

The following relationships are required for a consistent world model:

1. Every location identifier should be unique in `locations.csv`.
2. Every location should belong to exactly one region in `regions.csv`.
3. Every transition origin and destination should reference an existing region.
4. Every location logic rule and transition logic rule must be supported by `CoreLogicParser`.
5. Every item identifier used by CSV data or logic must be supported by `DictionaryItemFactory` or an explicit item implementation.
6. Every location that is instantiated at runtime must be supported by `DictionaryLocationFactory`.
7. Pool names in CSV files must match the mappings in the corresponding repository.

The model intentionally uses string references instead of object references at load time. This keeps CSV data simple, but makes validation and naming consistency especially important.

## Changing the data model

When adding a new item:

1. Add its row to `items.csv`.
2. Add an identifier constant to `Item` when code refers to it by name.
3. Add factory metadata or a concrete branch in `DictionaryItemFactory`.
4. Implement or reuse the correct `Collect()` behavior.
5. Add logic and tests if the item changes accessibility.

When adding a new location:

1. Add its row to `locations.csv`.
2. Add the location to the correct region in `regions.csv`.
3. Add a `DictionaryLocationFactory` mapping or use an existing supported convention.
4. Add Unity selector or replacement metadata when the location targets a game object.
5. Add the corresponding logic test case.

When adding a new region or transition:

1. Add the region to `regions.csv`.
2. Add its transitions to `transitions.csv`.
3. Add the region to a zone when it belongs to a new or existing game area.
4. Verify that all logic rules use supported parser names.
5. Add region accessibility tests.

## Related documentation

- [Runtime architecture](./architecture.md)
- [Developer documentation](./index.md)
- [Player settings](../players/settings.md)
