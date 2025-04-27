namespace MinishootRandomizer;

public class GoalLocation : Location
{
    private readonly Goals _goal;

    public Goals Goal => _goal;

    public GoalLocation(string identifier, string logicRule, LocationPool pool, Goals goal) : base(identifier, logicRule, pool)
    {
        _goal = goal;
    }

    public override IPatchAction Accept(ILocationVisitor visitor, Item item)
    {
        return visitor.VisitGoal(this, item);
    }
}
