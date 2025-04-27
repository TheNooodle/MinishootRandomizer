namespace MinishootRandomizer;

public enum DashlessGapsValue
{
    NeedsDash,
    NeedsBoost,
    NeedsNeither,
}

public class DashlessGaps : ISetting
{
    public DashlessGapsValue Value { get; set; } = DashlessGapsValue.NeedsDash;

    public DashlessGaps(DashlessGapsValue value)
    {
        Value = value;
    }
}
