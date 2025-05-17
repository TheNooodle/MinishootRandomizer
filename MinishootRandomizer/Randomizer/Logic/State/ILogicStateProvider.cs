namespace MinishootRandomizer;

/// <summary>
/// Interface for providing the current logic state.
/// A logic state is a representation of both the current items and the settings used in the randomizer.
/// </summary>
public interface ILogicStateProvider
{
    LogicState GetLogicState();
}
