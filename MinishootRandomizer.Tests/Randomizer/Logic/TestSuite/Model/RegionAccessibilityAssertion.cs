namespace MinishootRandomizer.Tests;

public class RegionAccessibilityAssertion : AbstractLogicAssertion
{
    private readonly Region _region;
    private readonly LogicAccessibility _expectedAccessibility;

    public Region Region => _region;
    public LogicAccessibility ExpectedAccessibility => _expectedAccessibility;

    public RegionAccessibilityAssertion(Region region, LogicAccessibility expectedAccessibility)
    {
        _region = region;
        _expectedAccessibility = expectedAccessibility;
    }
}
