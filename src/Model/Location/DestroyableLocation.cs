using UnityEngine;

namespace MinishootRandomizer;

public class DestroyableLocation : Location
{
    private readonly ISelector _destroyableSelector;
    private readonly Vector3 _offset;
    private readonly ISelector _itemSelector = null;

    public ISelector DestroyableSelector => _destroyableSelector;
    public Vector3 Offset => _offset;
    public ISelector ItemSelector => _itemSelector;

    public DestroyableLocation(
        string identifier,
        string logicRule,
        LocationPool pool,
        ISelector destroyableSelector,
        Vector3 offset,
        ISelector itemSelector = null
    ) : base(identifier, logicRule, pool)
    {
        _destroyableSelector = destroyableSelector;
        _offset = offset;
        _itemSelector = itemSelector;
    }

    public override IPatchAction Accept(ILocationVisitor visitor, Item item)
    {
        return visitor.VisitDestroyable(this, item);
    }
}

public class DestroyableReplacementData
{
    public ISelector DestroyableSelector { get; set; }
    public Vector3 Offset { get; set; }
    public ISelector ItemSelector { get; set; }

    public DestroyableReplacementData(ISelector destroyableSelector, Vector3 offset, ISelector itemSelector = null)
    {
        DestroyableSelector = destroyableSelector;
        Offset = offset;
        ItemSelector = itemSelector;
    }
}
