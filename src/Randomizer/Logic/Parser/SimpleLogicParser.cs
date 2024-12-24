using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MinishootRandomizer;

public class SimpleLogicParser : ILogicParser
{
    private readonly IItemRepository _itemRepository;
    private readonly IRegionRepository _regionRepository;

    public SimpleLogicParser(IItemRepository itemRepository, IRegionRepository regionRepository)
    {
        _itemRepository = itemRepository;
        _regionRepository = regionRepository;
    }

    public bool ParseLogic(string logicRule, LogicState state, List<ISetting> settings)
    {
        // We split "or" first.
        string[] orSplit = Regex.Split(logicRule, @"\s+or\s+");
        foreach (string orRule in orSplit)
        {
            // We then split "and".
            bool andResult = true;
            string[] andSplit = Regex.Split(orRule, @"\s+and\s+");
            foreach (string andRule in andSplit)
            {
                // We split the rule into its parts.
                string rule = andRule.Split("(")[0].Trim();
                int arg = 1;

                if (andRule.Contains("("))
                {
                    arg = int.Parse(andRule.Split("(")[1].Split(")")[0]);
                }

                // We parse the rule.
                bool result = ParseRule(rule, state, arg, settings);
                if (!result)
                {
                    andResult = false;
                    break;
                }
            }

            if (andResult)
            {
                return true;
            }
        }

        return false;
    }

