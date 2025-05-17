namespace MinishootRandomizer;

/// <summary>
/// Interface for checking the logic accessibility of locations.
/// This interface is used to determine if a location is accessible in logic, out of logic, or inaccessible.
/// It uses the logic parser to evaluate the logic rules of each location, by considering the current logic state.
/// </summary>
public interface ILocationLogicChecker
{
    LogicAccessibility CheckLocationLogic(LogicState logicState, Location location);
    LocationAccessibilitySet CheckAllLocationsLogic(LogicState logicState);
}
