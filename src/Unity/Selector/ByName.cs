using System;

namespace MinishootRandomizer;

public class ByName : ISelector
{
    private string _name;
    private Type _type = null;
    private bool _includeInactive = true;

    public string Name => _name;
    public Type Type => _type;
    public bool IncludeInactive => _includeInactive;

    public ByName(string name, Type type = null, bool includeInactive = true)
    {
        _name = name;
        _type = type;
        _includeInactive = includeInactive;
    }

    public override string ToString()
    {
        return _name;
    }
}
