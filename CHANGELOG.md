# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## Unreleased

### Added

- The ingame map now shows accessible locations for the player to get, acting as an ingame tracker of sort.
    - Locations are indicated with markers, just like in the vanilla game using the Ancien Astrolabe.
    - Indoor locations are indicated with possible entrances being marked, like the vanilla game
    - Scarabs and spirits are marked with a special marker when they are not shuffled to help new players route locations such as the spirit tower or the scarab collector items.
    - Pre-release note : dungeon rewards are **not yet** indicated on the tracker when accessible
- With the addition of the ingame tracker, when starting a new game file, the following items are given for free :
    - All maps (scanned with entrances, buildings and point of interests shown)
    - Ancient Astrolabe
    - Compass
    - Explorer NPC
    - Pre-release note : those items are **not yet** replaced in the AP World.
- Added a patch that remove the trigger in the starting grotto that asks the player what type of aiming do they want
- Added a patch that removed NPCs introduction cutscenes
    - Known issue : the Unchosen (their blue version) still have their cutscene played.

## [0.2.1] - 2024-12-01

### Changed

- Changed Dungeon 2 logic :
    - "Hidden by plants" now needs Dash and a way to destroy rocks (this is fix, as this location used to have no requirements at all)
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
