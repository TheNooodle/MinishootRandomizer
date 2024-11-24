using UnityEngine;

namespace MinishootRandomizer;

public class RemoveGameObjectAction : IPatchAction
{
    private readonly GameObject _gameObject;

    public RemoveGameObjectAction(GameObject gameObject)
    {
        _gameObject = gameObject;
    }

    public void Dispose()
    {
        ForceDeactivationComponent deactivationComponent = _gameObject.GetComponent<ForceDeactivationComponent>();
        if (deactivationComponent)
        {
            GameObject.Destroy(deactivationComponent);
        }
    }

    public void Patch()
    {
        ForceDeactivationComponent deactivationComponent = _gameObject.GetComponent<ForceDeactivationComponent>();
        if (!deactivationComponent)
        {
            deactivationComponent = _gameObject.AddComponent<ForceDeactivationComponent>();
        }
        deactivationComponent.ForceDeactivation = true;
    }

    public void Unpatch()
    {
        // no-op
    }

    public override string ToString()
    {
        return $"RemoveGameObjectAction({_gameObject.name})";
    }
}
