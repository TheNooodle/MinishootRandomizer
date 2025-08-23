using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MinishootRandomizer;

public class CoreLogicParser : ILogicParser
{
    private readonly IItemRepository _itemRepository;
    private readonly IRegionRepository _regionRepository;

    public CoreLogicParser(IItemRepository itemRepository, IRegionRepository regionRepository)
    {
        _itemRepository = itemRepository;
        _regionRepository = regionRepository;
    }

    public LogicParsingResult ParseLogic(string logicRule, LogicState state)
    {
        List<string> usedItems = new List<string>();
        bool result = false;

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
                LogicParsingResult parseResult = ParseRule(rule, state, arg);
                if (!parseResult.Result)
                {
                    andResult = false;
                }
                foreach (string item in parseResult.UsedItemNames)
                {
                    if (!usedItems.Contains(item))
                    {
                        usedItems.Add(item);
                    }
                }
            }

            if (andResult)
            {
                result = true;
            }
        }

        return new LogicParsingResult(result, usedItems);
    }

    private LogicParsingResult ParseRule(string rule, LogicState state, int arg)
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
        Item mercant = _itemRepository.Get(Item.Merchant);
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
        Item spirit = _itemRepository.Get(Item.Spirit);

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
        Region swampSouthWestIsland = _regionRepository.Get(Region.SwampSouthWestIsland);

        Dictionary<string, Func<LogicParsingParameters, LogicParsingResult>> rules = new()
        {
            { "true", p => new LogicParsingResult(
                true
            )},
            { "can_free_blacksmith", p => new LogicParsingResult(
                p.State.HasItem(blacksmith),
                new List<string>() {Item.Blacksmith}
            )},
            { "can_free_mercant", p => new LogicParsingResult(
                p.State.HasItem(mercant),
                new List<string>() {Item.Merchant}
            )},
            { "can_obtain_super_crystals", p => new LogicParsingResult(
                p.State.HasItem(cannon) && CanDash(p) && p.State.HasItem(supershot) && CanSurf(p, WaterType.Normal),
                new List<string>() {Item.ProgressiveCannon, Item.Dash, Item.Supershot, Item.Surf}
            )},
            { "can_fight", p => new LogicParsingResult(
                p.State.HasItem(cannon),
                new List<string>() {Item.ProgressiveCannon}
            )},
            { "can_fight_lvl2", p => new LogicParsingResult(
                CanFight(p, 2),
                new List<string>() {Item.ProgressiveCannon}
            )},
            { "can_fight_lvl3", p => new LogicParsingResult(
                CanFight(p, 3),
                new List<string>() {Item.ProgressiveCannon}
            )},
            { "can_fight_lvl4", p => new LogicParsingResult(
                CanFight(p, 4),
                new List<string>() {Item.ProgressiveCannon}
            )},
            { "can_fight_lvl5", p => new LogicParsingResult(
                CanFight(p, 5),
                new List<string>() {Item.ProgressiveCannon}
            )},
            { "can_cross_gaps", p => new LogicParsingResult(
                CanCrossGaps(p, GapSize.Normal),
                new List<string>() {Item.Dash}
            )},
            { "can_cross_tight_gaps", p => new LogicParsingResult(
                CanCrossGaps(p, GapSize.Tight),
                new List<string>() {Item.Dash}
            )},
            { "can_cross_very_tight_gaps", p => new LogicParsingResult(
                CanCrossGaps(p, GapSize.VeryTight),
                new List<string>() {Item.Dash}
            )},
            { "can_surf", p => new LogicParsingResult(
                CanSurf(p, WaterType.Normal),
                new List<string>() {Item.Surf}
            )},
            { "can_boost", p => new LogicParsingResult(
                p.State.HasItem(boost),
                new List<string>() {Item.Boost}
            )},
            { "can_destroy_bushes", p => new LogicParsingResult(
                p.State.HasItem(cannon),
                new List<string>() {Item.ProgressiveCannon}
            )},
            { "can_destroy_ruins", p => new LogicParsingResult(
                p.State.HasItem(cannon),
                new List<string>() {Item.ProgressiveCannon}
            )},
            { "have_d1_keys", p => new LogicParsingResult(
                p.State.HasItem(d1SmallKey, p.Arg),
                new List<string>() {Item.D1SmallKey}
            )},
            { "have_d1_boss_key", p => new LogicParsingResult(
                p.State.HasItem(d1BossKey),
                new List<string>() {Item.D1BossKey}
            )},
            { "can_destroy_rocks", p => new LogicParsingResult(
                CanDestroyWalls(p),
                new List<string>() {Item.Supershot, Item.PrimordialCrystal}
            )},
            { "can_destroy_pots", p => new LogicParsingResult(
                p.State.HasItem(cannon),
                new List<string>() {Item.ProgressiveCannon}
            )},
            { "can_destroy_crystals", p => new LogicParsingResult(
                p.State.HasItem(cannon),
                new List<string>() {Item.ProgressiveCannon}
            )},
            { "have_d2_keys", p => new LogicParsingResult(
                p.State.HasItem(d2SmallKey, p.Arg),
                new List<string>() {Item.D2SmallKey}
            )},
            { "have_d2_boss_key", p => new LogicParsingResult(
                p.State.HasItem(d2BossKey),
                new List<string>() {Item.D2BossKey}
            )},
            { "can_destroy_walls", p => new LogicParsingResult(
                CanDestroyWalls(p),
                new List<string>() {Item.Supershot, Item.PrimordialCrystal}
            )},
            { "can_obtain_scarabs", p => new LogicParsingResult(
                p.State.HasItem(scarab, p.Arg),
                new List<string>() {Item.Scarab}
            )},
            { "can_free_scarab_collector", p => new LogicParsingResult(
                p.State.HasItem(scarabCollector),
                new List<string>() {Item.ScarabCollector}
            )},
            { "can_light_torches", p => new LogicParsingResult(
                p.State.HasItem(supershot),
                new List<string>() {Item.Supershot}
            )},
            { "can_destroy_plants", p => new LogicParsingResult(
                p.State.HasItem(cannon),
                new List<string>() {Item.ProgressiveCannon}
            )},
            { "can_destroy_coconuts", p => new LogicParsingResult(
                p.State.HasItem(cannon),
                new List<string>() {Item.ProgressiveCannon}
            )},
            { "can_destroy_shells", p => new LogicParsingResult(
                p.State.HasItem(cannon),
                new List<string>() {Item.ProgressiveCannon}
            )},
            { "have_d3_keys", p => new LogicParsingResult(
                p.State.HasItem(d3SmallKey, p.Arg),
                new List<string>() {Item.D3SmallKey}
            )},
            { "have_d3_boss_key", p => new LogicParsingResult(
                p.State.HasItem(d3BossKey),
                new List<string>() {Item.D3BossKey}
            )},
            { "can_light_all_scarab_temple_torches", p => new LogicParsingResult(
                CanSurf(p, WaterType.Normal) && p.State.HasItem(supershot) && CanFight(p, 4),
                new List<string>() {LogicParsingResult.AnyItemName}
            )},
            { "can_dodge_purple_bullets", p => new LogicParsingResult(
                CanDash(p) && CanSpiritDash(p),
                new List<string>() {Item.Dash, Item.SpiritDash}
            )},
            { "can_unlock_final_boss_door", p => new LogicParsingResult(
                p.State.HasItem(darkHeart),
                new List<string>() {Item.DarkHeart}
            )},
            { "can_open_north_city_bridge", p => new LogicParsingResult(
                CanDash(p) && CanFight(p, 4) && CanSurf(p, WaterType.Gold) && CanDestroyWalls(p),
                new List<string>() {LogicParsingResult.AnyItemName}
            )},
            { "can_free_bard", p => new LogicParsingResult(
                p.State.HasItem(bard),
                new List<string>() {Item.Bard}
            )},
            { "have_all_spirits", p => new LogicParsingResult(
                HaveAllSpirits(p),
                new List<string>() {Item.Spirit}
            )},
            { "can_open_dungeon_5", p => new LogicParsingResult(
                p.State.HasItem(d1Reward) && p.State.HasItem(d2Reward) && p.State.HasItem(d3Reward) && p.State.HasItem(d4Reward) && p.State.HasItem(darkKey),
                new List<string>() {Item.D1Reward, Item.D2Reward, Item.D3Reward, Item.D4Reward, Item.DarkKey}
            )},
            { "can_unlock_primordial_cave_door", p => new LogicParsingResult(
                p.State.HasItem(scarabKey),
                new List<string>() {Item.ScarabKey}
            )},
            { "can_light_city_torches", p => new LogicParsingResult(
                p.State.HasItem(supershot) && CanSurf(p, WaterType.Gold) && CanFight(p, 4) && CanUseSpringboards(p),
                new List<string>() {LogicParsingResult.AnyItemName}
            )},
            { "can_open_sunken_temple", p => new LogicParsingResult(
                CanSurf(p, WaterType.Gold) && CanFight(p, 4) && CanDash(p) && CanDestroyWalls(p),
                new List<string>() {LogicParsingResult.AnyItemName}
            )},
            { "can_light_desert_grotto_torches", p => new LogicParsingResult(
                p.State.HasItem(supershot) && (CanSurf(p, WaterType.Normal) || CanCrossGaps(p, GapSize.Normal)) && CanFight(p, 3),
                new List<string>() {LogicParsingResult.AnyItemName}
            )},
            { "can_clear_both_d5_arenas", p => new LogicParsingResult(
                CanFight(p, 5) && CanDash(p) && CanSurf(p, WaterType.Soiled),
                new List<string>() {LogicParsingResult.AnyItemName}
            )},
            { "can_free_family", p => new LogicParsingResult(
                p.State.HasItem(familyChild) && p.State.HasItem(familyParent1) && p.State.HasItem(familyParent2),
                new List<string>() {Item.FamilyChild, Item.FamilyParent1, Item.FamilyParent2}
            )},
            { "forest_is_blocked", p => new LogicParsingResult(
                IsSettingEnabled<BlockedForest>(p)
            )},
            { "forest_is_open", p => new LogicParsingResult(
                !IsSettingEnabled<BlockedForest>(p)
            )},
            { "can_blast_crystals", p => new LogicParsingResult(
                p.State.HasItem(cannon) && p.State.HasItem(powerOfProtection),
                new List<string>() {Item.ProgressiveCannon, Item.PowerOfProtection}
            )},
            { "can_destroy_trees", p => new LogicParsingResult(
                p.State.HasItem(supershot),
                new List<string>() {Item.Supershot}
            )},
            { "can_use_springboards", p => new LogicParsingResult(
                CanUseSpringboards(p),
                new List<string>() {Item.Boost, Item.Dash}
            )},
            { "can_race_spirits", p => new LogicParsingResult(
                CanRaceSpirits(p),
                new List<string>() {Item.Boost, Item.Dash}
            )},
            { "can_race_torches", p => new LogicParsingResult(
                CanRaceTorches(p),
                new List<string>() {Item.Boost}
            )},
            { "can_open_swamp_tower", p => new LogicParsingResult(
                CanSurf(p, WaterType.Normal) || CanUseSpringboards(p),
                new List<string>() {Item.Boost}
            )},
        };

        if (rules.ContainsKey(rule))
        {
            return rules[rule](new LogicParsingParameters(state, arg));
        }

        throw new UnknownRuleException($"Unknown rule: {rule}");
    }

    private bool CanDash(LogicParsingParameters parameters)
    {
        return parameters.State.HasItem(_itemRepository.Get(Item.Dash));
    }

    private bool CanSpiritDash(LogicParsingParameters parameters)
    {
        return parameters.State.HasItem(_itemRepository.Get(Item.SpiritDash));
    }

    private bool CanFight(LogicParsingParameters parameters, int level = 1)
    {
        IgnoreCannonLevelRequirements setting = parameters.State.GetSetting<IgnoreCannonLevelRequirements>();

        if (!setting.Enabled)
        {
            return parameters.State.HasItem(_itemRepository.Get(Item.ProgressiveCannon), level);
        }
        else
        {
            return parameters.State.HasItem(_itemRepository.Get(Item.ProgressiveCannon));
        }
    }

    private bool CanCrossGaps(LogicParsingParameters parameters, GapSize gapSize)
    {
        if (CanDash(parameters))
        {
            return true;
        }

        DashlessGaps setting = parameters.State.GetSetting<DashlessGaps>();
        if ((gapSize == GapSize.Tight || gapSize == GapSize.VeryTight) && setting.Value != DashlessGapsValue.NeedsDash && parameters.State.HasItem(_itemRepository.Get(Item.Boost)))
        {
            return true;
        }

        if (gapSize == GapSize.VeryTight && setting.Value == DashlessGapsValue.NeedsNeither)
        {
            return true;
        }

        return false;
    }

    private bool CanDestroyWalls(LogicParsingParameters parameters)
    {
        EnablePrimordialCrystalLogic setting = parameters.State.GetSetting<EnablePrimordialCrystalLogic>();
        if (setting.Enabled)
        {
            return parameters.State.HasItem(_itemRepository.Get(Item.Supershot)) || parameters.State.HasItem(_itemRepository.Get(Item.PrimordialCrystal));
        }
        else
        {
            return parameters.State.HasItem(_itemRepository.Get(Item.Supershot));
        }
    }

    private bool CanSurf(LogicParsingParameters parameters, WaterType waterType)
    {
        return parameters.State.HasItem(_itemRepository.Get(Item.Surf));
    }

    private bool CanUseSpringboards(LogicParsingParameters parameters)
    {
        BoostlessSpringboards setting = parameters.State.GetSetting<BoostlessSpringboards>();

        if (setting.Enabled)
        {
            return parameters.State.HasItem(_itemRepository.Get(Item.Boost)) || CanDash(parameters);
        }
        else
        {
            return parameters.State.HasItem(_itemRepository.Get(Item.Boost));
        }
    }

    private bool CanRaceSpirits(LogicParsingParameters parameters)
    {
        BoostlessSpiritRaces setting = parameters.State.GetSetting<BoostlessSpiritRaces>();

        if (setting.Enabled)
        {
            return parameters.State.HasItem(_itemRepository.Get(Item.Boost)) || CanDash(parameters);
        }
        else
        {
            return parameters.State.HasItem(_itemRepository.Get(Item.Boost));
        }
    }

    private bool CanRaceTorches(LogicParsingParameters parameters)
    {
        BoostlessTorchRaces setting = parameters.State.GetSetting<BoostlessTorchRaces>();

        if (setting.Enabled)
        {
            return true;
        }
        else
        {
            return parameters.State.HasItem(_itemRepository.Get(Item.Boost));
        }
    }

    private bool IsSettingEnabled<T>(LogicParsingParameters parameters) where T : BooleanSetting
    {
        T setting = parameters.State.GetSetting<T>();
        if (setting == null)
        {
            return false;
        }

        return setting.Enabled;
    }

    private bool HaveAllSpirits(LogicParsingParameters parameters)
    {
        SpiritTowerRequirement setting = parameters.State.GetSetting<SpiritTowerRequirement>();
        if (setting == null)
        {
            return parameters.State.HasItem(_itemRepository.Get(Item.Spirit), 8);
        }

        return parameters.State.HasItem(_itemRepository.Get(Item.Spirit), setting.Value);
    }
}
