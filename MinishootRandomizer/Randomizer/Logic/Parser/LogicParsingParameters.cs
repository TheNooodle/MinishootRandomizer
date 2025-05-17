namespace MinishootRandomizer;

public class LogicParsingParameters
{
    private LogicState _state;
    private int _arg;

    public LogicState State => _state;
    public int Arg => _arg;

    public LogicParsingParameters(LogicState state, int arg)
    {
        _state = state;
        _arg = arg;
    }
}
