using System.Collections.Generic;
using UnityEngine;

namespace MinishootRandomizer
{
    public class DictionaryLocationFactory: ILocationFactory
    {
        private static Dictionary<string, string> _pickupLocationToGameObjectName = new()
        {
            { "Abyss - Backroom item", "PickupModuleRage" },
            { "Abyss Church - Unchosen statue", "PickupKeyFinalBoss" },
            { "Abyss Ruined shop - Item", "MapPickupAbyss" },
            { "Abyss Shack - Hidden room", "CaveHp4" },
            { "Beach - East Island", "PickupModuleOvercharge" },
            { "Beach - Protected item", "MapPickupBeach" },
            { "Beach - South East Island", "OverworldHp9" },
            { "Cemetery - Crying house", "OverworldHp7" },
            { "Cemetery Tower - Top of tower", "LorePickup5" },
            { "Crystal Grove Temple - Boss reward", "CavePowerBombLevel0" },
            { "Crystal Grove Temple - South West Hidden pond", "Temple1Hp0" },
            { "Desert - North east platforms", "OverworldEnergy3" },
            { "Desert Temple - Boss reward", "Temple2PowerSlowLevel0" },
            { "Dungeon 1 - Central item", "SkillDash"},
            { "Dungeon 1 - Debris wall item", "Dungeon1Hp0"},
            { "Dungeon 1 - Entrance west bridge", "Dungeon1CrystalKey2"},
            { "Dungeon 1 - Far West Arena after spinning enemy", "Dungeon1CrystalKey1"},
            { "Dungeon 1 - Near east spinning enemy", "Dungeon1CrystalKey3"},
            { "Dungeon 1 - North West Arena", "Dungeon1CrystalKey0"},
            { "Dungeon 1 - South item", "Dungeon1BossKey"},
            { "Dungeon 1 - West bridge hidden item", "Dungeon1Energy0"},
            { "Dungeon 2 - Central item", "Dungeon2BossKey" },
            { "Dungeon 2 - Item after jumps", "SkillSupershot" },
            { "Dungeon 2 - North item", "Dungeon2Hp0" },
            { "Dungeon 2 - North west arena", "Dungeon2CrystalKey0" },
            { "Dungeon 2 - South west arena", "Dungeon2CrystalKey1" },
            { "Dungeon 2 - Walled arena item", "Dungeon2CrystalKey2" },
            { "Dungeon 2 - West arena", "Dungeon2CrystalKey3" },
            { "Dungeon 3 - Central Item", "SkillHover" },
            { "Dungeon 3 - East Island", "Dungeon3CrystalKey2" },
            { "Dungeon 3 - Hidden Tunnel", "Dungeon3Energy0" },
            { "Dungeon 3 - Item protected by spikes", "Dungeon3Hp0" },
            { "Dungeon 3 - Over the pit", "Dungeon3CrystalKey0" },
            { "Dungeon 3 - Race on the water", "Dungeon3CrystalKey3" },
            { "Dungeon 3 - South corridor", "Dungeon3BossKey" },
            { "Dungeon 3 - South of torches", "Dungeon3CrystalKey1" },
            { "Dungeon 5 - Central Item", "PickupKeyDarker" },
            { "Family House Cave - Hidden Tunnel", "CaveHp1" },
            { "Family House Cave - Reunited Family", "PickupModuleHeartCrystal" },
            { "Forest - Boss Reward", "PickupModuleRetaliation" },
            { "Forest - Enemy tree near Beach", "OverworldHp6" },
            { "Forest - Faraway island item", "LorePickup2" },
            { "Forest - Hidden east tunnel", "OverworldHp3" },
            { "Forest - Secret pond behind bushes", "OverworldHp13" },
            { "Forest - Secret within Secret", "OverworldEnergy2"},
            { "Green - Closed Arena Item", "OverworldHp1" },
            { "Green - Crossroad Arena Item", "MapPickupGreen"},
            { "Green - Hidden before Island Arena", "OverworldHp0" },
            { "Green - Island Arena Item", "SkillBoost"},
            { "Green - Outside Dungeon 1 Cave", "OverworldHp2" },
            { "Green - River near Forest Entrance", "OverworldEnergy1"},
            { "Green Grotto - Drop", "PickupModuleBlueBullet"},
            { "Junkyard East Shack - Item", "MapPickupJunkyard" },
            { "Junkyard West Shack - Item", "CaveHp6" },
            { "Primordial Cave - Meet the Primordial Scarab", "PickupModulePrimordialCrystal" },
            { "Scarab Temple - Central Item", "PickupKeyScarab" },
            { "Scarab Temple - Middle Entrance", "CaveHp0"},
            { "Sewers - Central room boss reward", "LorePickup4" },
            { "Sewers - Drop", "CaveHp3" },
            { "Sewers - Near Family House Cave", "CaveHp2" },
            { "Spirit Tower - Item", "PickupModuleSpiritDash" },
            { "Starting Grotto - West Item", "CaveBulletNumber0"},
            { "Sunken City - Inside the walls", "OverworldHp5" },
            { "Sunken City - North East district", "OverworldHp8" },
            { "Sunken City - South West building item", "OverworldHp14" },
            { "Sunken City - West bridge", "MapPickupSunkenCity" },
            { "Sunken Temple - Boss reward", "Temple3PowerAllyLevel0" },
            { "Swamp - North item", "OverworldHp12" },
            { "Swamp - South West Island Hidden in trees Item", "OverworldEnergy4"},
            { "Swamp Jumps Grotto - Drop", "CaveExtraHp0" },
            { "Town Pillars - Hidden Pond", "OverworldEnergy0"},
            { "Town Pillars Grotto - Reward", "CaveHp7"},
        };

