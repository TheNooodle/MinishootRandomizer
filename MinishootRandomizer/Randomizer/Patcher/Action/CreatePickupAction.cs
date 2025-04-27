using UnityEngine;

namespace MinishootRandomizer;

public class CreatePickupAction : IPatchAction
{
    private readonly PickupManager _pickupManager;

    private readonly RandomizerPickup _pickup;

    private bool _isPatched = false;

    public CreatePickupAction(PickupManager pickupManager, RandomizerPickup pickup)
    {
        _pickupManager = pickupManager;
        _pickup = pickup;
    }

    public void Dispose()
    {
        if (_isPatched)
        {
            _pickupManager.RemovePickup(_pickup);
        }

        Object.Destroy(_pickup.gameObject);
    }

    public void Patch()
    {
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
            return "CreatePickupAction(null)";
        }
        return $"CreatePickupAction({_pickup.Location.Identifier})";
    }
}
