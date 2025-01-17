using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MinishootRandomizer;

public class DictionaryItemFactory : IItemFactory
{
    private static Dictionary<string, PickupItemData> _pickupItemData = new()
    {
        {Item.ProgressiveCannon, new PickupItemData(Stats.BulletNumber)},
        {Item.HpCrystalShard, new PickupItemData(Stats.Hp)},
        {Item.EnergyCrystalShard, new PickupItemData(Stats.Energy)},
        {Item.PowerOfProtection, new PickupItemData(Stats.PowerBombLevel)},
        {Item.PowerOfSpirits, new PickupItemData(Stats.PowerAllyLevel)},
        {Item.PowerOfTime, new PickupItemData(Stats.PowerSlowLevel)},
    };

    private static Dictionary<string, SkillItemData> _skillItemData = new()
    {
        {Item.Boost, new SkillItemData(Skill.Boost)},
        {Item.Dash, new SkillItemData(Skill.Dash)},
        {Item.Supershot, new SkillItemData(Skill.Supershot)},
        {Item.Surf, new SkillItemData(Skill.Hover)},
    };

    private static Dictionary<string, ModuleItemData> _moduleItemData = new()
    {
        {Item.AdvancedEnergy, new ModuleItemData(Modules.BoostCost)},
        {Item.AncientAstrolabe, new ModuleItemData(Modules.CollectableScan)},
        {Item.Compass, new ModuleItemData(Modules.Compass)},
        {Item.CrystalBullet, new ModuleItemData(Modules.BlueBullet)},
        {Item.EnchantedHeart, new ModuleItemData(Modules.HearthCrystal)},
        {Item.EnchantedPowers, new ModuleItemData(Modules.FreePower)},
        {Item.IdolOfProtection, new ModuleItemData(Modules.IdolBomb)},
        {Item.IdolOfSpirits, new ModuleItemData(Modules.IdolAlly)},
        {Item.IdolOfTime, new ModuleItemData(Modules.IdolSlow)},
        {Item.LuckyHeart, new ModuleItemData(Modules.HpDrop)},
        {Item.Overcharge, new ModuleItemData(Modules.Overcharge)},
        {Item.PrimordialCrystal, new ModuleItemData(Modules.PrimordialCrystal)},
        {Item.RestorationEnhancer, new ModuleItemData(Modules.XpGain)},
        {Item.SpiritDash, new ModuleItemData(Modules.SpiritDash)},
        {Item.VengefulTalisman, new ModuleItemData(Modules.Retaliation)},
        {Item.VillageStar, new ModuleItemData(Modules.Teleport)},
        {Item.WoundedHeart, new ModuleItemData(Modules.Rage)},
    };

    private static Dictionary<string, NpcItemData> _npcItemData = new()
    {
        {Item.Academician, new NpcItemData(NpcIds.Academician)},
        {Item.Bard, new NpcItemData(NpcIds.Bard)},
        {Item.Blacksmith, new NpcItemData(NpcIds.Blacksmith)},
        {Item.Explorer, new NpcItemData(NpcIds.Explorer)},
        {Item.FamilyChild, new NpcItemData(NpcIds.Familly3)},
        {Item.FamilyParent1, new NpcItemData(NpcIds.Familly1)},
        {Item.FamilyParent2, new NpcItemData(NpcIds.Familly2)},
        {Item.Healer, new NpcItemData(NpcIds.Healer)},
        {Item.ScarabCollector, new NpcItemData(NpcIds.ScarabCollector)},
        {Item.Mercant, new NpcItemData(NpcIds.MercantHub)},
    };

    private static Dictionary<string, MapItemData> _mapItemData = new()
    {
        {Item.AbyssMap, new MapItemData(MapRegion.Abyss)},
        {Item.BeachMap, new MapItemData(MapRegion.Beach)},
        {Item.BlueForestMap, new MapItemData(MapRegion.Forest)},
        {Item.DesertMap, new MapItemData(MapRegion.Desert)},
        {Item.GreenMap, new MapItemData(MapRegion.Green)},
        {Item.JunkyardMap, new MapItemData(MapRegion.Junkyard)},
        {Item.SunkenCityMap, new MapItemData(MapRegion.SunkenCity)},
        {Item.SwampMap, new MapItemData(MapRegion.Swamp)},
    };
        
    private static Dictionary<string, KeyItemData> _keyItemData = new()
    {
        {Item.DarkHeart, new KeyItemData(KeyUse.FinalBoss)},
        {Item.DarkKey, new KeyItemData(KeyUse.Darker)},
        {Item.ScarabKey, new KeyItemData(KeyUse.Scarab)},
    };

