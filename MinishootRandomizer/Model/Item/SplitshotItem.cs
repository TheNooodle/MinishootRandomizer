namespace MinishootRandomizer;

public class SplitshotItem : SkillItem
{
    public SplitshotType Type { get; }

    public SplitshotItem(string identifier, ItemCategory itemCategory, SplitshotType type)
        : base(identifier, itemCategory, Skill.Supershot)
    {
        Type = type;
    }

    public override void Collect()
    {
        base.Collect();
        WorldState.Set(GetWorldStateKey(), true);
    }

    public override int GetOwnedQuantity()
    {
        return WorldState.Get(GetWorldStateKey()) ? 1 : 0;
    }

    private string GetWorldStateKey()
    {
        return Type == SplitshotType.Blast ? "BlastshotUnlocked" : "FlameshotUnlocked";
    }
}
