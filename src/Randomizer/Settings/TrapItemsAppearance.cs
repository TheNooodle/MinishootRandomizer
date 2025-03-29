namespace MinishootRandomizer;

public enum TrapItemsAppearanceValue
{
    MajorItems,
    JunkItems,
    Anything
}

public class TrapItemsAppearance : ISetting
{
    public TrapItemsAppearanceValue Value { get; set; }

    public TrapItemsAppearance(TrapItemsAppearanceValue value)
    {
        Value = value;
    }
}
