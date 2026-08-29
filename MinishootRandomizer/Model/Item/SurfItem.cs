namespace MinishootRandomizer;

public class SurfItem : SkillItem
{
    public WaterType WaterType { get; }

    public SurfItem(string identifier, ItemCategory itemCategory, WaterType waterType) : base(identifier, itemCategory, Skill.Hover)
    {
        WaterType = waterType;
    }

    public override void Collect()
    {
        base.Collect();
        WorldState.Set($"{WaterType}HoverUnlocked", true);
    }

    public override int GetOwnedQuantity()
    {
        return WorldState.Get($"{WaterType}HoverUnlocked") ? 1 : 0;
    }
}
