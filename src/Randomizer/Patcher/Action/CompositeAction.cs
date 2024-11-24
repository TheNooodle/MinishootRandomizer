using System.Collections.Generic;

namespace MinishootRandomizer;

public class CompositeAction : IPatchAction
{
    private string _name = null;
    private List<IPatchAction> _actions = new List<IPatchAction>();

    public CompositeAction(string name = null)
    {
        _name = name;
    }

    public void Add(IPatchAction action)
    {
        _actions.Add(action);
    }

    public void Dispose()
    {
        foreach (IPatchAction action in _actions)
        {
            action.Dispose();
        }
    }

    public void Patch()
    {
        foreach (IPatchAction action in _actions)
        {
            action.Patch();
        }
    }

    public void Unpatch()
    {
        foreach (IPatchAction action in _actions)
        {
            action.Unpatch();
        }
    }

    public override string ToString()
    {
        if (_name == null)
        {
            return "CompositeAction with " + _actions.Count + " actions";
        }
        return _name + " with " + _actions.Count + " actions";
    }
}
