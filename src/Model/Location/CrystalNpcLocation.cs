using UnityEngine;

namespace MinishootRandomizer;

public class CrystalNpcLocation: Location
{
    private ISelector _selector;

    public ISelector Selector => _selector;

    public CrystalNpcLocation(
        string identifier,
        string logicRule,
        LocationPool pool,
        ISelector selector
    ): base(identifier, logicRule, pool)
    {
        _selector = selector;
    }

    public override IPatchAction Accept(ILocationVisitor visitor, Item item)
    {
        return visitor.VisitCrystalNpc(this, item);
    }
}
