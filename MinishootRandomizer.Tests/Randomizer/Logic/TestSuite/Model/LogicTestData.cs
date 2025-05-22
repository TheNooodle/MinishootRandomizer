namespace MinishootRandomizer.Tests;

public class LogicTestData
{
    private readonly string _name;
    private readonly List<ISetting> _overrideSettings;
    private readonly Dictionary<Item, int> _addItemCounts;
    private readonly List<AbstractLogicAssertion> _assertions;

    public string Name => _name;
    public List<ISetting> OverrideSettings => _overrideSettings;
    public Dictionary<Item, int> AddItemCounts => _addItemCounts;
    public List<AbstractLogicAssertion> Assertions => _assertions;

    public LogicTestData(string name, List<ISetting> overrideSettings, Dictionary<Item, int> addItemCounts, List<AbstractLogicAssertion> assertions)
    {
        _name = name;
        _overrideSettings = overrideSettings;
        _addItemCounts = addItemCounts;
        _assertions = assertions;
    }
}
