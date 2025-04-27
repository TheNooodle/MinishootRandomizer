namespace MinishootRandomizer;

public class RaceLocation : Location
{
    public RaceLocation(string identifier, string logicRule, LocationPool pool) : base(identifier, logicRule, pool)
    {
    }

    public override IPatchAction Accept(ILocationVisitor visitor, Item item)
    {
        // @TODO: implement
        return new NullAction();
    }
}
