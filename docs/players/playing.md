# Starting a randomized game

## Hosting an AP server

* Download the AP World [here](https://github.com/TheNooodle/Archipelago/releases).
* Go over your root Archipelago directory, and put the `.apworld` file in the `custom_worlds` directory.
* Also in the Archipelago directory, put the players `.yaml` files in the `Players` folder
    * The `archipelago-template.yaml` file in the [release assets](https://github.com/TheNooodle/MinishootRandomizer/releases) should document every available settings.
    * For more info, head over the Archipelago website/Discord server.
* Execute `ArchipelagoLauncher.exe`, and click on "Generate" to generate the seed.
* To start hosting the game, execute `ArchipelagoLauncher.exe`, and click on "Host" to host your seed.
    * As an alternative, you can upload the `.zip` file created when you clicked on "Generate" to [this URL](https://archipelago.gg/uploads) to let `archipelago.gg` host your game.
    * The zip file is located in the `output` folder by default.

## Joining an AP server

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
* When starting or continuing a save file, the mod will attempt to resync with the server (i.e. you will receive all the items that other players sent you while you were away).
    * This also works if you deleted your save file (you will still need to progress the game locally, e.g. push buttons, gain xp on enemies, complete dungeons...).
* You will start the game with the following items unlocked :
    * The starting cannon
    * All maps (scanned with entrances, buildings and point of interests shown)
    * Ancient Astrolabe
    * Compass
    * Explorer NPC

## Gameplay tips

* The ingame map shows the locations you can check with your current inventory, just like in the vanilla game with the Ancient Astrolabe.
    * Indoor locations are indicated with possible entrances being marked, like the vanilla game.
    * Scarabs and spirits are marked with a special marker when they are not shuffled to help new players route locations such as the spirit tower or the scarab collector items.
    * Markers without a golden outline are out-of-logic : you can access them, but the randomizer does not expect you to do so at this moment, and the next logical step might be elsewhere.
* The Primordial Crystal can break rocks and walls. It **cannot** light torches or break cracks in trees (such as shop entrances).
* You can use your dash to cross most gaps with a springboard (by dashing in mid-air, from the springboard, or both at the same time).
* Some items might only appear when destroying certains objects, such as pots, rocks or bushes.
* When beating the normal ending, you only have to beat the Dungeon 5 boss.
    * If you don't want to trigger the credits, escape Dungeon 5 as normal. When outside, save & quit **without moving**.
    * If you move, you will trigger the credits.
