# Minishoot' Adventures Randomizer

This is a mod for Minishoot' Adventures that handles randomization for various elements (such as items, NPCs, or XP Crystals).

It uses [Archipelago](https://archipelago.gg) for seed generation and multiworld integration.

## Installation

(Please note that this mod is in very early development, and that it was only tested on Windows. If you plan to play on Mac OS or Linux, the following steps might not suffice).

* Download the [latest BepInEx 5 release here](https://github.com/BepInEx/BepInEx/releases/tag/v5.4.23.2).
    * BepInEx 6 will **not** work.
    * Choose the right version depending on your machine architecture (in 99% of the case, choose "BepInEx_win_x64_5.4.23.2.zip")
* Extract the BepInEx archive content into the root of the game directory
    * e.g. on Windows, you should have a `BepInEx` folder alongside `Minishoot.exe`, in a folder called `Windows`.
* Launch the game a first time. This will allow BepInEx to create all necessary files. You can close the game afterwards when you arrive at the main menu.
* **IMPORTANT** : For the randomizer to work, you need to modify the configuration file of BepInEx. Once the game is closed, go to `<game-root-directory>/BepInEx/config` and edit the `BepInEx.cfg` file (for example, with Notepad).
    * After that, search for the line containing `HideManagerGameObject` (it should be near the top of the file by default), and ensure that its value is set to `true`.
* Download the randomizer [here](https://github.com/TheNooodle/MinishootRandomizer/releases).
* Extract `MinishootRandomizer.zip` in `<game-root-directory>/BepInEx/plugins`.
    * You should have a `MinishootRandomizer` directory in the `plugins` folder, with some `.dll` files in the former.
* Launch the game. Once on the title screen, you should see a window in the top-left titled "Randomizer Menu". This means the mod was successfully installed.
    * Please note that the game might be slower to start, due to the randomizer doing some bootstrapping work before letting the game start.

## Starting a randomized game

### Hosting an AP server

* Download the AP World [here](https://github.com/TheNooodle/Archipelago/releases).
* Go over your root Archipelago directory, and put the `.apworld` file in the `custom_worlds` directory.
* Also in the Archipelago directory, put the players `.yaml` files in the `Players` folder
    * The `archipelago-template.yaml` file in the [release assets](https://github.com/TheNooodle/MinishootRandomizer/releases) should document every available settings.
    * For more info, head over the Archipelago website/Discord server.
* Execute `ArchipelagoLauncher.exe`, and click on "Generate" to generate the seed.
* To start hosting the game, execute `ArchipelagoLauncher.exe`, and click on "Host" to host your seed.
    * As an alternative, you can upload the `.zip` file created when you clicked on "Generate" to [this URL](https://archipelago.gg/uploads) to let `archipelago.gg` host your game.
    * The zip file is located in the `output` folder by default.

### Joining an AP server

* Before joining a server, you should backup your saves (if you want to keep your vanilla progress).
    * If by mistake you load your vanilla save file while being connected to an AP server, the server will send all items belonging to you, effectively making you overpowered.
    * A future update will save connection infos per save file, avoiding this issue.
    * On Windows, your save files are located at `%appdata%\LocalLow\SoulGame\Minishoot`
* Launch the game, go over to the title screen.
* In the window on the top left, enter the address, your slot name and optionally the room password.
* Press `Connect`.
    * If you cannot connect to the server (timeout, bad credentials...), you will be given an error.
* You should see `Connected` written.
* Start a new save file (or continue an existing one).
* Enjoy !

## Technical caveats

* If the mod don't/can't connect to a server, launching a save file will play the vanilla game.
* If you lose the connection to the server while playing, the mod will attempt to reconnect periodically.
    * Right now, connection attempts block the game. A future update will make sure that everything network-related plays in the background.
* When starting or continuing a save file, the mod will attempt to resync with the server (i.e. you will receive all the items that other players sent you while you were away).
    * This also works if you deleted your save file (you will still need to progress the game locally, e.g. push buttons, gain xp on enemies, complete dungeons...).
* You will start the game with the following item unlocked :
    * The starting cannon
    * All maps (scanned with entrances, buildings and point of interests shown)
    * Ancient Astrolabe
    * Compass
    * Explorer NPC

## Available settings

* NPC Sanity
    * Randomizes all trapped NPCs, which means NPCs are now items, and their previous locations (especially the towers) are now locations to check.
* Scarab Sanity
    * Randomizes all 18 scarabs locations, hidden by their respectives destroyable objects. Scarabs will be available as normal pickup items.
* ShardSanity
    * Randomizes the XP shards "groups" (not individual crystals). You will be able to receive XP via items.
* Key Sanity
    * When enabled, all small keys of dungeons will be randomized outside of their respective dungeons.
    * When disabled, the keys will still be randomized within their dungeons.
* Boss Key Sanity
    * Same thing as above, but with boss keys.
* Show Archipelago item category
    * When enabled, Archipelago items sprites will indicate if its an important item (with an arrow pointing up), an helpful one (with the default icon), or not important (with a black and white sprite).
* Simple Temple Exit
    * When enabled, this setting simplifies the exit after the scarab boss in the three temples and remove the checkpoints at the three statues (where the vanilla powers are located).
    * For Temple 1 : it removes the scripted encounter, the barriers and activate the bridge.
    * For Temple 2 : it removes some of the enemies to facilitate exit.
    * For Temple 3 : it removes both enemies and the door blocking the way out.
* Blocked Forest
    * When enabled, replaces the bushes in the forest secret pond (you can access this pond with the secret path located to the south of the main town) with rocks.
    * This change in logic ensures that the south of the forest is not considered in logic for sphere 0-1 locations, as they are blocked by a quite difficult fight.
    * The Supershot/Primordial Crystal requirement also make sure that you are geared up for this fight, especially when combined with the next setting.
* Cannon Level Logical Requirement
    * When enabled, any locations or transitions with a obligatory fight will require a certain amount of progressive cannon level.
    * The main goal of this setting is to make sure that an appropriate number of progressive cannon items are available before necessary difficult fights.
        * e.g no dungeon 3 boss with a level 2 cannon
* Boostless Springboards
    * When this setting is off, the logic will require you to use the boost to jump from springboards.
    * When this setting is on, the dash will be enough to jump from springboards logically, which make the dash even more useful.
* Boostless Spirit Races
    * When this setting is off, races against spirits will logically require the boost.
    * When this setting is on, the logic will assume that you can complete those races with the dash instead.
    * Note that you will still need the boost to complete the the spirits races of the Beach, Scarab Temple and Sunken City.
    * Also note that this setting may require you to farm some XP to level up your speed.
* Boostless Torch Races
    * When this setting is off, timed torch races will logically require the boost.
    * When this setting is on, the logic will assume that you can complete those races without it.
    * Note that this setting may require you to farm some XP to level up your speed.
* Completion Goal
    * Determines the goal of the seed.
    * "Dungeon 5" means you have to beat the normal ending. You will need all 4 skills, all 4 dungeon rewards, and the Dark Key (the item you get in the vanilla game when beating both dark spirits in the Junkyard)
    * "Snow" means you have to beat the true ending. You will need the Dash, the Spirit Dash, the Bard and the Dark Heart (the item you get in the vanilla game in the Abyss Church, under the statue that moves after beating Dungeon 5)
        * Depending on your settings, you might also need the Supershot to access the tree in the forest.
    * "Both" means you have to complete both endings.
        * Note that the first two goals are mutually exclusives, meaning you can beat the true ending before beating Dungeon 5 (or even entering any dungeon).

## Gameplay tips

* The ingame map shows the locations you can check with your current inventory, just like in the vanilla game with the Ancient Astrolabe.
    * Indoor locations are indicated with possible entrances being marked, like the vanilla game.
    * Scarabs and spirits are marked with a special marker when they are not shuffled to help new players route locations such as the spirit tower or the scarab collector items.
* The Primordial Crystal can break rocks and walls. It **cannot** light torches or break cracks in trees (such as shop entrances).
* You can use your dash to cross most gaps with a springboard (by dashing in mid-air, from the springboard, or both at the same time).
* Some items might only appear when destroying certains objects, such as pots, rocks or bushes.
* When beating both endings, it is recommended to beat the true ending first, as the normal ending triggers an unskippable credits scene.
