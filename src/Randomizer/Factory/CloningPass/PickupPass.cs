using UnityEngine;

namespace MinishootRandomizer;

public class PickupPass : ICloningPass
{
    public void Apply(GameObject existingObject, GameObject cloneObject)
    {
        Pickup originalPickup = existingObject.GetComponent<Pickup>();
        if (originalPickup == null)
        {
            return;
        }

        RandomizerPickup pickupClone = cloneObject.AddComponent<RandomizerPickup>();
        pickupClone.InitializeFromPickup((Pickup)originalPickup);
    }

    public bool CanApply(CloningType cloningType)
    {
        return cloningType == CloningType.Recreate;
    }
}
