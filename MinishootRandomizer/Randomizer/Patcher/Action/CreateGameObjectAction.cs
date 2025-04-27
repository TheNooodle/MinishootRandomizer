using UnityEngine;

namespace MinishootRandomizer;

public class CreateGameObjectAction : IPatchAction
{
    private readonly GameObject _gameObject;

    public CreateGameObjectAction(GameObject gameObject)
    {
        _gameObject = gameObject;

        if (_gameObject.GetComponent<IActivationChecker>() == null)
        {
            throw new InvalidActionException($"Replacement GameObject {_gameObject.name} does not have an IActivationChecker component");
        }
    }

    public void Dispose()
    {
        GameObject.Destroy(_gameObject);
    }

    public void Patch()
    {
        _gameObject.GetComponent<IActivationChecker>().CheckActivation();
    }

    public void Unpatch()
    {
        _gameObject.SetActive(false);
    }

    public override string ToString()
    {
        return $"CreateGameObjectAction({_gameObject.name})";
    }
}
