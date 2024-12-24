using System.Collections.Generic;

namespace MinishootRandomizer;

public class SimpleLogicParameters
{
    private LogicState _state;
    private int _arg;
    private List<ISetting> _settings;

    public LogicState State => _state;
    public int Arg => _arg;
    public List<ISetting> Settings => _settings;

    public SimpleLogicParameters(LogicState state, int arg, List<ISetting> settings)
    {
        _state = state;
        _arg = arg;
        _settings = settings;
    }
}
