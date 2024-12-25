namespace MinishootRandomizer;

public class SkillItem : Item
{
    public SkillItem(string identifier, ItemCategory itemCategory, Skill skill) : base(identifier, itemCategory)
    {
        Skill = skill;
    }

    public Skill Skill { get; }

    public override void Collect()
    {
        PlayerState.SetSkill(Skill, unlocked: true);
    }

    public override int GetOwnedQuantity()
    {
        return PlayerState.Skills.TryGetValue(Skill, out bool owned) && owned ? 1 : 0;
    }
}
