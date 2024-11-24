# Minishoot' Adventures Randomizer

This is a mod for Minishoot' Adventures that handles randomization for various elements (such as items, NPCs, or XP Crystals).

It uses [Archipelago](https://archipelago.gg) for seed generation and multiworld integration.

## Installation

(Please note that this mod is in very early development, and that it was only tested on Windows. If you plan to play on Mac OS or Linux, the following steps might not suffice).

* Download BepInEx **5** [here](https://github.com/BepInEx/BepInEx/releases)
    * BepInEx 6 will **not** work.
* Extract the BepInEx archive content into the root of the game directory
    * e.g. on Windows, you should have a `BepInEx` folder alongside `Minishoot.exe`, in a folder called `Windows`.
* Launch the game a first time. This will allow BepInEx to create all necessary files. You can close the game afterwards.
* **IMPORTANT** : For the randomizer to work, you need to modify the configuration file of BepInEx. Once the game is closed, go to `<game-root-directory>/BepInEx/config` and edit the `BepInEx.cfg` file (for example, with Notepad).
    * After that, search for the line containing `HideManagerGameObject` (it should be near the top of the file by default), and ensure that its value is set to `true`.
* Download the randomizer [here](https://github.com/TheNooodle/MinishootRandomizer/releases).
* Extract `MinishootRandomizer.zip` in `<game-root-directory>/BepInEx/plugins`.
    * You should have a `MinishootRandomizer` directory in the `plugins` folder, with some `.dll` files in the former.
    * The folder also have a `archipelago-template.yaml` file, which you can use to generate seeds using Archipelago.
* Launch the game. Once on the title screen, you should see a window in the top-left titled "Randomizer Menu". This means the mod was successfully installed.
    * Please note that the game might be slower to start, due to the randomizer doing some bootstrapping work before letting the game start.

## Starting a randomized game

### Hosting an AP server

* Download the AP World [here](https://github.com/TheNooodle/Archipelago/releases).
* Go over your root Archipelago directory, and put the `.apworld` file in the `custom_worlds` directory.
* Also in the Archipelago directory, put the players `.yaml` files in the `Players` folder
    * The `archipelago-template.yaml` file in the mod folder should document every available settings.
    * For more info, head over the Archipelago website/Discord server.
* Execute `ArchipelagoGenerate.exe` to generate the seed.
* To start hosting the game, execute `ArchipelagoServer.exe output/<seed-zip-file>`, where `<seed-zip-file>` is the generated file from the step before.

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
* You will start the game with a level one cannon.
    * Right now, this means that a spare cannon level is available.
    * There is no level 6 upgrade : picking up the 6th upgrade will do nothing.

## Available settings

* NPC Sanity
    * Randomizes all trapped NPCs, which means NPCs are now items, and their previous locations (especially the towers) are now locations to check.
* ShardSanity
    * Randomizes the XP shards "groups" (not individual crystals). You will be able to receive XP via items.
* Key Sanity
    * When enabled, all small keys of dungeons will be randomized outside of their respective dungeons.
    * When disabled, the keys will still be randomized within their dungeons.
* Boss Key Sanity
    * Same thing as above, but with boss keys.
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
* Completion Goal
    * Determines the goal of the seed.
    * "Dungeon 5" means you have to beat the normal ending. You will need all 4 skills, all 4 dungeon rewards, and the Dark Key (the item you get in the vanilla game when beating both dark spirits in the Junkyard)
    * "Snow" means you have to beat the true ending. You will need the Dash, the Spirit Dash, the Bard and the Dark Heart (the item you get in the vanilla game in the Abyss Church, under the statue that moves after beating Dungeon 5)
        * Depending on your settings, you might also need the Supershot to access the tree in the forest.
    * "Both" means you have to complete both endings.
        * Note that the first two goals are mutually exclusives, meaning you can beat the true ending before beating Dungeon 5 (or even entering any dungeon).

## Gameplay tips

* The Primordial Crystal can break rocks and walls. It **cannot** light torches or break cracks in trees (such as shop entrances).
* You can use your dash to cross most gaps with a springboard (by dashing in mid-air, from the springboard, or both at the same time).
    * This trick is **not** in logic at the moment, as it make the Boost not that important. A new setting will handle this aspect in the future.
* Some items might only appear when destroying certains objects, such as pots, rocks or bushes.
* When beating both endings, it is recommended to beat the true ending first, as the normal ending triggers an unskippable credits scene.
