# Logic system

The logic system determines whether a region or location can be reached with the player's current inventory and the active randomizer settings. It is shared by the randomizer engine, the in-game tracker and the automated logic tests.

The implementation is in `MinishootRandomizer/Randomizer/Logic/`. World data is stored in `Resources/locations.csv`, `Resources/regions.csv` and `Resources/transitions.csv`.

## Responsibilities

The logic subsystem has four main responsibilities:

- build a `LogicState` from the current game state;
- evaluate textual rules against that state;
- crawl the region graph through usable transitions;
- classify locations as `InLogic`, `OutOfLogic` or `Inaccessible`.

The parser interface contains an important compatibility requirement:

> `ILogicParser` must maintain function parity with the implementation in the Archipelago world.

Changing a rule in the plugin without making the same change in the AP World causes external trackers and the in-game tracker to disagree.

## Evaluation pipeline

The normal evaluation flow is:

```text
Game state and randomizer settings
        |
        v
ILogicStateProvider -> LogicState
        |
        +--> CoreLogicParser.ParseLogic()
        |
        +--> CoreRegionLogicChecker
        |        -> evaluate transition rules
        |        -> crawl reachable regions
        |
        +--> CoreLocationLogicChecker
                 -> resolve location region
                 -> evaluate location rule
                 -> classify accessibility
```

`LocalLogicStateProvider` creates a state by querying every item from `IItemRepository` and calling `Item.GetOwnedQuantity()`. It then copies settings from `IRandomizerEngine` into the state.

`CachedLogicStateProvider` caches the result and clears it when relevant events occur, including item collection, NPC freeing, entering a game location, scarab currency changes, goal completion and exiting the game.

## LogicState

`LogicState` stores:

- item quantities, keyed by item identifier;
- settings, keyed by their concrete setting type.

`HasItem(item)` checks for at least one copy. `HasItem(item, count)` checks a minimum quantity. `SetItemCount()` replaces a count, while `AddItemCount()` increments it.

`SetSetting()` replaces an existing setting of the same type. A missing setting returns the default value for its type, so rules should account for optional settings when they need a fallback.

## Rule syntax

Rules are stored as plain strings in CSV columns. The parser supports a conjunction of conditions and a disjunction of alternatives:

```text
condition and condition
condition or condition
condition and condition or condition
```

The parser splits on the words `and` and `or`, surrounded by whitespace. `and` has effectively higher precedence because each `or` branch is evaluated as an AND group.

An integer argument may be passed in parentheses:

```text
have_d1_keys(2)
can_buy_from_scarab_collector_3
can_cross_gaps and can_fight_lvl2
```

The current parser does not implement nested expressions or arbitrary parentheses. Parentheses are interpreted only as the argument syntax for a rule.

`ParseLogic()` returns a `LogicParsingResult` containing:

- `Result`, the boolean evaluation result;
- `UsedItemNames`, the items referenced while evaluating the rule.

Unknown rule names raise `UnknownRuleException`. Invalid integer arguments can raise a parsing exception.

## Built-in rules

All built-in rules are declared in the rule table in `CoreLogicParser.ParseRule()`. The following list is the current rule vocabulary.

### Constants and NPCs

| Rule | Behavior |
| --- | --- |
| `true` | Always succeeds. |
| `can_free_blacksmith` | Requires the Blacksmith item. |
| `can_free_mercant` | Requires the Merchant item. |
| `can_free_scarab_collector` | Requires the Scarab Collector item. |
| `can_free_bard` | Requires the Bard item. |
| `can_free_family` | Requires Family Child, Family Parent 1 and Family Parent 2. |

### Combat and destruction

