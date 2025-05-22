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

    public List<Tuple<LogicTestData, LogicState>> GetTestDataWithLogicState()
    {
        List<Tuple<LogicTestData, LogicState>> testDataWithLogicState = new List<Tuple<LogicTestData, LogicState>>();
        foreach (LogicTestData testData in _testData)
        {
            LogicState logicState = new LogicState();
            // We first merge the default settings with the overridden test data settings.
            foreach (ISetting setting in _defaultSettings)
            {
                logicState.SetSetting(setting);
            }
            foreach (ISetting setting in testData.OverrideSettings)
            {
                logicState.SetSetting(setting);
            }
            // We then add the default item counts to the logic state.
            foreach (var itemCount in _defaultItemCounts)
            {
                logicState.AddItemCount(itemCount.Key, itemCount.Value);
            }
            foreach (var itemCount in testData.AddItemCounts)
            {
                logicState.AddItemCount(itemCount.Key, itemCount.Value);
            }

            testDataWithLogicState.Add(new Tuple<LogicTestData, LogicState>(testData, logicState));
        }

        return testDataWithLogicState;
    }
}