    private ILogger _logger;

    public DictionaryItemFactory(ILogger logger = null)
    {
        _logger = logger ?? new NullLogger();
    }

    public Item CreateItem(string identifier, ItemCategory category)
    {
        if (_pickupItemData.ContainsKey(identifier))
        {
            return new PickupItem(identifier, category, _pickupItemData[identifier].Stats);
        }
        else if (_skillItemData.ContainsKey(identifier))
        {
            return new SkillItem(identifier, category, _skillItemData[identifier].Skill);
        }
        else if (_moduleItemData.ContainsKey(identifier))
        {
            return new ModuleItem(identifier, category, _moduleItemData[identifier].Module);
        }
        else if (_npcItemData.ContainsKey(identifier))
        {
            return new NpcItem(identifier, category, _npcItemData[identifier].NpcId);
        }
        else if (_mapItemData.ContainsKey(identifier))
        {
            return new MapItem(identifier, category, _mapItemData[identifier].MapRegion);
        }
        else if (_keyItemData.ContainsKey(identifier))
        {
            return new KeyItem(identifier, category, _keyItemData[identifier].KeyId);
        }
        else if (identifier == Item.AncientTablet)
        {
            return new LoreItem(identifier, category);
        }
        else if (identifier == Item.Scarab)
        {
            return new ScarabItem(identifier, category);
        }
        else if (identifier == Item.Spirit)
        {
            return new SpiritItem(identifier, category);
        
        }
        else if (identifier.Contains("XP Crystals x"))
        {
            // "XP Crystals x20" must return 20 for example.
            Match match = Regex.Match(identifier, @"\d+");
            if (!match.Success)
            {
                _logger.LogError($"Invalid item identifier: {identifier}");
                throw new InvalidItemException();
            }

            return new XpCrystalsItem(identifier, category, int.Parse(match.Value));
        }
        else if (identifier.Contains("Super Crystals x"))
        {
            // "Super Crystals x20" must return 20 for example.
            Match match = Regex.Match(identifier, @"\d+");
            if (!match.Success)
            {
                _logger.LogError($"Invalid item identifier: {identifier}");
                throw new InvalidItemException();
            }

            return new SuperCrystalsItem(identifier, category, int.Parse(match.Value));
        }
        else if (identifier.Contains("Small Key (Dungeon"))
        {
            // "Small Key (Dungeon 1)" must return 1 for example.
            Match match = Regex.Match(identifier, @"\d+");

            if (!match.Success)
            {
                _logger.LogError($"Invalid item identifier: {identifier}");
                throw new InvalidItemException();
            }

            return new SmallKeyItem(identifier, category, int.Parse(match.Value));
        }
        else if (identifier.Contains("Boss Key (Dungeon"))
        {
            // "Boss Key (Dungeon 1)" must return 1 for example.
            Match match = Regex.Match(identifier, @"\d+");
            if (!match.Success)
            {
                _logger.LogError($"Invalid item identifier: {identifier}");
                throw new InvalidItemException();
            }

            return new BossKeyItem(identifier, category, int.Parse(match.Value));
        }
        else if (Regex.IsMatch(identifier, "Dungeon [0-9] Reward"))
        {
            // "Dungeon 1 Reward" must return 1 for example.
            Match match = Regex.Match(identifier, @"\d+");
            if (!match.Success)
            {
                _logger.LogError($"Invalid item identifier: {identifier}");
                throw new InvalidItemException();
            }

            return new DungeonRewardItem(identifier, category, int.Parse(match.Value));
        }
        else
        {
            _logger.LogError($"Invalid item identifier: {identifier}");
            throw new InvalidItemException();
        }
    }
}

public class PickupItemData
{
    public Stats Stats { get; set; }

    public PickupItemData(Stats stats)
    {
        Stats = stats;
    }
}

public class SkillItemData
{
    public Skill Skill { get; set; }

    public SkillItemData(Skill skill)
    {
        Skill = skill;
    }
}

public class ModuleItemData
{
    public Modules Module { get; set; }

    public ModuleItemData(Modules module)
    {
        Module = module;
    }
}

public class NpcItemData
{
    private NpcIds _npcId;
    public NpcIds NpcId => _npcId;

    public NpcItemData(NpcIds npcId)
    {
        _npcId = npcId;
    }
}

public class MapItemData
{
    public MapRegion MapRegion { get; set; }

    public MapItemData(MapRegion mapRegion)
    {
        MapRegion = mapRegion;
    }
}

public class KeyItemData
{
    public KeyUse KeyId { get; set; }

    public KeyItemData(KeyUse keyId)
    {
        KeyId = keyId;
    }
}
