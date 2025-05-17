namespace MinishootRandomizer.Tests;

public class LogicTestData
{
    private readonly string _name;
    private readonly List<ISetting> _overrideSettings;
    private readonly Dictionary<Item, int> _overrideItemCounts;
    private readonly List<LogicTestData> _tests;

    public string Name => _name;
    public List<ISetting> OverrideSettings => _overrideSettings;
    public Dictionary<Item, int> OverrideItemCounts => _overrideItemCounts;
    public List<LogicTestData> Tests => _tests;

    public LogicTestData(string name, List<ISetting> overrideSettings, Dictionary<Item, int> overrideItemCounts, List<LogicTestData> tests)
    {
        _name = name;
        _overrideSettings = overrideSettings;
        _overrideItemCounts = overrideItemCounts;
        _tests = tests;
    }
}
