# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## Unreleased

### Added

- The randomizer will now properly checks dungeon reward locations when collecting the "skulls" at the 4 dungeons.

### Fixed

- Fixed cache stampede issue when receiving a remote item while being on the map screen.

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
