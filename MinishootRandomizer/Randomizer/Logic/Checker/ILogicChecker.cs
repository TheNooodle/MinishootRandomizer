namespace MinishootRandomizer;

public interface ILogicChecker
{
    LogicAccessibility CheckLocationLogic(Location location);
    LocationAccessibilitySet CheckAllLocationsLogic();
}
