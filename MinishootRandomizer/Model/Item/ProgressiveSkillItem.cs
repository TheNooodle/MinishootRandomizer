namespace MinishootRandomizer;

public class ProgressiveSkillItem : Item
{
    private readonly Skill _skill;
    private readonly Modules _module;

    public ProgressiveSkillItem(string identifier, ItemCategory category, Skill skill, Modules module) : base(identifier, category)
    {
        _skill = skill;
        _module = module;
    }

    public override void Collect()
    {
        // Unlock the skill if it is not already unlocked.
        // If it is already unlocked, unlock the module instead.
        if (!PlayerState.Skills.TryGetValue(_skill, out bool skillOwned) || !skillOwned)
        {
            PlayerState.SetSkill(_skill, unlocked: true);
        }
        else
        {
            PlayerState.SetModule(_module, true);
        }
    }

    public override int GetOwnedQuantity()
    {
        int count = 0;
        count += PlayerState.Skills.TryGetValue(_skill, out bool owned) && owned ? 1 : 0;
        count += PlayerState.Modules.TryGetValue(_module, out bool owned2) && owned2 ? 1 : 0;

        return count;
    }
}