        private static Dictionary<string, ShardLocationReplacementData> _shardLocationToReplacementData = new()
        {
            { "Abyss - Ambush Island", new ShardLocationReplacementData(-65.75f, -69f, 0f) },
            { "Abyss - Near dungeon entrance", new ShardLocationReplacementData(-92f, -42f, 0f) },
            { "Abyss - Near protected enemy", new ShardLocationReplacementData(-134f, -59f, 0f) },
            { "Abyss - South of spinning enemy", new ShardLocationReplacementData(-109f, -61.5f, 0f) },
            { "Abyss - Village Entrance", new ShardLocationReplacementData(-180f, -98f, 0f) },
            { "Abyss - Within Crystal Grove", new ShardLocationReplacementData(-108f, 18f, 0f) },
            { "Abyss Shack - Hidden corridor", new ShardLocationReplacementData(-822f, -130f, 0f) },
            { "Beach - Coast Hidden by plants", new ShardLocationReplacementData(136f, -3f, 0f) },
            { "Beach - Coast North hidden alcove", new ShardLocationReplacementData(144f, 20f, 0f) },
            { "Beach - Coast South hidden alcove", new ShardLocationReplacementData(94f, -32f, 0f) },
            { "Crystal Grove Temple - Dodge the east cannons", new ShardLocationReplacementData(-703f, -399f, 0f) },
            { "Crystal Grove Temple - East tunnels", new ShardLocationReplacementData(-751f, -381f, 0f) },
            { "Crystal Grove Temple - North east hidden room", new ShardLocationReplacementData(-737f, -344f, 0f) },
            { "Desert - On the river", new ShardLocationReplacementData(-188f, 68.5f, 0f) },
            { "Desert Temple - Secret room", new ShardLocationReplacementData(-505f, -430f, 0f) },
            { "Dungeon 1 - Entrance after south ramp", new ShardLocationReplacementData(547f, 200f, 0f) },
            { "Dungeon 1 - Entrance East Arena", new ShardLocationReplacementData(616f, 206f, 0f) },
            { "Dungeon 1 - Hidden below debris wall", new ShardLocationReplacementData(544f, 217f, 0f) },
            { "Dungeon 1 - Near boss", new ShardLocationReplacementData(492f, 273f, 0f) },
            { "Dungeon 1 - Near west spinning enemy", new ShardLocationReplacementData(428f, 207f, 0f) },
            { "Dungeon 2 - Hidden by plants", new ShardLocationReplacementData(924f, 318f, 0f) },
            { "Dungeon 2 - North east beyond arena", new ShardLocationReplacementData(1032f, 336f, 0f) },
            { "Dungeon 2 - Treasure room entrance", new ShardLocationReplacementData(1018f, 278f, 0f) },
            { "Dungeon 2 - Walled arena extra", new ShardLocationReplacementData(972f, 316f, 0f) },
            { "Dungeon 2 - West arena extra", new ShardLocationReplacementData(818f, 292f, 0f) },
            { "Dungeon 3 - Behind North West doors", new ShardLocationReplacementData(767f, 6f, 0f) },
            { "Dungeon 3 - East wall", new ShardLocationReplacementData(947f, 28f, 0f) },
            { "Dungeon 3 - North arena", new ShardLocationReplacementData(867f, 16f, 0f) },
            { "Dungeon 3 - South west of torches", new ShardLocationReplacementData(769f, -78f, 0f) },
            { "Family House Cave - Before shortcut", new ShardLocationReplacementData(-685f, 51f, 0f) },
            { "Family House Cave - Near button", new ShardLocationReplacementData(-774f, -30f, 0f) },
            { "Family House Cave - Near tree", new ShardLocationReplacementData(-762f, 10f, 0f) },
            { "Family House Cave - Sewers", new ShardLocationReplacementData(-719f, 100f, 0f) },
            { "Forest - Faraway island extra", new ShardLocationReplacementData(-95f, -256f, 0f) },
            { "Forest - Tunnel below big tree enemy", new ShardLocationReplacementData(-60f, -152f, 0f) },
            { "Green - Behind Closed Arena", new ShardLocationReplacementData(73f, -26f, 0f) },
            { "Green - Bridge Shortcut", new ShardLocationReplacementData(50f, 35f, 0f) },
            { "Green - Button Item", new ShardLocationReplacementData(67f, 66f, 0f) },
            { "Green - Forest Entrance", new ShardLocationReplacementData(41f, -29f, 0f) },
            { "Green - Grove near button", new ShardLocationReplacementData(-32f, 4f, 0f) },
            { "Green Grotto - Before race", new ShardLocationReplacementData(-512f, 8f, 0f) },
            { "Green Grotto - Corner", new ShardLocationReplacementData(-604f, -38f, 0f) },
            { "Junkyard - East pond", new ShardLocationReplacementData(52f, 202f, 0f) },
            { "Junkyard - Inside Sunken City", new ShardLocationReplacementData(-108f, 169f, 0f) },
            { "Junkyard - South East", new ShardLocationReplacementData(19f, 133f, 0f) },
            { "Scarab Temple - After race 1", new ShardLocationReplacementData(-668f, -76f, 0f) },
            { "Scarab Temple - After race 2", new ShardLocationReplacementData(-678f, -74f, 0f) },
            { "Scarab Temple - After race 3", new ShardLocationReplacementData(-688f, -76f, 0f) },
            { "Scarab Temple - Bottom Left Torch Item", new ShardLocationReplacementData(-568f, 67f, 0f) },
            { "Scarab Temple - East side", new ShardLocationReplacementData(-524f, 75f, 0f) },
            { "Sewers - Behind West Entrance", new ShardLocationReplacementData(-844f, 87f, 0f) },
            { "Sewers - Central room corner", new ShardLocationReplacementData(-786f, 190f, 0f) },
            { "Starting Grotto - Entrance", new ShardLocationReplacementData(-918f, -101f, 0f) },
            { "Starting Grotto - North Corridor", new ShardLocationReplacementData(-964f, -51f, 0f) },
            { "Starting Grotto - Secret Wall", new ShardLocationReplacementData(-940f, -64f, 0f) },
            { "Sunken City - Below West bridge", new ShardLocationReplacementData(-164f, 157f, 0f) },
            { "Sunken City - Near West entrance", new ShardLocationReplacementData(-166f, 115f, 0f) },
            { "Sunken City - North bridge", new ShardLocationReplacementData(-129f, 202f, 0f) },
            { "Sunken City Building - Drop", new ShardLocationReplacementData(-249f, -440f, 0f) },
            { "Sunken Temple - Entrance", new ShardLocationReplacementData(-806f, -654f, 0f) },
            { "Sunken Temple - Secret tunnel", new ShardLocationReplacementData(-822f, -636f, 0f) },
            { "Sunken Temple - South West tunnel", new ShardLocationReplacementData(-856f, -650f, 0f) },
            { "Swamp - Blocked tunnel", new ShardLocationReplacementData(190f, 120f, 0f) },
            { "Swamp - Hidden Before Big Enemy", new ShardLocationReplacementData(109f, 159f, 0f) },
            { "Swamp - Near cracked wall", new ShardLocationReplacementData(164f, 187f, 0f) },
            { "Swamp Shop Extra", new ShardLocationReplacementData(-479f, 142f, 0f) },
            { "Town Pillars - Hidden below bridge", new ShardLocationReplacementData(-26f, 18f, 0f) }
        };

