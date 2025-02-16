using System;
using System.Collections.Generic;

namespace MinishootRandomizer;

public class InMemoryMarkerDataProvider : IMarkerDataProvider
{
    public List<MarkerData> GetMarkerDatas()
    {
        return new List<MarkerData>() {
            new MarkerData(new List<string> {
                "Abyss - Ambush Island",
            }, new List<Tuple<float, float>> {
                new Tuple<float, float>(-1.40f, -2.25f),
            }),
            new MarkerData(
                markerName: "Abyss Connector",
                locationNames: new List<string> {
                    "Abyss - Backroom item",
                    "Abyss Church - Unchosen statue",
                    "Abyss North Connector - Under ruins",
                },
                coordinates: new List<Tuple<float, float>> {
                    new Tuple<float, float>(-1.63f, -2.25f),
                    new Tuple<float, float>(-5.74f, -1.80f),
                    new Tuple<float, float>(-2.78f, 0.49f),
                    new Tuple<float, float>(-6.70f, -2.76f),
                },
                scarabMarkerData: new ScarabMarkerData(new Dictionary<string, string> {
                    { "CaveScarabPickup2", "Abyss North Connector - Under ruins" },
                })
            ),
            new MarkerData(
                locationNames: new List<string> {
                    "Abyss Race - Reward",
                },
                coordinates: new List<Tuple<float, float>> {
                    new Tuple<float, float>(-1.55f, -4.03f),
                },
                spiritMarkerData: new SpiritMarkerData(
                    "Abyss Race - Reward", "NpcTiny3"
                )
            ),
            new MarkerData(new List<string> {
                "Abyss - Near dungeon entrance",
            }, new List<Tuple<float, float>> {
                new Tuple<float, float>(-2.52f, -1.49f),
            }),
            new MarkerData(new List<string> {
                "Abyss - Near protected enemy",
            }, new List<Tuple<float, float>> {
                new Tuple<float, float>(-3.52f, -1.98f),
            }),
            new MarkerData(new List<string> {
                "Abyss - South of spinning enemy",
            }, new List<Tuple<float, float>> {
                new Tuple<float, float>(-2.81f, -2.04f),
            }),
            new MarkerData(new List<string> {
                "Abyss - Village Entrance",
            }, new List<Tuple<float, float>> {
                new Tuple<float, float>(-4.81f, -3.10f),
            }),
            new MarkerData(new List<string> {
                "Abyss - Within Crystal Grove",
            }, new List<Tuple<float, float>> {
                new Tuple<float, float>(-2.77f, 0.10f),
            }),
            new MarkerData(new List<string> {
                "Abyss Ruined shop - Item",
            }, new List<Tuple<float, float>> {
                new Tuple<float, float>(-1.92f, -1.38f),
            }),
            new MarkerData(
                markerName: "Abyss Shack",
                locationNames: new List<string> {
                    "Abyss Shack - Hidden corridor",
                    "Abyss Shack - Hidden room",
                    "Abyss Shack - Under pot",
                },
                coordinates: new List<Tuple<float, float>> {
                    new Tuple<float, float>(-6.57f, -3.16f),
                },
                scarabMarkerData: new ScarabMarkerData(new Dictionary<string, string> {
                    { "CaveScarabPickup3", "Abyss Shack - Under pot" },
                })
            ),
            new MarkerData(
                markerName: "Abyss Tower",
                locationNames: new List<string> {
                    "Abyss Tower - Top of tower",
                },
                coordinates: new List<Tuple<float, float>> {
                    new Tuple<float, float>(-4.83f, -2.41f),
                },
                npcMarkerData: new NpcMarkerData(
                    "Abyss Tower - Top of tower", NpcIds.Familly2.Str()
                )
            ),
            new MarkerData(new List<string> {
                "Beach - Coast Hidden by plants",
            }, new List<Tuple<float, float>> {
                new Tuple<float, float>(4.25f, -0.36f),
            }),
            new MarkerData(new List<string> {
                "Beach - Coast North hidden alcove",
            }, new List<Tuple<float, float>> {
                new Tuple<float, float>(4.48f, 0.30f),
            }),
            new MarkerData(new List<string> {
                "Beach - Coast South hidden alcove",
            }, new List<Tuple<float, float>> {
                new Tuple<float, float>(3.05f, -1.19f),
            }),
            new MarkerData(
                locationNames: new List<string> {
                    "Beach - Coconut pile",
                },
                coordinates: new List<Tuple<float, float>> {
                    new Tuple<float, float>(6.93f, 0.78f),
                },
                scarabMarkerData: new ScarabMarkerData(new Dictionary<string, string> {
                    { "OverworldScarabPickup6", "Beach - Coconut pile" },
                })
            ),
            new MarkerData(new List<string> {
                "Beach - East Island",
            }, new List<Tuple<float, float>> {
                new Tuple<float, float>(7.09f, -1.10f),
            }),
            new MarkerData(new List<string> {
                "Beach - Protected item",
            }, new List<Tuple<float, float>> {
                new Tuple<float, float>(5.51f, -0.95f),
            }),
            new MarkerData(
                locationNames: new List<string> {
                    "Beach - Seashell above dungeon",
                },
                coordinates: new List<Tuple<float, float>> {
                    new Tuple<float, float>(3.96f, -1.67f),
                },
                scarabMarkerData: new ScarabMarkerData(new Dictionary<string, string> {
                    { "OverworldScarabPickup5", "Beach - Seashell above dungeon" },
                })
            ),
            new MarkerData(new List<string> {
                "Beach - South East Island",
            }, new List<Tuple<float, float>> {
                new Tuple<float, float>(6.23f, -2.43f),
            }),
            new MarkerData(
                locationNames: new List<string> {
                    "Beach Race - Reward",
                },
                coordinates: new List<Tuple<float, float>> {
                    new Tuple<float, float>(6.62f, 0.86f),
                },
                spiritMarkerData: new SpiritMarkerData(
                    "Beach Race - Reward", "NpcTiny7"
                )
            ),
            new MarkerData(new List<string> {
                "Cemetery - Crying house",
            }, new List<Tuple<float, float>> {
                new Tuple<float, float>(0.31f, 1.31f),
            }),
            new MarkerData(new List<string> {
                "Cemetery - Under enemy",
            }, new List<Tuple<float, float>> {
                new Tuple<float, float>(-0.88f, 2.39f),
            }),
            new MarkerData(
                locationNames: new List<string> {
                    "Cemetery - West pot",
                },
                coordinates: new List<Tuple<float, float>> {
                    new Tuple<float, float>(-1.53f, 2.25f),
                },
                scarabMarkerData: new ScarabMarkerData(new Dictionary<string, string> {
                    { "OverworldScarabPickup7", "Cemetery - West pot" },
                })
            ),
            new MarkerData(
                markerName: "Cemetery Tower",
                locationNames: new List<string> {
                    "Cemetery Tower - Top of tower",
                    "Scarab Temple - After race 1",
                    "Scarab Temple - After race 2",
                    "Scarab Temple - After race 3",
                    "Scarab Temple - Race Reward",
                },
                coordinates: new List<Tuple<float, float>> {
                    new Tuple<float, float>(1.90f, 3.85f),
                },
                spiritMarkerData: new SpiritMarkerData(
                    "Scarab Temple - Race Reward", "NpcTiny1"
                )
            ),
            new MarkerData(
                markerName: "Crystal Grove Temple",
                locationNames: new List<string> {
                    "Crystal Grove Temple - Boss reward",
                    "Crystal Grove Temple - Dodge the east cannons",
                    "Crystal Grove Temple - East tunnels",
                    "Crystal Grove Temple - North east hidden room",
                    "Crystal Grove Temple - South West Hidden pond",
                    "Crystal Grove Temple - West pot"
                },
                coordinates: new List<Tuple<float, float>> {
                    new Tuple<float, float>(-2.30f, 0.68f),
                },
                scarabMarkerData: new ScarabMarkerData(new Dictionary<string, string> {
                    { "Temple1ScarabPickup0", "Crystal Grove Temple - West pot" },
                })
            ),
            new MarkerData(
                markerName: "Crystal Grove Tower",
                locationNames: new List<string> {
                    "Crystal Grove Tower - Top of tower",
                },
                coordinates: new List<Tuple<float, float>> {
                    new Tuple<float, float>(-2.27f, 1.60f),
                },
                npcMarkerData: new NpcMarkerData(
                    "Crystal Grove Tower - Top of tower", NpcIds.ScarabCollector.Str()
                )
            ),
            new MarkerData(new List<string> {
                "Desert - North east platforms",
            }, new List<Tuple<float, float>> {
                new Tuple<float, float>(-2.76f, 2.82f),
            }),
            new MarkerData(new List<string> {
                "Desert - On the river",
            }, new List<Tuple<float, float>> {
                new Tuple<float, float>(-5.02f, 1.57f),
            }),
            new MarkerData(new List<string> {
                "Desert - Under ruins",
            }, new List<Tuple<float, float>> {
                new Tuple<float, float>(-3.94f, 1.64f),
            }),
            new MarkerData(
                markerName: "Desert Grotto",
                locationNames: new List<string> {
                    "Desert Grotto - Both torches lighted",
                },
                coordinates: new List<Tuple<float, float>> {
                    new Tuple<float, float>(-5.05f, -0.55f),
                    new Tuple<float, float>(-5.98f, -0.05f),
                    new Tuple<float, float>(-6.91f, -0.53f),
                },
                npcMarkerData: new NpcMarkerData(
                    "Desert Grotto - Both torches lighted", NpcIds.Familly1.Str()
                )
            ),
            new MarkerData(
                locationNames: new List<string> {
                    "Desert Race - Reward",
                },
                coordinates: new List<Tuple<float, float>> {
                    new Tuple<float, float>(-6.87f, 1.74f),
                },
                spiritMarkerData: new SpiritMarkerData(
                    "Desert Race - Reward", "NpcTiny5"
                )
            ),
            new MarkerData(
                markerName: "Desert Temple",
                locationNames: new List<string> {
                    "Desert Temple - Boss reward",
                    "Desert Temple - North East pot",
                    "Desert Temple - Secret room"
                },
                coordinates: new List<Tuple<float, float>> {
                    new Tuple<float, float>(-5.95f, 0.98f),
                },
                scarabMarkerData: new ScarabMarkerData(new Dictionary<string, string> {
                    { "Temple2ScarabPickup0", "Desert Temple - North East pot" },
                })
            ),
            new MarkerData(
                markerName: "Dungeon 1",
                locationNames: new List<string> {
                    "Dungeon 1 - Central item",
                    "Dungeon 1 - Crystal near east armored spinner",
                    "Dungeon 1 - Entrance after south ramp",
                    "Dungeon 1 - Entrance East Arena",
                    "Dungeon 1 - Entrance west bridge",
                    "Dungeon 1 - Far West Arena after spinner",
                    "Dungeon 1 - Hidden below crystal wall",
                    "Dungeon 1 - Hidden in West Arena",
                    "Dungeon 1 - Inside the crystal wall",
                    "Dungeon 1 - Near boss",
                    "Dungeon 1 - Near east armored spinner",
                    "Dungeon 1 - Near west armored spinner",
                    "Dungeon 1 - North West Arena",
                    "Dungeon 1 - Platform after crystal wall",
                    "Dungeon 1 - South item",
                    "Dungeon 1 - West bridge hidden item",
                },
                coordinates: new List<Tuple<float, float>> {
                    new Tuple<float, float>(1.54f, 2.30f),
                },
                objectiveMarkerData: new ObjectiveMarkerData(
                    "Dungeon 1 - Dungeon reward", Goals.Dungeon5
                )
            ),
            new MarkerData(
                markerName: "Dungeon 2",
                locationNames: new List<string> {
                    "Dungeon 2 - Central item",
                    "Dungeon 2 - Hidden by plants",
                    "Dungeon 2 - Item after jumps",
                    "Dungeon 2 - North east beyond arena",
                    "Dungeon 2 - North item",
                    "Dungeon 2 - North west arena",
                    "Dungeon 2 - Secret room",
                    "Dungeon 2 - South west arena",
                    "Dungeon 2 - Treasure room",
                    "Dungeon 2 - Treasure room entrance",
                    "Dungeon 2 - Walled arena extra",
                    "Dungeon 2 - Walled arena item",
                    "Dungeon 2 - West arena",
                    "Dungeon 2 - West arena extra",
                },
                coordinates: new List<Tuple<float, float>> {
                    new Tuple<float, float>(-2.76f, -0.56f),
                },
                scarabMarkerData: new ScarabMarkerData(new Dictionary<string, string> {
                    { "Dungeon2ScarabPickup0", "Dungeon 2 - Secret room" },
                }),
                objectiveMarkerData: new ObjectiveMarkerData(
                    "Dungeon 2 - Dungeon reward", Goals.Dungeon5
                )
            ),
            new MarkerData(
                markerName: "Dungeon 3",
                locationNames: new List<string> {
                    "Dungeon 3 - Behind North West doors",
                    "Dungeon 3 - Central Item",
                    "Dungeon 3 - East Island",
                    "Dungeon 3 - East rock 1",
                    "Dungeon 3 - East rock 2",
                    "Dungeon 3 - East wall",
                    "Dungeon 3 - Hidden Tunnel",
                    "Dungeon 3 - Inside middle pot",
                    "Dungeon 3 - Item protected by spikes",
                    "Dungeon 3 - North arena",
                    "Dungeon 3 - Over the pit",
                    "Dungeon 3 - Pot Island",
                    "Dungeon 3 - Race on the water",
                    "Dungeon 3 - South corridor",
                    "Dungeon 3 - South of torches",
                    "Dungeon 3 - South west of torches",
                },
                coordinates: new List<Tuple<float, float>> {
                    new Tuple<float, float>(4.46f, -2.84f),
                },
                scarabMarkerData: new ScarabMarkerData(new Dictionary<string, string> {
                    { "Dungeon3ScarabPickup0", "Dungeon 3 - East rock 2" },
                    { "Dungeon3ScarabPickup1", "Dungeon 3 - Pot Island" },
                }),
                objectiveMarkerData: new ObjectiveMarkerData(
                    "Dungeon 3 - Dungeon reward", Goals.Dungeon5
                )
            ),
            new MarkerData(
                markerName: "Dungeon 4",
                locationNames: new List<string> {
                    // nothing
                },
                coordinates: new List<Tuple<float, float>> {
                    new Tuple<float, float>(-3.76f, 5.36f),
                },
                objectiveMarkerData: new ObjectiveMarkerData(
                    "Dungeon 4 - Dungeon reward", Goals.Dungeon5
                )
            ),
            new MarkerData(
                markerName: "Dungeon 5",
                locationNames: new List<string> {
                    // nothing
                },
                coordinates: new List<Tuple<float, float>> {
                    new Tuple<float, float>(0.25f, 6.70f),
                },
                objectiveMarkerData: new ObjectiveMarkerData(
                    "Dungeon 5 - Beat the boss", Goals.Dungeon5
                )
            ),
            new MarkerData(new List<string> {
                "Dungeon 5 - Central Item",
            }, new List<Tuple<float, float>> {
                new Tuple<float, float>(1.69f, 7.10f),
                new Tuple<float, float>(-1.12f, 7.13f),
            }),
            new MarkerData(
                markerName: "Family House Cave",
                locationNames: new List<string> {
                    "Family House Cave - Before shortcut",
                    "Family House Cave - Hidden Tunnel",
                    "Family House Cave - Near button",
                    "Family House Cave - Near tree",
                    "Family House Cave - Reunited Family",
                    "Family House Cave - Sewers"
                },
                coordinates: new List<Tuple<float, float>> {
                    new Tuple<float, float>(-0.13f, 0.62f),
                    new Tuple<float, float>(-3.76f, 0.49f),
                    new Tuple<float, float>(-3.77f, -0.44f),
                }
            ),
            new MarkerData(new List<string> {
                "Forest - Boss Reward",
            }, new List<Tuple<float, float>> {
                new Tuple<float, float>(1.46f, -4.21f),
            }),
            new MarkerData(new List<string> {
                "Forest - Bush behind tree",
            }, new List<Tuple<float, float>> {
                new Tuple<float, float>(-0.24f, -1.29f),
            }),
            new MarkerData(new List<string> {
                "Forest - Enemy tree near Beach",
            }, new List<Tuple<float, float>> {
                new Tuple<float, float>(2.47f, -3.35f),
            }),
            new MarkerData(
                locationNames: new List<string> {
                    "Forest - Far South East",
                },
                coordinates: new List<Tuple<float, float>> {
                    new Tuple<float, float>(2.63f, -7.77f),
                },
                npcMarkerData: new NpcMarkerData(
                    "Forest - Far South East", NpcIds.Bard.Str()
                )
            ),
            new MarkerData(new List<string> {
                "Forest - Faraway island item",
                "Forest - Faraway island extra"
            }, new List<Tuple<float, float>> {
                new Tuple<float, float>(-2.48f, -7.57f),
            }),
            new MarkerData(new List<string> {
                "Forest - Hidden east tunnel",
            }, new List<Tuple<float, float>> {
                new Tuple<float, float>(1.54f, -5.38f),
            }),
            new MarkerData(
                locationNames: new List<string> {
                    "Forest - Pot",
                },
                coordinates: new List<Tuple<float, float>> {
                    new Tuple<float, float>(2.57f, -4.01f),
                },
                scarabMarkerData: new ScarabMarkerData(new Dictionary<string, string> {
                    { "OverworldScarabPickup1", "Forest - Pot" },
                })
            ),
            new MarkerData(new List<string> {
                "Forest - Secret pond behind bushes",
            }, new List<Tuple<float, float>> {
                new Tuple<float, float>(0.28f, -2.98f),
            }),
            new MarkerData(
                locationNames: new List<string> {
                    "Forest - Secret pond bush",
                },
                coordinates: new List<Tuple<float, float>> {
                    new Tuple<float, float>(0.52f, -1.46f),
                },
                scarabMarkerData: new ScarabMarkerData(new Dictionary<string, string> {
                    { "OverworldScarabPickup0", "Forest - Secret pond bush" },
                })
            ),
            new MarkerData(new List<string> {
                "Forest - Secret within Secret",
            }, new List<Tuple<float, float>> {
                new Tuple<float, float>(0.86f, -0.86f),
            }),
            new MarkerData(new List<string> {
                "Forest - Tunnel below big tree enemy",
            }, new List<Tuple<float, float>> {
                new Tuple<float, float>(-1.37f, -4.58f),
            }),
            new MarkerData(
                markerName: "Forest Grotto",
                locationNames: new List<string> {
                    "Forest Grotto - After ramp",
                },
                coordinates: new List<Tuple<float, float>> {
                    new Tuple<float, float>(0.98f, -2.70f),
                    new Tuple<float, float>(-0.02f, -0.83f),
                },
                npcMarkerData: new NpcMarkerData("Forest Grotto - After ramp", NpcIds.Explorer.Str())
            ),
            new MarkerData(
                markerName: "Forest Shop",
                locationNames: new List<string> {
                    "Forest Shop 1",
                    "Forest Shop 2",
                    "Forest Shop 3",
                    "Forest Shop Race - Reward"
                },
                coordinates: new List<Tuple<float, float>> {
                    new Tuple<float, float>(-0.54f, -2.96f),
                },
                spiritMarkerData: new SpiritMarkerData(
                    "Forest Shop Race - Reward", "NpcTiny2"
                )
            ),
            new MarkerData(new List<string> {
                "Green - Behind Closed Arena",
            }, new List<Tuple<float, float>> {
                new Tuple<float, float>(2.40f, -0.98f),
            }),
            new MarkerData(new List<string> {
                "Green - Bridge Shortcut",
            }, new List<Tuple<float, float>> {
                new Tuple<float, float>(1.77f, 0.77f),
            }),
            new MarkerData(new List<string> {
                "Green - Button Item",
            }, new List<Tuple<float, float>> {
                new Tuple<float, float>(2.26f, 1.65f),
            }),
            new MarkerData(new List<string> {
                "Green - Closed Arena Item",
            }, new List<Tuple<float, float>> {
                new Tuple<float, float>(2.42f, -0.77f),
            }),
            new MarkerData(new List<string> {
                "Green - Crossroad Arena Item",
            }, new List<Tuple<float, float>> {
                new Tuple<float, float>(2.89f, 0.82f),
            }),
            new MarkerData(new List<string> {
                "Green - Forest Entrance",
            }, new List<Tuple<float, float>> {
                new Tuple<float, float>(1.52f, -1.10f),
            }),
            new MarkerData(new List<string> {
                "Green - Grove near button",
            }, new List<Tuple<float, float>> {
                new Tuple<float, float>(-0.57f, -0.11f),
            }),
            new MarkerData(new List<string> {
                "Green - Grove under ruins",
            }, new List<Tuple<float, float>> {
                new Tuple<float, float>(-1.14f, -0.05f),
            }),
            new MarkerData(new List<string> {
                "Green - Hidden before Island Arena",
            }, new List<Tuple<float, float>> {
                new Tuple<float, float>(3.71f, -0.06f),
            }),
            new MarkerData(new List<string> {
                "Green - Island Arena Item",
            }, new List<Tuple<float, float>> {
                new Tuple<float, float>(3.71f, 2.05f),
            }),
            new MarkerData(new List<string> {
                "Green - Outside Dungeon 1 Cave",
            }, new List<Tuple<float, float>> {
                new Tuple<float, float>(1.54f, 1.36f),
            }),
            new MarkerData(new List<string> {
                "Green - River near Forest Entrance",
            }, new List<Tuple<float, float>> {
                new Tuple<float, float>(-1.25f, -1.11f),
            }),
            new MarkerData(
                locationNames: new List<string> {
                    "Green - Shortcut to Town Pillars",
                },
                coordinates: new List<Tuple<float, float>> {
                    new Tuple<float, float>(-1.34f, 0.82f),
                },
                npcMarkerData: new NpcMarkerData(
                    "Green - Shortcut to Town Pillars", NpcIds.MercantHub.Str()
                )
            ),
            new MarkerData(
                markerName: "Green Grotto",
                locationNames: new List<string> {
                    "Green Grotto - Before race",
                    "Green Grotto - Corner",
                    "Green Grotto - Drop",
                    "Green Grotto - Race Reward"
                },
                coordinates: new List<Tuple<float, float>> {
                    new Tuple<float, float>(2.39f, -0.09f),
                    new Tuple<float, float>(1.36f, -0.69f),
                    new Tuple<float, float>(2.33f, -1.50f),
                },
                spiritMarkerData: new SpiritMarkerData(
                    "Green Grotto - Race Reward", "NpcTiny0"
                )
            ),
            new MarkerData(
                markerName: "Jar Shop",
                locationNames: new List<string> {
                    "Jar Shop 1",
                    "Jar Shop 2",
                    "Jar Shop 3",
                },
                coordinates: new List<Tuple<float, float>> {
                    new Tuple<float, float>(-1.54f, 0.40f),
                }
            ),
            new MarkerData(new List<string> {
                "Junkyard - East pond",
            }, new List<Tuple<float, float>> {
                new Tuple<float, float>(1.79f, 5.54f),
            }),
            new MarkerData(new List<string> {
                "Junkyard - Inside Sunken City",
            }, new List<Tuple<float, float>> {
                new Tuple<float, float>(-2.74f, 4.55f),
            }),
            new MarkerData(new List<string> {
                "Junkyard - South East",
            }, new List<Tuple<float, float>> {
                new Tuple<float, float>(0.88f, 3.57f),
            }),
            new MarkerData(new List<string> {
                "Junkyard East Shack - Item",
            }, new List<Tuple<float, float>> {
                new Tuple<float, float>(1.25f, 5.32f),
            }),
            new MarkerData(new List<string> {
                "Junkyard West Shack - Item",
            }, new List<Tuple<float, float>> {
                new Tuple<float, float>(-1.56f, 6.10f),
            }),
            new MarkerData(new List<string> {
                "Primordial Cave - Meet the Primordial Scarab",
            }, new List<Tuple<float, float>> {
                new Tuple<float, float>(-3.75f, 7.19f),
            }),
            new MarkerData(
                markerName: "Scarab Temple",
                locationNames: new List<string> {
                    "Scarab Temple - Backroom",
                    "Scarab Temple - Central Item",
                    "Scarab Temple - East side",
                    "Scarab Temple - Middle Entrance"
                },
                coordinates: new List<Tuple<float, float>> {
                    new Tuple<float, float>(0.53f, 1.02f),
                    new Tuple<float, float>(2.31f, 1.36f),
                    new Tuple<float, float>(2.86f, 1.97f),
                    new Tuple<float, float>(2.85f, 2.76f),
                },
                npcMarkerData: new NpcMarkerData(
                    "Scarab Temple - Backroom", NpcIds.Academician.Str()
                )
            ),
            new MarkerData(
                markerName: "Scarab Temple Drop",
                locationNames: new List<string> {
                    "Scarab Temple - Bottom Left Torch Item",
                    "Swamp Jumps Grotto - Drop"
                },
                coordinates: new List<Tuple<float, float>> {
                    new Tuple<float, float>(0.94f, 2.70f),
                }
            ),
            new MarkerData(
                markerName: "Sewers",
                locationNames: new List<string> {
                    "Sewers - Behind West Entrance",
                    "Sewers - Central room boss reward",
                    "Sewers - Central room corner",
                    "Sewers - Drop",
                    "Sewers - Near Family House Cave",
                    "Sewers - North pot room",
                    "Sewers - South pot room",
                    "Sunken City - Inside the walls"
                },
                coordinates: new List<Tuple<float, float>> {
                    new Tuple<float, float>(-4.81f, 2.88f),
                    new Tuple<float, float>(-5.93f, 3.58f),
                    new Tuple<float, float>(-7.52f, 3.76f),
                    new Tuple<float, float>(-7.01f, 4.53f),
                    new Tuple<float, float>(-5.31f, 5.85f),
                    new Tuple<float, float>(-4.03f, 4.83f),
                    new Tuple<float, float>(-3.48f, 4.83f),
                },
                scarabMarkerData: new ScarabMarkerData(new Dictionary<string, string> {
                    { "CaveScarabPickup0", "Sewers - South pot room" },
                    { "CaveScarabPickup1", "Sewers - North pot room" },
                })
            ),
            new MarkerData(
                markerName: "Snow",
                locationNames: new List<string> {
                    // nothing
                },
                coordinates: new List<Tuple<float, float>> {
                    new Tuple<float, float>(1.27f, -7.28f),
                },
                objectiveMarkerData: new ObjectiveMarkerData(
                    "Snow - Beat the Unchosen", Goals.Snow
                )
            ),
            new MarkerData(new List<string> {
                "Spirit Tower - Item",
            }, new List<Tuple<float, float>> {
                new Tuple<float, float>(0.29f, -4.48f),
            }),
            new MarkerData(new List<string> {
                "Sunken City - Below West bridge",
            }, new List<Tuple<float, float>> {
                new Tuple<float, float>(-4.42f, 4.31f),
            }),
            new MarkerData(new List<string> {
                "Sunken City - Inside the walls",
            }, new List<Tuple<float, float>> {
                new Tuple<float, float>(-3.75f, 4.28f),
            }),
            new MarkerData(new List<string> {
                "Sunken City - Near West entrance",
            }, new List<Tuple<float, float>> {
                new Tuple<float, float>(-4.40f, 3.05f),
            }),
            new MarkerData(new List<string> {
                "Sunken City - North bridge",
            }, new List<Tuple<float, float>> {
                new Tuple<float, float>(-3.33f, 5.54f),
            }),
            new MarkerData(new List<string> {
                "Sunken City - North East district",
            }, new List<Tuple<float, float>> {
                new Tuple<float, float>(-3.26f, 5.36f),
            }),
            new MarkerData(
                locationNames: new List<string> {
                    "Sunken City - North West pot",
                },
                coordinates: new List<Tuple<float, float>> {
                    new Tuple<float, float>(-7.76f, 7.39f),
                },
                scarabMarkerData: new ScarabMarkerData(new Dictionary<string, string> {
                    { "OverworldScarabPickup4", "Sunken City - North West pot" },
                })
            ),
            new MarkerData(new List<string> {
                "Sunken City - South West building item",
            }, new List<Tuple<float, float>> {
                new Tuple<float, float>(-7.52f, 3.60f),
            }),
            new MarkerData(new List<string> {
                "Sunken City - West bridge",
            }, new List<Tuple<float, float>> {
                new Tuple<float, float>(-4.57f, 4.62f),
            }),
            new MarkerData(
                locationNames: new List<string> {
                    "Sunken City - West bridge pot",
                },
                coordinates: new List<Tuple<float, float>> {
                    new Tuple<float, float>(-4.55f, 4.80f),
                },
                scarabMarkerData: new ScarabMarkerData(new Dictionary<string, string> {
                    { "OverworldScarabPickup3", "Sunken City - West bridge pot" },
                })
            ),
            new MarkerData(new List<string> {
                "Sunken City Building - Drop",
            }, new List<Tuple<float, float>> {
                new Tuple<float, float>(-3.03f, 4.29f),
                new Tuple<float, float>(-3.03f, 5.16f),
                new Tuple<float, float>(-3.29f, 5.08f),
            }),
            new MarkerData(
                locationNames: new List<string> {
                    "Sunken City Race - Reward",
                },
                coordinates: new List<Tuple<float, float>> {
                    new Tuple<float, float>(-2.68f, 3.46f),
                },
                spiritMarkerData: new SpiritMarkerData(
                    "Sunken City Race - Reward", "NpcTiny6"
                )
            ),
            new MarkerData(
                markerName: "Sunken Temple",
                locationNames: new List<string> {
                    "Sunken Temple - Boss reward",
                    "Sunken Temple - Entrance",
                    "Sunken Temple - Secret tunnel",
                    "Sunken Temple - South West tunnel",
                    "Sunken Temple - Under the lilypad"
                },
                coordinates: new List<Tuple<float, float>> {
                    new Tuple<float, float>(-5.95f, 4.85f),
                },
                scarabMarkerData: new ScarabMarkerData(new Dictionary<string, string> {
                    { "Temple3ScarabPickup0", "Sunken Temple - Under the lilypad" },
                })
            ),
            new MarkerData(new List<string> {
                "Swamp - Blocked tunnel",
            }, new List<Tuple<float, float>> {
                new Tuple<float, float>(5.92f, 3.35f),
            }),
            new MarkerData(new List<string> {
                "Swamp - Hidden Before Big Enemy",
            }, new List<Tuple<float, float>> {
                new Tuple<float, float>(3.46f, 4.31f),
            }),
            new MarkerData(
                locationNames: new List<string> {
                    "Swamp - Hidden in plant",
                },
                coordinates: new List<Tuple<float, float>> {
                    new Tuple<float, float>(5.17f, 2.40f),
                },
                scarabMarkerData: new ScarabMarkerData(new Dictionary<string, string> {
                    { "OverworldScarabPickup2", "Swamp - Hidden in plant" },
                })
            ),
            new MarkerData(new List<string> {
                "Swamp - Near cracked wall",
            }, new List<Tuple<float, float>> {
                new Tuple<float, float>(5.03f, 5.11f),
            }),
            new MarkerData(new List<string> {
                "Swamp - North item",
            }, new List<Tuple<float, float>> {
                new Tuple<float, float>(5.26f, 7.10f),
            }),
            new MarkerData(new List<string> {
                "Swamp - South West Island Hidden in trees Item",
            }, new List<Tuple<float, float>> {
                new Tuple<float, float>(2.20f, 3.02f),
            }),
            new MarkerData(new List<string> {
                "Swamp - Under rocks",
            }, new List<Tuple<float, float>> {
                new Tuple<float, float>(2.95f, 4.52f),
            }),
            new MarkerData(
                locationNames: new List<string> {
                    "Swamp Race - Reward",
                },
                coordinates: new List<Tuple<float, float>> {
                    new Tuple<float, float>(7.61f, 2.58f),
                },
                spiritMarkerData: new SpiritMarkerData(
                    "Swamp Race - Reward", "NpcTiny4"
                )
            ),
            new MarkerData(
                markerName: "Swamp Shop",
                locationNames: new List<string> {
                    "Swamp Shop 1",
                    "Swamp Shop 2",
                    "Swamp Shop 3",
                    "Swamp Shop Extra",
                },
                coordinates: new List<Tuple<float, float>> {
                    new Tuple<float, float>(4.50f, 4.44f),
                }
            ),
            new MarkerData(
                markerName: "Swamp Tower",
                locationNames: new List<string> {
                    "Swamp Tower - Top of tower",
                },
                coordinates: new List<Tuple<float, float>> {
                    new Tuple<float, float>(3.73f, 2.87f),
                },
                npcMarkerData: new NpcMarkerData(
                    "Swamp Tower - Top of tower", NpcIds.Familly3.Str()
                )
            ),
            new MarkerData(
                markerName: "Town",
                locationNames: new List<string> {
                    "Town - Blacksmith Item 1",
                    "Town - Blacksmith Item 2",
                    "Town - Blacksmith Item 3",
                    "Town - Blacksmith Item 4",
                    "Town - Mercant Item 1",
                    "Town - Mercant Item 2",
                    "Town - Mercant Item 3",
                    "Town - Plaza",
                    "Town - Scarab Collector Item 1",
                    "Town - Scarab Collector Item 2",
                    "Town - Scarab Collector Item 3",
                    "Town - Scarab Collector Item 4",
                    "Town - Scarab Collector Item 5",
                    "Town - Scarab Collector Item 6",
                    "Starting Grotto - Entrance",
                    "Starting Grotto - North Corridor",
                    "Starting Grotto - Secret Wall",
                    "Starting Grotto - West Item",
                },
                coordinates: new List<Tuple<float, float>> {
                    new Tuple<float, float>(0.28f, 0.64f),
                },
                npcMarkerData: new NpcMarkerData(
                    "Town - Plaza", NpcIds.Blacksmith.Str()
                )
            ),
            new MarkerData(new List<string> {
                "Town Pillars - Hidden below bridge"
            }, new List<Tuple<float, float>> {
                new Tuple<float, float>(-0.40f, 0.28f),
            }),
            new MarkerData(new List<string> {
                "Town Pillars - Hidden Pond"
            }, new List<Tuple<float, float>> {
                new Tuple<float, float>(-0.88f, 0.30f),
            }),
            new MarkerData(new List<string> {
                "Town Pillars Grotto - Reward"
            }, new List<Tuple<float, float>> {
                new Tuple<float, float>(-0.89f, 1.19f),
            }),
            new MarkerData(
                markerName: "Zelda 1 Grotto",
                locationNames: new List<string> {
                    "Zelda 1 Grotto - Behind the closed doors"
                },
                coordinates: new List<Tuple<float, float>> {
                    new Tuple<float, float>(0.08f, 0.00f),
                },
                npcMarkerData: new NpcMarkerData(
                    "Zelda 1 Grotto - Behind the closed doors", NpcIds.Healer.Str()
                )
            ),
        };
    }
}
