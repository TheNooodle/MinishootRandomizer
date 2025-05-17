namespace MinishootRandomizer.Tests;

public class LogicTestSuite
{
    private readonly List<ISetting> _defaultSettings;
    private readonly Dictionary<Item, int> _defaultItemCounts;
    private readonly List<LogicTestData> _testData;

    public List<ISetting> DefaultSettings => _defaultSettings;
    public Dictionary<Item, int> DefaultItemCounts => _defaultItemCounts;
    public List<LogicTestData> TestData => _testData;

    public LogicTestSuite(List<ISetting> defaultSettings, Dictionary<Item, int> defaultItemCounts, List<LogicTestData> testData)
    {
        _defaultSettings = defaultSettings;
        _defaultItemCounts = defaultItemCounts;
        _testData = testData;
    }
}