        private static Dictionary<string, string> _crystalNpcLocationToGameObjectName = new()
        {
            {"Abyss Tower - Top of tower", "Familly2"},
            {"Crystal Grove Tower - Top of tower", "ScarabCollector"},
            {"Desert Grotto - Both torches lighted", "Familly1"},
            {"Forest - Far South East", "Bard"},
            {"Forest Grotto - After ramp", "Explorer"},
            {"Green - Shortcut to Town Pillars", "MercantHub"},
            {"Scarab Temple - Backroom", "Academician"},
            {"Swamp Tower - Top of tower", "Familly3"},
            {"Town - Plaza", "Blacksmith"},
            {"Zelda 1 Grotto - Behind the closed doors", "Healer"},
        };

        private static Dictionary<string, LinearShopLocationReplacementData> _linearShopLocationToReplacementData = new()
        {
            {"Town - Blacksmith Item 1", new LinearShopLocationReplacementData("Blacksmith", 5, Currency.SuperCrystal)},
            {"Town - Blacksmith Item 2", new LinearShopLocationReplacementData("Blacksmith", 22, Currency.SuperCrystal)},
            {"Town - Blacksmith Item 3", new LinearShopLocationReplacementData("Blacksmith", 65, Currency.SuperCrystal)},
            {"Town - Blacksmith Item 4", new LinearShopLocationReplacementData("Blacksmith", 120, Currency.SuperCrystal)},
            {"Town - Scarab Collector Item 1", new LinearShopLocationReplacementData("ScarabCollector", 3, Currency.Scarab)},
            {"Town - Scarab Collector Item 2", new LinearShopLocationReplacementData("ScarabCollector", 3, Currency.Scarab)},
            {"Town - Scarab Collector Item 3", new LinearShopLocationReplacementData("ScarabCollector", 3, Currency.Scarab)},
            {"Town - Scarab Collector Item 4", new LinearShopLocationReplacementData("ScarabCollector", 3, Currency.Scarab)},
            {"Town - Scarab Collector Item 5", new LinearShopLocationReplacementData("ScarabCollector", 3, Currency.Scarab)},
            {"Town - Scarab Collector Item 6", new LinearShopLocationReplacementData("ScarabCollector", 3, Currency.Scarab)},
        };

