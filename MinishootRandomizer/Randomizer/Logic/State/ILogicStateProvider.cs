namespace MinishootRandomizer;

public interface ILogicStateProvider
{
    LogicState GetLogicState(LogicTolerance tolerance = LogicTolerance.Strict);
}
