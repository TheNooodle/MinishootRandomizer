using System.Collections.Generic;

namespace MinishootRandomizer;

public class LogicParsingResult
{
    public const string AnyItemName = "Any";

    private readonly bool _result;
    private readonly List<string> _usedItemNames;

    public bool Result => _result;
    public List<string> UsedItemNames => _usedItemNames;

    public LogicParsingResult(bool result, List<string> usedItemNames = null)
    {
        _result = result;
        _usedItemNames = usedItemNames ?? new List<string>();
    }
}