        private static Dictionary<string, ChoiceShopLocationReplacementData> _choiceShopLocationToReplacementData = new()
        {
            {"Forest Shop 1", new ChoiceShopLocationReplacementData("MercantBusher", 7, Currency.SuperCrystal, "CaveEnergy0")},
            {"Forest Shop 2", new ChoiceShopLocationReplacementData("MercantBusher", 12, Currency.SuperCrystal, "PickupModuleXpGain")},
            {"Forest Shop 3", new ChoiceShopLocationReplacementData("MercantBusher", 4, Currency.SuperCrystal, "LorePickup1")},
            {"Jar Shop 1", new ChoiceShopLocationReplacementData("MercantJar", 5, Currency.SuperCrystal, "CaveHp5")},
            {"Jar Shop 2", new ChoiceShopLocationReplacementData("MercantJar", 10, Currency.SuperCrystal, "PickupModuleBoostCost")},
            {"Jar Shop 3", new ChoiceShopLocationReplacementData("MercantJar", 5, Currency.SuperCrystal, "MapPickupForest")},
            {"Swamp Shop 1", new ChoiceShopLocationReplacementData("MercantFrogger", 5, Currency.SuperCrystal, "MapPickupSwamp")},
            {"Swamp Shop 2", new ChoiceShopLocationReplacementData("MercantFrogger", 15, Currency.SuperCrystal, "PickupModuleHpDrop")},
            {"Swamp Shop 3", new ChoiceShopLocationReplacementData("MercantFrogger", 14, Currency.SuperCrystal, "LorePickup3")},
            {"Town - Mercant Item 1", new ChoiceShopLocationReplacementData("MercantHub", 65, Currency.SuperCrystal, "PickupModuleCompass")},
            {"Town - Mercant Item 2", new ChoiceShopLocationReplacementData("MercantHub", 2, Currency.SuperCrystal, "PickupModuleCollectableScan")},
            {"Town - Mercant Item 3", new ChoiceShopLocationReplacementData("MercantHub", 18, Currency.SuperCrystal, "PickupModuleTeleport")},
        };

