namespace MinishootRandomizer;

public class DungeonRewardLocation : Location
{
    public DungeonRewardLocation(
        string identifier,
        string logicRule,
        LocationPool pool
    ) : base(identifier, logicRule, pool)
    {
    }

    public override IPatchAction Accept(ILocationVisitor visitor, Item item)
    {
        return visitor.VisitDungeonReward(this, item);
    }
}
