using UnityEngine;

namespace MinishootRandomizer;

public class MoveGameObjectAction : IPatchAction
{
    private readonly GameObject _gameObject;
    private readonly Vector3 _position;

    private bool _isPatched = false;
    private Vector3 _originalPosition;

    public MoveGameObjectAction(GameObject gameObject, Vector3 position)
    {
        _gameObject = gameObject;
        _position = position;
    }

    public void Dispose()
    {
        if (_isPatched)
        {
            _gameObject.transform.position = _originalPosition;
            _isPatched = false;
        }
    }

    public void Patch()
    {
        if (_isPatched)
        {
            return;
        }

        _originalPosition = _gameObject.transform.position;
        _gameObject.transform.position = _position;
        _isPatched = true;
    }

    public void Unpatch()
    {
        // no-op
    }
}
