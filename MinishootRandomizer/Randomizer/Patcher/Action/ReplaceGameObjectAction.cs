using UnityEngine;

namespace MinishootRandomizer;

public class ReplaceGameObjectAction : IPatchAction
{
    private readonly GameObject _originalGameObject;
    private readonly GameObject _replacementGameObject;

    public ReplaceGameObjectAction(GameObject originalGameObject, GameObject replacementGameObject)
    {
        _originalGameObject = originalGameObject;
        _replacementGameObject = replacementGameObject;

        if (_replacementGameObject.GetComponent<IActivationChecker>() == null)
        {
            throw new InvalidActionException($"Replacement GameObject {_replacementGameObject.name} does not have an IActivationChecker component");
        }
    }

    public void Dispose()
    {
        ForceDeactivationComponent deactivationComponent = _originalGameObject.GetComponent<ForceDeactivationComponent>();
        if (deactivationComponent)
        {
            GameObject.Destroy(deactivationComponent);
        }

        GameObject.Destroy(_replacementGameObject);
    }

    public void Patch()
    {
        ForceDeactivationComponent deactivationComponent = _originalGameObject.GetComponent<ForceDeactivationComponent>();
        if (!deactivationComponent)
        {
            deactivationComponent = _originalGameObject.AddComponent<ForceDeactivationComponent>();
        }
        deactivationComponent.ForceDeactivation = true;

        _replacementGameObject.GetComponent<IActivationChecker>().CheckActivation();
    }

    public void Unpatch()
    {
        _replacementGameObject.SetActive(false);
    }

    public override string ToString()
    {
        return $"ReplaceGameObjectAction({_originalGameObject.name}, {_replacementGameObject.name})";
    }
}