    private bool ParseRule(string rule, LogicState state, int arg, List<ISetting> settings)
    {
        Item cannon = _itemRepository.Get(Item.ProgressiveCannon);

        Item boost = _itemRepository.Get(Item.Boost);
        Item dash = _itemRepository.Get(Item.Dash);
        Item supershot = _itemRepository.Get(Item.Supershot);
        Item surf = _itemRepository.Get(Item.Surf);
        Item primordialCrystal = _itemRepository.Get(Item.PrimordialCrystal);
        Item spiritDash = _itemRepository.Get(Item.SpiritDash);

        Item powerOfProtection = _itemRepository.Get(Item.PowerOfProtection);

        Item blacksmith = _itemRepository.Get(Item.Blacksmith);
        Item mercant = _itemRepository.Get(Item.Mercant);
        Item scarabCollector = _itemRepository.Get(Item.ScarabCollector);
        Item bard = _itemRepository.Get(Item.Bard);
        Item familyChild = _itemRepository.Get(Item.FamilyChild);
        Item familyParent1 = _itemRepository.Get(Item.FamilyParent1);
        Item familyParent2 = _itemRepository.Get(Item.FamilyParent2);

        Item d1BossKey = _itemRepository.Get(Item.D1BossKey);
        Item d1SmallKey = _itemRepository.Get(Item.D1SmallKey);
        Item d1Reward = _itemRepository.Get(Item.D1Reward);
        Item d2BossKey = _itemRepository.Get(Item.D2BossKey);
        Item d2SmallKey = _itemRepository.Get(Item.D2SmallKey);
        Item d2Reward = _itemRepository.Get(Item.D2Reward);
        Item d3BossKey = _itemRepository.Get(Item.D3BossKey);
        Item d3SmallKey = _itemRepository.Get(Item.D3SmallKey);
        Item d3Reward = _itemRepository.Get(Item.D3Reward);
        Item d4Reward = _itemRepository.Get(Item.D4Reward);

        Item scarab = _itemRepository.Get(Item.Scarab);

        Item darkKey = _itemRepository.Get(Item.DarkKey);
        Item darkHeart = _itemRepository.Get(Item.DarkHeart);
        Item scarabKey = _itemRepository.Get(Item.ScarabKey);

        Region bottomLeftTorch = _regionRepository.Get(Region.ScarabTempleBottomLeftTorch);
        Region bottomRightTorch = _regionRepository.Get(Region.ScarabTempleBottomRightTorch);
        Region topLeftTorch = _regionRepository.Get(Region.ScarabTempleTopLeftTorch);
        Region topRightTorch = _regionRepository.Get(Region.ScarabTempleTopRightTorch);
        Region sunkenCityFountain = _regionRepository.Get(Region.SunkenCityFountain);
        Region sunkenCityWestTorch = _regionRepository.Get(Region.SunkenCityWestTorch);
        Region sunkenCityEastTorch = _regionRepository.Get(Region.SunkenCityEastTorch);
        Region sunkenCityWestIsland = _regionRepository.Get(Region.SunkenCityWestIsland);
        Region sunkenCityCity = _regionRepository.Get(Region.SunkenCityCity);
        Region sunkenCityEast = _regionRepository.Get(Region.SunkenCityEast);
        Region desertGrottoWestDrop = _regionRepository.Get(Region.DesertGrottoWestDrop);
        Region desertGrottoEastDrop = _regionRepository.Get(Region.DesertGrottoEastDrop);
        Region d5WestWing = _regionRepository.Get(Region.Dungeon5WestWing);
        Region d5EastWing = _regionRepository.Get(Region.Dungeon5EastWing);
        Region d5Boss = _regionRepository.Get(Region.Dungeon5Boss);

        Dictionary<string, Func<SimpleLogicParameters, bool>> rules = new()
        {
            { "true", p => true },
            { "can_free_blacksmith", p => p.State.HasItem(blacksmith) },
            { "can_free_mercant", p => p.State.HasItem(mercant) },
            { "can_obtain_super_crystals", p => p.State.HasItem(cannon) && CanDash(p) && p.State.HasItem(supershot) && CanSurf(p, WaterType.Normal) },
            { "can_fight", p => p.State.HasItem(cannon) },
            { "can_fight_lvl2", p => CanFight(p, 2) },
            { "can_fight_lvl3", p => CanFight(p, 3) },
            { "can_fight_lvl4", p => CanFight(p, 4) },
            { "can_fight_lvl5", p => CanFight(p, 5) },
            { "can_dash", p => CanDash(p) },
            { "can_boost", p => p.State.HasItem(boost) },
            { "can_destroy_bushes", p => p.State.HasItem(cannon) },
            { "can_destroy_ruins", p => p.State.HasItem(cannon) },
            { "have_d1_keys", p => p.State.HasItem(d1SmallKey, p.Arg) },
            { "have_d1_boss_key", p => p.State.HasItem(d1BossKey) },
            { "can_dodge_homing_charges", p => CanDash(p) || p.State.HasItem(boost) },
            { "can_destroy_rocks", p => p.State.HasItem(supershot) || p.State.HasItem(primordialCrystal) },
            { "can_dodge_fast_patterns", p => CanDash(p) || p.State.HasItem(boost) },
            { "can_destroy_pots", p => p.State.HasItem(cannon) },
            { "can_destroy_crystals", p => p.State.HasItem(cannon) },
            { "have_d2_keys", p => p.State.HasItem(d2SmallKey, p.Arg) },
            { "have_d2_boss_key", p => p.State.HasItem(d2BossKey) },
            { "can_destroy_walls", p => p.State.HasItem(supershot) || p.State.HasItem(primordialCrystal) },
            { "can_obtain_scarabs", p => p.State.HasItem(scarab, p.Arg) },
            { "can_free_scarab_collector", p => p.State.HasItem(scarabCollector) },
            { "can_light_torches", p => p.State.HasItem(supershot) },
            { "can_destroy_plants", p => p.State.HasItem(cannon) },
            { "can_destroy_coconuts", p => p.State.HasItem(cannon) },
            { "can_destroy_shells", p => p.State.HasItem(cannon) },
            { "have_d3_keys", p => p.State.HasItem(d3SmallKey, p.Arg) },
            { "have_d3_boss_key", p => p.State.HasItem(d3BossKey) },
            { "can_light_all_scarab_temple_torches", p => p.State.CanReach(bottomLeftTorch) && p.State.CanReach(bottomRightTorch) && p.State.CanReach(topLeftTorch) && p.State.CanReach(topRightTorch) && p.State.HasItem(supershot) },
            { "can_dodge_purple_bullets", p => CanDash(p) && CanSpiritDash(p) },
            { "can_unlock_final_boss_door", p => p.State.HasItem(darkHeart) },
            { "can_open_north_city_bridge", p => p.State.CanReach(sunkenCityFountain) && CanFight(p, 4) && CanSurf(p, WaterType.Gold) },
            { "cannot_dash", p => !CanDash(p) },
            { "can_free_bard", p => p.State.HasItem(bard) },
            { "have_all_spirits", p => CanSurf(p, WaterType.Normal) && CanDash(p) && p.State.HasItem(boost) && p.State.HasItem(supershot) },
            { "can_open_dungeon_5", p => p.State.HasItem(d1Reward) && p.State.HasItem(d2Reward) && p.State.HasItem(d3Reward) && p.State.HasItem(d4Reward) && p.State.HasItem(darkKey) },
            { "can_unlock_primordial_cave_door", p => p.State.HasItem(scarabKey) },
            { "can_light_city_torches", p => p.State.HasItem(supershot) && p.State.CanReach(sunkenCityWestTorch) && p.State.CanReach(sunkenCityEastTorch) },
            { "can_open_sunken_temple", p => CanSurf(p, WaterType.Gold) && CanFight(p, 4) && p.State.CanReach(sunkenCityWestIsland) && p.State.CanReach(sunkenCityCity) && p.State.CanReach(sunkenCityEast) },
            { "can_light_desert_grotto_torches", p => p.State.HasItem(supershot) && p.State.CanReach(desertGrottoWestDrop) && p.State.CanReach(desertGrottoEastDrop) },
            { "can_clear_both_d5_arenas", p => p.State.CanReach(d5WestWing) && p.State.CanReach(d5EastWing) && CanFight(p, 5) && CanDash(p) },
            { "can_free_family", p => p.State.HasItem(familyChild) && p.State.HasItem(familyParent1) && p.State.HasItem(familyParent2) },
            { "cannot_surf", p => !p.State.HasItem(surf) },
            { "have_cleared_d5", p => p.State.CanReach(d5Boss) },
            { "forest_is_blocked", p => IsSettingEnabled<BlockedForest>(p) },
            { "forest_is_open", p => !IsSettingEnabled<BlockedForest>(p) },
            { "can_blast_crystals", p => p.State.HasItem(cannon) && p.State.HasItem(powerOfProtection) },
            { "can_destroy_trees", p => p.State.HasItem(supershot) }
        };

        if (rules.ContainsKey(rule))
        {
            return rules[rule](new SimpleLogicParameters(state, arg, settings));
        }

        throw new UnknownRuleException($"Unknown rule: {rule}");
    }

    private bool CanDash(SimpleLogicParameters parameters)
    {
        return parameters.State.HasItem(_itemRepository.Get(Item.Dash));
    }

    private bool CanSpiritDash(SimpleLogicParameters parameters)
    {
        return parameters.State.HasItem(_itemRepository.Get(Item.SpiritDash));
    }

    private bool CanFight(SimpleLogicParameters parameters, int level = 1)
    {
        CannonLevelLogicalRequirements setting = (CannonLevelLogicalRequirements)parameters.Settings.Find(s => s is CannonLevelLogicalRequirements);

        if (setting.Enabled)
        {
            return parameters.State.HasItem(_itemRepository.Get(Item.ProgressiveCannon), level);
        }
        else
        {
            return parameters.State.HasItem(_itemRepository.Get(Item.ProgressiveCannon));
        }
    }

    private bool CanSurf(SimpleLogicParameters parameters, WaterType waterType)
    {
        return parameters.State.HasItem(_itemRepository.Get(Item.Surf));
    }

    private bool IsSettingEnabled<T>(SimpleLogicParameters parameters) where T : BooleanSetting
    {
        T setting = (T)parameters.Settings.Find(s => s is T);
        if (setting == null)
        {
            return false;
        }

        return setting.Enabled;
    }
}
