namespace MinishootRandomizer.Tests;

public class LocationAccessibilityAssertion : AbstractLogicAssertion
{
    private readonly Location _location;
    private readonly LogicAccessibility _expectedAccessibility;

    public Location Location => _location;
    public LogicAccessibility ExpectedAccessibility => _expectedAccessibility;

    public LocationAccessibilityAssertion(Location location, LogicAccessibility expectedAccessibility)
    {
        _location = location;
        _expectedAccessibility = expectedAccessibility;
    }
}
