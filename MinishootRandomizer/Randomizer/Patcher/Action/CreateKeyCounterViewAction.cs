using UnityEngine;

namespace MinishootRandomizer;

public class CreateKeyCounterViewAction : IPatchAction
{
    private readonly IKeyCounterObjectFactory _keyCounterObjectFactory;

    private GameObject _keyCounterViewObject = null;

    public CreateKeyCounterViewAction(IKeyCounterObjectFactory keyCounterObjectFactory)
    {
        _keyCounterObjectFactory = keyCounterObjectFactory;
    }
    
    public void Dispose()
    {
        if (_keyCounterViewObject != null)
        {
            GameObject.Destroy(_keyCounterViewObject);
            _keyCounterViewObject = null;
        }
    }

    public void Patch()
    {
        _keyCounterViewObject = _keyCounterObjectFactory.CreateKeyCounterGameObject();
    }

    public void Unpatch()
    {
        // no-op
    }
}
