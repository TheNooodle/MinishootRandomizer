namespace MinishootRandomizer;

public class ProgressiveDashItem : Item
{
    public ProgressiveDashItem(string identifier, ItemCategory category) : base(identifier, category)
    {
    }

    public override void Collect()
    {
        // Unlock the Dash skill if it is not already unlocked.
        // If it is already unlocked, unlock the Spirit Dash module instead.
        if (!PlayerState.Skills.TryGetValue(Skill.Dash, out bool dashOwned) || !dashOwned)
        {
            PlayerState.SetSkill(Skill.Dash, unlocked: true);
        }
        else
        {
            PlayerState.SetModule(Modules.SpiritDash, true);
        }
    }

    public override int GetOwnedQuantity()
    {
        int count = 0;
        count += PlayerState.Skills.TryGetValue(Skill.Dash, out bool owned) && owned ? 1 : 0;
        count += PlayerState.Modules.TryGetValue(Modules.SpiritDash, out bool owned2) && owned2 ? 1 : 0;

        return count;
    }
}
