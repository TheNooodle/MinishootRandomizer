using YamlDotNet.RepresentationModel;

namespace MinishootRandomizer.Tests;

public class YamlTestSuiteProvider : ITestSuiteProvider
{
    private readonly string _yamlFilePath;
    private readonly IItemRepository _itemRepository;
    private readonly ILocationRepository _locationRepository;
    private readonly IRegionRepository _regionRepository;

    private static readonly Dictionary<string, Type> SettingTypeMap = new()
    {
        { "npc_sanity", typeof(NpcSanity) },
        { "scarab_sanity", typeof(ScarabSanity) },
        { "shard_sanity", typeof(ShardSanity) },
        { "key_sanity", typeof(KeySanity) },
        { "boss_key_sanity", typeof(BossKeySanity) },
        { "blocked_forest", typeof(BlockedForest) },
        { "ignore_cannon_level_requirements", typeof(IgnoreCannonLevelRequirements) },
        { "boostless_springboards", typeof(BoostlessSpringboards) },
        { "boostless_spirit_races", typeof(BoostlessSpiritRaces) },
        { "boostless_torch_races", typeof(BoostlessTorchRaces) },
        { "enable_primordial_crystal_logic", typeof(EnablePrimordialCrystalLogic) },
        { "progressive_dash", typeof(ProgressiveDash) },
        { "dashless_gaps", typeof(DashlessGaps) },
        { "completion_goals", typeof(CompletionGoals) },
    };

    private static readonly Dictionary<Type, Type> SettingTypeEnumMap = new()
    {
        { typeof(DashlessGaps), typeof(DashlessGapsValue) },
        { typeof(CompletionGoals), typeof(Goals) },
    };

    public YamlTestSuiteProvider(string yamlFilePath, IItemRepository itemRepository, ILocationRepository locationRepository, IRegionRepository regionRepository)
    {
        _yamlFilePath = yamlFilePath;
        _itemRepository = itemRepository;
        _locationRepository = locationRepository;
        _regionRepository = regionRepository;
    }

    public LogicTestSuite GetLogicTestSuite()
    {
        var input = new StringReader(File.ReadAllText(_yamlFilePath));
        var yaml = new YamlStream();
        yaml.Load(input);

        var rootNode = (YamlMappingNode)yaml.Documents[0].RootNode;
        List<ISetting> defaultSettings = rootNode.Children.ContainsKey("default_settings")
            ? ParseDefaultSettings((YamlMappingNode)rootNode.Children["default_settings"])
            : new List<ISetting>();

        Dictionary<Item, int> defaultItemCounts = rootNode.Children.ContainsKey("default_item_counts")
            ? ParseDefaultItemCounts((YamlMappingNode)rootNode.Children["default_item_counts"])
            : new Dictionary<Item, int>();

        List<LogicTestData> testData = rootNode.Children.ContainsKey("test_cases")
            ? ParseTestData((YamlSequenceNode)rootNode.Children["test_cases"])
            : new List<LogicTestData>();

        return new LogicTestSuite(
            defaultSettings,
            defaultItemCounts,
            testData
        );
    }

    private List<ISetting> ParseDefaultSettings(YamlMappingNode defaultSettingsNode)
    {
        var settings = new List<ISetting>();

        foreach (var settingNode in defaultSettingsNode.Children)
        {
            settings.Add(CreateSetting(settingNode.Key.ToString(), settingNode.Value.ToString()));
        }

        return settings;
    }

    private ISetting CreateSetting(string settingName, string settingValue)
    {
        if (!SettingTypeMap.TryGetValue(settingName, out var settingType) || settingType == null)
        {
            throw new InvalidOperationException($"Setting {settingName} is not recognized.");
        }

        if (settingType == typeof(BooleanSetting) || settingType.IsSubclassOf(typeof(BooleanSetting)))
        {
            bool isEnabled = settingValue.ToLower() == "true";
            return (BooleanSetting)Activator.CreateInstance(settingType, new object[] { isEnabled })!;
        }

        if (SettingTypeEnumMap.TryGetValue(settingType, out var enumType))
        {
            if (Enum.TryParse(enumType, settingValue, true, out var enumValue))
            {
                return (ISetting)Activator.CreateInstance(settingType, new object[] { enumValue })!;
            }
            throw new InvalidOperationException($"Setting {settingName} has an invalid value: {settingValue}");
        }

        throw new InvalidOperationException($"Setting {settingName} is not a setting.");
    }


