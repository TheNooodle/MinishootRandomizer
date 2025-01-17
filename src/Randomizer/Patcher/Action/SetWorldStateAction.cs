namespace MinishootRandomizer;

public class SetWorldStateAction : IPatchAction
{
    private readonly string _key;
    private readonly bool _value;

    public SetWorldStateAction(string key, bool value = true)
    {
        _key = key;
        _value = value;
    }

    public void Dispose()
    {
        // no-op
    }

    public void Patch()
    {
        WorldState.Set(_key, _value);
    }

    public void Unpatch()
    {
        WorldState.Set(_key, !_value);
    }

    public override string ToString()
    {
        return $"Set WorldState{_key} = {_value}";
    }
}