| Rule | Behavior |
| --- | --- |
| `can_fight` | Requires a cannon. |
| `can_fight_lvl2` | Requires cannon level 2 unless cannon level requirements are ignored. |
| `can_fight_lvl3` | Requires cannon level 3 unless cannon level requirements are ignored. |
| `can_fight_lvl4` | Requires cannon level 4 unless cannon level requirements are ignored. |
| `can_fight_lvl5` | Requires cannon level 5 unless cannon level requirements are ignored. |
| `can_destroy_bushes` | Requires a cannon. |
| `can_destroy_ruins` | Requires a cannon. |
| `can_destroy_pots` | Requires a cannon. |
| `can_destroy_crystals` | Requires a cannon. |
| `can_destroy_plants` | Requires a cannon. |
| `can_destroy_coconuts` | Requires a cannon. |
| `can_destroy_shells` | Requires a cannon. |
| `can_destroy_trees` | Requires Supershot. |
| `can_destroy_rocks` | Requires Supershot, or Primordial Crystal when its logic setting is enabled. |
| `can_destroy_walls` | Same behavior as `can_destroy_rocks`. |
| `can_blast_crystals` | Requires a cannon and Power of Protection. |
| `can_light_torches` | Requires Supershot. |

### Movement and races

| Rule | Behavior |
| --- | --- |
| `can_cross_gaps` | Requires Dash, unless the configured dashless-gap tolerance permits Boost or neither. |
| `can_cross_tight_gaps` | Same as `can_cross_gaps` for tight gaps. |
| `can_cross_very_tight_gaps` | Same as `can_cross_gaps` for very tight gaps. |
| `can_surf` | Requires Surf. The water type argument is internal and currently does not change this check. |
| `can_boost` | Requires Boost. |
| `can_use_springboards` | Requires Boost, or Dash when `BoostlessSpringboards` is enabled. |
| `can_race_spirits` | Requires Boost, or Dash when `BoostlessSpiritRaces` is enabled. |
| `can_race_torches` | Requires Boost unless `BoostlessTorchRaces` is enabled. |
| `can_dodge_purple_bullets` | Requires Dash and Spirit Dash. |

### Keys, currencies and collection thresholds

| Rule | Behavior |
| --- | --- |
| `have_d1_keys(n)` | Requires at least `n` Dungeon 1 small keys. |
| `have_d1_boss_key` | Requires the Dungeon 1 boss key. |
| `have_d2_keys(n)` | Requires at least `n` Dungeon 2 small keys. |
| `have_d2_boss_key` | Requires the Dungeon 2 boss key. |
| `have_d3_keys(n)` | Requires at least `n` Dungeon 3 small keys. |
| `have_d3_boss_key` | Requires the Dungeon 3 boss key. |
| `can_buy_from_scarab_collector_1` through `_6` | Requires `index * scarab_items_cost` Scarabs, where the setting defaults to 3. |
| `have_all_spirits` | Requires the configured spirit tower requirement, defaulting to 8 spirits. |

### World progression and goals

| Rule | Behavior |
| --- | --- |
| `can_obtain_super_crystals` | Requires a cannon, Dash, Supershot and normal Surf. |
| `can_open_dungeon_5` | Requires Dungeon 1-4 rewards and the Dark Key. |
| `can_unlock_final_boss_door` | Requires Dark Heart. |
| `can_unlock_primordial_cave_door` | Requires Scarab Key. |
| `can_open_north_city_bridge` | Requires Dash, cannon level 4, gold Surf and wall destruction. |
| `can_open_sunken_temple` | Requires gold Surf, cannon level 4, Dash and wall destruction. |
| `can_clear_both_d5_arenas` | Requires cannon level 5, Dash and soiled Surf. |
| `can_light_all_scarab_temple_torches` | Requires normal Surf, Supershot and cannon level 4. |
| `can_light_city_torches` | Requires Supershot, gold Surf, cannon level 4 and springboards. |
| `can_light_desert_grotto_torches` | Requires Supershot, normal Surf or normal gap crossing, and cannon level 3. |
| `can_open_swamp_tower` | Requires normal Surf or springboards. |

### Setting-dependent rules

| Rule | Behavior |
| --- | --- |
| `forest_is_blocked` | Succeeds when `BlockedForest` is enabled. |
| `forest_is_open` | Succeeds when `BlockedForest` is disabled. |

## Combat level requirements

`CanFight()` checks the `IgnoreCannonLevelRequirements` setting. When it is disabled, `can_fight_lvlN` requires at least `N` Progressive Cannon items. When enabled, any cannon count satisfies the fight requirement.

This setting is separate from the item category or item count used by the randomizer. It changes only the logic evaluation of fight requirements.

## Dashless and boostless settings

Movement rules intentionally model alternate execution strategies:

- `BoostlessSpringboards` allows Dash to replace Boost for springboards;
- `BoostlessSpiritRaces` allows Dash to replace Boost for spirit races;
- `BoostlessTorchRaces` removes the Boost requirement from torch races;
- `DashlessGaps` can allow Boost or neither item for progressively tighter gaps.

These settings affect logical reachability only. They do not grant the player an ability or alter the underlying Unity physics.

## In-logic and out-of-logic evaluation

`OutOfLogicStateDecorator` reuses the real inventory but overrides selected settings with permissive values:

- ignore cannon level requirements;
- allow boostless springboards;
- allow boostless spirit races;
- allow boostless torch races;
- enable Primordial Crystal wall logic;
- allow crossing gaps without Dash.

This creates a second tolerance level. It answers: "Can the player technically reach this location by using a less conservative route or technique?" It does not add items to the inventory.

The resulting accessibility values are:

- `InLogic`: reachable with the normal logic state;
- `OutOfLogic`: not reachable with normal logic, but reachable with the permissive state;
- `Inaccessible`: not reachable with either state.

## Region accessibility

`CoreRegionLogicChecker` treats regions as graph nodes and transitions as directed edges. The crawl starts at `Region.StartingGrottoLake`.

The normal pass repeatedly evaluates outgoing transitions with the real `LogicState` until no new regions are found. It then evaluates remaining transitions with `OutOfLogicStateDecorator`. Finally, every region not reached by either pass is marked inaccessible.

The checker has a maximum of 1000 iterations per pass. Reaching the limit logs an error to protect the game from an accidental cyclic traversal problem.

## Location accessibility

`CoreLocationLogicChecker` evaluates a location in two stages:

1. Resolve the location's region through `IRegionRepository.GetRegionByLocation()`.
2. If the region is inaccessible, return `Inaccessible`.
3. Evaluate the location rule with the normal state.
4. If it succeeds, return the region's accessibility.
5. Otherwise evaluate the rule with the out-of-logic state.
6. Return `OutOfLogic` if that succeeds, otherwise `Inaccessible`.

The location rule is therefore evaluated in the context of both graph reachability and local requirements. A valid location rule cannot make an inaccessible region reachable.

## Cache behavior

Cached region and location checkers use a cache key based on `LogicState.GetCacheKey()`. Strict and lenient states use different keys through `LogicTolerance`, preventing normal and out-of-logic calculations from being mixed.

Whenever the inventory or relevant world state changes, the corresponding cache must be cleared. If a new event changes accessibility, its listener should be connected to the relevant cache invalidation handler in `InlineServiceDefinitionProvider`.

## Testing rules

Logic tests are defined in `MinishootRandomizer.Tests/Randomizer/Logic/logic_tests.yaml` and executed by `LogicTests`.

The YAML structure supports:

```yaml
default_settings:
  blocked_forest: 'true'
default_item_counts:
  Progressive Cannon: 1
test_cases:
  - name: "Example"
    override_settings:
      enable_primordial_crystal_logic: 'true'
    add_items:
      Primordial Crystal: 1
    tests:
      - region_accessibility:
          region_name: "Desert Grotto - East Drop"
          expected_accessibility: "in_logic"
```

Supported assertion types are `location_accessibility` and `region_accessibility`. Expected values are `in_logic`, `out_of_logic` and `inaccessible`.

When adding or changing a rule:

1. Update the parser and the AP World implementation together.
2. Add a test for the minimum required inventory.
3. Add a test for disabled and enabled setting variants when relevant.
4. Add an out-of-logic case when the rule has permissive behavior.
5. Run `dotnet test` from the solution root.

## Extension points and cautions

New rule implementations belong in `CoreLogicParser`, but the rule name should be treated as a compatibility API because it appears in CSV data and the AP World.

Prefer small helper methods for reusable mechanics, such as movement or combat checks. Return the items used by a rule in `LogicParsingResult` so consumers can explain or visualize the requirement.

Do not use Unity APIs from the parser or core logic checkers. Their inputs should be repositories, `LogicState` and model objects, which keeps the logic testable without a running game. Unity-specific inventory queries belong in `ILogicStateProvider` or item implementations.

## Related documentation

- [Runtime architecture](./architecture.md)
- [Data model](./data-model.md)
- [Developer documentation](./index.md)