        private static Dictionary<string, DestroyableReplacementData> _destroyableLocationToSelector = new()
        {
            {"Abyss North Connector - Under debris", new DestroyableReplacementData(
                new ByName("WreckShip2 (5)"),
                Vector3.zero
            )},
            {"Abyss Shack - Under pot", new DestroyableReplacementData(
                new ByName("CaveDestroyable306"),
                Vector3.zero
            )},
            {"Beach - Coconut pile", new DestroyableReplacementData(
                new ByProximity(typeof(CoconutPile), new Vector3(231.87f, 36.35f, 0f), 0.1f),
                Vector3.zero
            )},
            {"Beach - Seashell above dungeon", new DestroyableReplacementData(
                new ByName("Shell1"),
                Vector3.zero
            )},
            {"Cemetery - Under enemy", new DestroyableReplacementData(
                new ByName("Overworld 170 JarrerT2 S3"),
                Vector3.zero
            )},
            {"Cemetery - West pot", new DestroyableReplacementData(
                new ByName("OverworldDestroyable423"),
                Vector3.zero
            )},
            {"Crystal Grove Temple - West pot", new DestroyableReplacementData(
                new ByName("Temple1Destroyable28"),
                Vector3.zero
            )},
            {"Desert - Under ruins", new DestroyableReplacementData(
                new ByName("WreckShipSnakerHead"),
                Vector3.zero,
                new ByName("MapPickupDesert")
            )},
            {"Desert Temple - North East pot", new DestroyableReplacementData(
                new ByName("Temple2Destroyable72"),
                Vector3.zero
            )},
            {"Dungeon 1 - East behind debris", new DestroyableReplacementData(
                new ByName("Dungeon1Destroyable51"),
                Vector3.zero
            )},
            {"Dungeon 1 - Hidden in West Arena", new DestroyableReplacementData(
                new ByName("Dungeon1Destroyable1"),
                Vector3.zero
            )},
            {"Dungeon 2 - Secret room", new DestroyableReplacementData(
                new ByName("Dungeon2Destroyable111"),
                Vector3.zero
            )},
            {"Dungeon 2 - Treasure room", new DestroyableReplacementData(
                new ByName("Dungeon2Destroyable67"),
                Vector3.zero
            )},
            {"Dungeon 3 - East rock 1", new DestroyableReplacementData(
                new ByName("Dungeon3Destroyable32"),
                Vector3.zero
            )},
            {"Dungeon 3 - East rock 2", new DestroyableReplacementData(
                new ByName("Dungeon3Destroyable24"),
                Vector3.zero
            )},
            {"Dungeon 3 - Inside middle pot", new DestroyableReplacementData(
                new ByName("Dungeon3Destroyable58"),
                Vector3.zero,
                new ByName("Dungeon3CrystalKey4")
            )},
            {"Dungeon 3 - Pot Island", new DestroyableReplacementData(
                new ByName("Dungeon3Destroyable73"),
                Vector3.zero
            )},
            {"Forest - Bush behind tree", new DestroyableReplacementData(
                new ByName("BushCuttable (3)"),
                Vector3.zero
            )},
            {"Forest - Pot", new DestroyableReplacementData(
                new ByName("OverworldDestroyable394"),
                Vector3.zero
            )},
            {"Forest - Secret pond bush", new DestroyableReplacementData(
                new ByName("BushCuttable (2)"),
                Vector3.zero
            )},
            {"Green - Grove under ruins", new DestroyableReplacementData(
                new ByProximity(typeof(Destroyable), new Vector3(-51.9f, 9.5f, 0f), 0.1f, false),
                new Vector3(0f, -1.5f, 0f)
            )},
            {"Sewers - North pot room", new DestroyableReplacementData(
                new ByName("CaveDestroyable254"),
                Vector3.zero
            )},
            {"Sewers - South pot room", new DestroyableReplacementData(
                new ByName("CaveDestroyable253"),
                Vector3.zero
            )},
            {"Sunken City - North West pot", new DestroyableReplacementData(
                new ByName("OverworldDestroyable401"),
                Vector3.zero
            )},
            {"Sunken City - West bridge pot", new DestroyableReplacementData(
                new ByName("OverworldDestroyable396"),
                Vector3.zero
            )},
            {"Sunken Temple - Under the lilypad", new DestroyableReplacementData(
                new ByName("FlowerLilly (54)"),
                Vector3.zero
            )},
            {"Swamp - Hidden in plant", new DestroyableReplacementData(
                new ByProximity(typeof(PlantDestroyable), new Vector3(169.19f, 92.83f, 0f), 0.1f),
                Vector3.zero
            )},
            {"Swamp - Under rocks", new DestroyableReplacementData(
                new ByName("OverworldDestroyable369"),
                Vector3.zero,
                new ByName("OverworldHp4")
            )},
        };

