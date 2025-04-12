# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [0.4.0] - 2025-04-12

### Added

- A new setting called "Add Trap Items" has been added.
    - When enabled, traps will be added in the item pool as filler items.
    - As such, a new item called "Primordial Scarab Dialog" has been added, to trigger a dialog with the Primordial Scarab.
- A new setting called "Trap Items Appearance" has been added.
    - When set to "Major Items Only", trap items will be disguised as major items.
    - When set to "Junk Items Only", trap items will be disguised as junk items.
    - When set to "Anything", trap items can be disguised as any other items.
    - This setting affects Minishoot' Adventures trap items, as well as Archipelago items from other games.
- A new setting called "Enable Primordial Crystal Logic" has been added.
    - cf "Changed"
- A new setting called "Progressive Dash" has been added.
    - When enabled, the game will fuse the Dash and the Spirit Dash into two cumulative progressive upgrades.
    - The first upgrade will always allow you to dash, and the second one will allow you to dash through bullets.
    - As such, a new item called "Progressive Dash" has been added for this setting.
- A new setting called "Dashless Gaps" has been added.
    - When set on "Needs Dash", you will need the dash to cross gaps, regardless of their size.
    - When set on "Needs Boost", you will be able to logically cross gaps with the boost if the gap is small enough.
    - When set on "Needs neither", you will be able to cross certains, very tight gaps without any upgrade.
    - Note that this last value may require you to farm some XP to level up your speed.
- On Archipelago, when another world uses the `/collect` command, this world's items in Minishoot' Adventures will be correctly removed next time the save file is started.
- Added a version check when connecting to an Archipelago server.
    - Each client version will be assigned a range of compatible APWorld it can connect to (based on semantic versioning)
    - For example, client version 0.4.0 (as well as 0.4.1 and 0.4.2) will only be able to connect to APWorld version 0.4.0.
    - If the version of the APWorld is incompatible, the client will force a disconnection.
    - Note that the client will not check the version of Archipelago proper, only the Minishoot' Adventures APWorld.

### Removed

- Removed the setting "Simple Temple Exit"
    - This setting should be enabled all the time to avoid having the player getting stuck in temples.
    - This is even more problematic with random settings on Archipelago.
    - As such, it didn't make sense to keep this setting, and is now enabled by default.

### Changed

- The setting "Cannon Level Logical Requirements" has been changed :
    - It is now called "Ignore Cannon Level Requirements".
    - Its function has been reversed (meaning that if set to true, fights will only check the first cannon level).
    - This change has been made to normalize "difficulty settings", i.e. the seed will be harder if set to "true" (this setting was the only outlier, making the seed harder if set to "false").
- The "Primordial Crystal" is no longer considered in logic to destroy rocks and walls.
    - A new setting called "Enable Primordial Crystal Logic" has been added and, if set to true, can re-establish the previous logic.
- In an effort to stabilize seed generation on Archipelago, several changes has been made to the randomizer logic :
    - Changed logic rules that depends on the logic state being able to reach a certain region.
        - For example, the entrance to Swamp Tower would check if the player have access to "Swamp - South West Island" to activate the corresponding button.
        - Those checks were made to ease in a future entrance randomizer (ER) addition, but made seed generation on Archipelago failing some tests.
        - For now, those checks are being replaced by checking the correct item, in a non-ER context. Those will be properly replaced when ER will be a thing.
    - Dungeon 1 :
        - One door leading to "Dungeon 1 - South Item" has been removed. This means that this dungeon now has an extra key.
        - Removed the usage of the "cannot_dash" rule, meaning that small keys will likely be placed in the first part of the dungeon (when they are not shuffled).
    - Dungeon 3 :
        - One door leading to "Dungeon 3 - South corridor" has been removed. This means that this dungeon now has an extra key.
        - Removed the usage of the "cannot_surf" rule.
    - Abyss Church
        - The Unchosen Statue is now opened by default (instead of being opened when the player defeats the boss of Dungeon 5).
- Removed arbitrary logic rules for the fights against the boss of Dungeon 1 and the Busher boss in Forest (the one guarding the Spirit Tower).
    - Those fights now only checks for cannon levels.
- "Dungeon 3 - North arena" now properly checks for the "Boostless Torch Races" setting.
- Renamed all instances of "Mercant" to "Merchant".
- Temporarily removed file saving when receiving either a Small Key or a Boss Key.
    - This change is made to alleviate issues where exiting the map screen can drastically drop the game framerate.
    - This bug circumstances are still unclear, hence the temporary change.

## [0.3.2] - 2025-02-24

### Added

- The randomizer will now properly checks dungeon reward locations when collecting the "skulls" at the 4 dungeons.
- The map will now show objective markers according to the player's goal.
    - Setting the goal to "Dungeon 5" will show the 4 dungeon rewards when accessible, as well as the boss of dungeon 5.
    - Setting the goal to "Snow" will show the objective to beat the Unchosen when accessible.
    - Setting the goal to "Both" will do all of the above.
- The map will now use smaller, more discrete markers to reveal locations or goals that are accessible out-of-logic
    - Right now, out-of-logic checks are considered by your seed settings.
    - In a future update, it will also check key logic for dungeons.
