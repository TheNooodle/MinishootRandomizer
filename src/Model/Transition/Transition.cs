namespace MinishootRandomizer;

public class Transition
{
    private string _name;
    private string _from;
    private string _to;
    private string _logicRule;

    public string Name => _name;
    public string From => _from;
    public string To => _to;
    public string LogicRule => _logicRule;

    public Transition(string name, string from, string to, string logicRule)
    {
        _name = name;
        _from = from;
        _to = to;
        _logicRule = logicRule;
    }
}
