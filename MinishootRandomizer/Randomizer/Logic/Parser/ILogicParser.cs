namespace MinishootRandomizer;

// Heads up, this interface must maintain function parity with the implementation in the AP World.
// Otherwise, the use of external trackers will have different results than the internal tracker.
// Also, in case of a standalone engine implementation, the perceived randomization of different objects must be consistent.
public interface ILogicParser
{
    LogicParsingResult ParseLogic(string logicRule, LogicState state);
}
