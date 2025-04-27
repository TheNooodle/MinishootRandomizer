using UnityEngine;

namespace MinishootRandomizer;

public class ReplacePickupAction : IPatchAction
{
    private readonly PickupManager _pickupManager;
    private readonly GameObject _originalGameObject;
    private readonly RandomizerPickup _pickup;

    private bool _isPatched = false;

    public ReplacePickupAction(PickupManager pickupManager, GameObject originalGameObject, RandomizerPickup pickup)
    {
        _pickupManager = pickupManager;
        _originalGameObject = originalGameObject;
        _pickup = pickup;
    }

    public void Dispose()
    {
        ForceDeactivationComponent deactivationComponent = _originalGameObject.GetComponent<ForceDeactivationComponent>();
        if (deactivationComponent)
        {
            Object.Destroy(deactivationComponent);
        }

        if (_isPatched)
        {
            _pickupManager.RemovePickup(_pickup);
        }

        Object.Destroy(_pickup.gameObject);
    }

    public void Patch()
    {
        ForceDeactivationComponent deactivationComponent = _originalGameObject.GetComponent<ForceDeactivationComponent>();
        if (!deactivationComponent)
        {
            deactivationComponent = _originalGameObject.AddComponent<ForceDeactivationComponent>();
        }
        deactivationComponent.ForceDeactivation = true;

        if (!_isPatched)
        {
            _pickupManager.AddPickup(_pickup);
            _isPatched = true;
        }

        _pickup.CheckActivation();
    }

    public void Unpatch()
    {
        _pickup.gameObject.SetActive(false);
    }

    public override string ToString()
    {
        if (_pickup.Location == null)
        {
            return $"ReplacePickupAction({_originalGameObject.name})";
        }
        return $"ReplacePickupAction({_pickup.Location.Identifier})";
    }
}