- Restored floating animations for map markers.
- Restored floating animations for item pickups.

### Changed

- Changed custom markers on the map for scarab and spirits (when they are not shuffled) with better icons.

### Fixed

- Fixed cache stampede issue when receiving a remote item while being on the map screen.
- Tracker logic now correctly uses the total number of small key obtained, instead of using the current key count.

## [0.3.1] - 2025-02-01

### Added

- When collecting an item (either locally or by receiving it from a remote source), a message will appear in the bottom right corner.
    - When receiving multiple items at once, messages will queue and display in order.
    - Items will still be collected instantly.

### Fixed

- Fixed tracker logic for spirit tower entrance to correctly require 8 spirits.
    - This change only affects the tracker.

## [0.3.0] - 2025-01-19

### Added

- The ingame map now shows accessible locations for the player to get, acting as an ingame tracker of sort.
    - Locations are indicated with markers, just like in the vanilla game using the Ancien Astrolabe.
    - Indoor locations are indicated with possible entrances being marked, like the vanilla game.
    - Scarabs and spirits are marked with a special marker when they are not shuffled to help new players route locations such as the spirit tower or the scarab collector items.
    - Note : dungeon rewards are **not yet** indicated on the tracker when accessible.
- With the addition of the ingame tracker, when starting a new game file, the following items are given for free :
    - All maps (scanned with entrances, buildings and point of interests shown)
    - Ancient Astrolabe
    - Compass
    - Explorer NPC
    - Those items are replaced by filler items.
- Added a patch that removes the trigger in the starting grotto that asks the player what type of aiming do they want.
- Added a patch that removes NPCs introduction cutscenes.
- Added a new Super Crystals item (the red money the player can spend at the shop).
- Replaced items (such as tracker items and the extra cannon level, cf "Removed") are now replaced by filler items.
    - Those filler items are Super Crystals drops of various amounts.
    - On Archipelago, in case a location cannot be filled, it will be filled with a filler item instead.
- A new setting called "Boostless Springboards" has been added.
    - When enabled, the springboards can logically be used with the dash, instead of needing the boost.
- A new setting called "Boostless Spirit Races" has been added.
    - When enabled, most races against spirits can logically be beaten with the dash, instead of needing the boost.
    - Note that the races at Beach, Scarab Temple and Sunken City still logically need the boost.
- A new setting called "Boostless Torch Races" has been added.
    - When enabled, torch races (where the player must navigate through torch gates in a limited time) can be cleared without the need for boost.
- A new setting called "Show Archipelago item category" has been added.
    - When enabled, Archipelago items sprites will indicate if its an important item (with an arrow pointing up), an helpful one (with the default icon), or not important (with a black and white sprite).

### Removed

- Removed the extra progressive cannon level and replaced it with a filler item.

### Changed

- Archipelago items sprites now have a black outline to increase visibility in certain areas.
- Changed the logic for the location "Crystal Grove Tower - Top of tower"
    - Now no longer needs the dash or boost to get to the top.
- Changed the logic for the vanilla access to Dungeon 3 Exterior (without the surf, using the turtle)
    - Now properly requires the usage of Supershot to light torches if the player doesn't have access to Surf (D3 used to be accessible with Dash/Boost and Primordial Crystal)
    - Can no longer be cleared with the boost to get to the button to activate the bridge to D3.
    - The boost used to be considered because the coyote time to get to the button could be used to clear the gap.

## [0.2.1] - 2024-12-01

### Changed

- Changed Dungeon 2 logic :
    - "Hidden by plants" now needs Dash and a way to destroy rocks (this is a fix, as this location used to have no requirements at all)
    - Small gaps in this dungeon now needs the Dash, and are no longer in logic with the Boost
        - You can still use the Boost to cross those gaps, as the coyote time prevent you from falling
        - For now, you will need to dash across them.
        - This mainly impact all locations after the entrance area, as well as the "North West arena" location
    - "South West arena" will now require the proper amount of cannon level depending on your settings

## [0.2.0] - 2024-12-01

### Added

- Changelog file.
- New setting : Scarab Sanity
    - Adds 18 new locations, where the scarabs in the vanilla game are hidden.
    - Those locations only appear when their respective destroyable objects are entirely destroyed.

### Changed

- Changed Cemetery logic :
    - "West pot" and "Item under enemy" are no longer obtainable with Dash.
    - While the jump required to get to those locations is relatively easy to do with a gamepad, doing it with a keyboard is a lot more difficult. While there might be some additional settings altering the logic in the future, for now you will need Surf to obtain the items there.
    - "Crying house" is still obtainable with Dash and Power of Protection.
- Changed Dungeon 1 logic :
    - Added a missing XP Crystal location : "Inside the crystal wall" (in the vanilla game, it's a x5 shard).
- Replaced mentions of "debris" (both in location names and logic rules) by "crystals" to insure coherence

### Fixed

- Fixed linear shop locations being inserted at the wrong index (#3).
- Fixed item "Enchanted Powers" not having a proper model and not displaying shop prompt.

## [0.1.0] - 2024-11-24

### Added

- Initial pre-release
