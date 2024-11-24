using System;

namespace MinishootRandomizer;

public class ByComponent : ISelector
{
    private Type _type;
    private bool _includeInactive = true;

    public Type Type => _type;
    public bool IncludeInactive => _includeInactive;

    public ByComponent(Type type, bool includeInactive = true)
    {
        _type = type;
        _includeInactive = includeInactive;
    }

    public override string ToString()
    {
        return _type.Name;
    }
}
