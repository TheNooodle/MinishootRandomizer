# Available settings

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
