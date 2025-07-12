namespace MinishootRandomizer;

public class SpiritLocation : Location
{
    private readonly int _index;

    public int Index => _index;

    public SpiritLocation(string identifier, string logicRule, LocationPool pool, int index) : base(identifier, logicRule, pool)
    {
        _index = index;
    }

    public override IPatchAction Accept(ILocationVisitor visitor, Item item)
    {
        return visitor.VisitSpirit(this, item);
    }
}