    private Dictionary<Item, int> ParseDefaultItemCounts(YamlMappingNode defaultItemCountsNode)
    {
        Dictionary<Item, int> itemCounts = new Dictionary<Item, int>();

        foreach (var itemCountNode in defaultItemCountsNode.Children)
        {
            string itemName = itemCountNode.Key.ToString();
            int count = int.Parse(itemCountNode.Value.ToString()!);
            Item item = _itemRepository.Get(itemName);

            itemCounts[item] = count;
        }

        return itemCounts;
    }

    private List<LogicTestData> ParseTestData(YamlSequenceNode testDataNode)
    {
        List<LogicTestData> testDataList = new List<LogicTestData>();

        foreach (var testCaseNode in testDataNode.Children)
        {
            string name = testCaseNode["name"].ToString();
            YamlMappingNode mappingNode = (YamlMappingNode)testCaseNode;

            // If the "override_settings" node is present, we add its children to the override settings list
            List<ISetting> overrideSettings = new List<ISetting>();
            if (mappingNode.Children.ContainsKey("override_settings"))
            {
                var overrideSettingsNode = testCaseNode["override_settings"];
                foreach (var settingNode in ((YamlMappingNode)overrideSettingsNode).Children)
                {
                    overrideSettings.Add(CreateSetting(settingNode.Key.ToString(), settingNode.Value.ToString()));
                }
            }

            // If the "add_items" node is present, we add its children to the add item counts list
            Dictionary<Item, int> addItemCounts = new Dictionary<Item, int>();
            if (mappingNode.Children.ContainsKey("add_items"))
            {
                var addItemsNode = (YamlMappingNode)testCaseNode["add_items"];
                foreach (var itemCountNode in addItemsNode.Children)
                {
                    string itemName = itemCountNode.Key.ToString();
                    int count = int.Parse(itemCountNode.Value.ToString()!);
                    Item item = _itemRepository.Get(itemName);

                    addItemCounts[item] = count;
                }
            }

            // For each member of the "tests" node, we create a new LogicTestData object
            List<AbstractLogicAssertion> assertions = new List<AbstractLogicAssertion>();
            if (mappingNode.Children.ContainsKey("tests"))
            {
                var testsNode = (YamlSequenceNode)testCaseNode["tests"];
                foreach (var testNode in testsNode.Children)
                {
                    // We determine the type of assertion based on the node name
                    YamlMappingNode testMappingNode = (YamlMappingNode)testNode;
                    string assertionType = testMappingNode.Children.First().Key.ToString();
                    if (assertionType == "location_accessibility")
                    {
                        YamlMappingNode assertionNode = (YamlMappingNode)testMappingNode["location_accessibility"];
                        string locationName = assertionNode["location_name"].ToString();
                        Location location = _locationRepository.Get(locationName);
                        string expectedAccessibilityStr = assertionNode["expected_accessibility"].ToString();
                        LogicAccessibility expectedAccessibility = ParseLogicAccessibility(expectedAccessibilityStr);

                        assertions.Add(new LocationAccessibilityAssertion(location, expectedAccessibility));
                    }
                    else if (assertionType == "region_accessibility")
                    {
                        YamlMappingNode assertionNode = (YamlMappingNode)testMappingNode["region_accessibility"];
                        string regionName = assertionNode["region_name"].ToString();
                        Region region = _regionRepository.Get(regionName);
                        string expectedAccessibilityStr = assertionNode["expected_accessibility"].ToString();
                        LogicAccessibility expectedAccessibility = ParseLogicAccessibility(expectedAccessibilityStr);

                        assertions.Add(new RegionAccessibilityAssertion(region, expectedAccessibility));
                    }
                    else
                    {
                        throw new InvalidOperationException($"Assertion type {assertionType} is not recognized.");
                    }
                }
            }

            // We create a new LogicTestData object and add it to the list
            LogicTestData testData = new LogicTestData(name, overrideSettings, addItemCounts, assertions);
            testDataList.Add(testData);
        }

        return testDataList;
    }

    private LogicAccessibility ParseLogicAccessibility(string accessibilityStr)
    {
        return accessibilityStr switch
        {
            "out_of_logic" => LogicAccessibility.OutOfLogic,
            "in_logic" => LogicAccessibility.InLogic,
            _ => LogicAccessibility.Inaccessible,
        };
    }
}