        public Location CreateLocation(string name, string logicRule, LocationPool pool)
        {
            if (_pickupLocationToGameObjectName.ContainsKey(name))
            {
                return new PickupLocation(
                    name,
                    logicRule,
                    pool,
                    new ByName(_pickupLocationToGameObjectName[name], typeof(Pickup))
                );
            }
            if (_shardLocationToReplacementData.ContainsKey(name))
            {
                ShardLocationReplacementData replacementData = _shardLocationToReplacementData[name];

                return new ShardLocation(
                    name,
                    logicRule,
                    pool,
                    new UnityEngine.Vector3(
                        replacementData.X,
                        replacementData.Y,
                        replacementData.Z
                    )
                );
            }
            if (_crystalNpcLocationToGameObjectName.ContainsKey(name))
            {
                return new CrystalNpcLocation(
                    name,
                    logicRule,
                    pool,
                    new ByName(_crystalNpcLocationToGameObjectName[name], typeof(CrystalNpc))
                );
            }
            if (_linearShopLocationToReplacementData.ContainsKey(name))
            {
                LinearShopLocationReplacementData replacementData = _linearShopLocationToReplacementData[name];

                return new LinearShopLocation(
                    name,
                    logicRule,
                    pool,
                    replacementData.NpcName,
                    replacementData.DefaultPrice,
                    replacementData.DefaultCurrency
                );
            }
            if (_choiceShopLocationToReplacementData.ContainsKey(name))
            {
                ChoiceShopLocationReplacementData replacementData = _choiceShopLocationToReplacementData[name];

                return new ChoiceShopLocation(
                    name,
                    logicRule,
                    pool,
                    replacementData.NpcName,
                    replacementData.DefaultPrice,
                    replacementData.DefaultCurrency,
                    new ByName(replacementData.ReplacedGameObjectName, typeof(Buyable))
                );
            }
            if (_destroyableLocationToSelector.ContainsKey(name))
            {
                DestroyableReplacementData replacementData = _destroyableLocationToSelector[name];

                return new DestroyableLocation(
                    name,
                    logicRule,
                    pool,
                    replacementData.DestroyableSelector,
                    replacementData.Offset,
                    replacementData.ItemSelector ?? null
                );
            }
            if (pool == LocationPool.DungeonReward)
            {
                return new DungeonRewardLocation(name, logicRule, pool);
            }       

            throw new InvalidLocationException($"Location {name} have invalid data");
        }
    }
}
