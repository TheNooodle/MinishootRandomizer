using UnityEngine;

namespace MinishootRandomizer;

public interface ICloningPass
{
    bool CanApply(CloningType cloningType);
    void Apply(GameObject existingObject, GameObject cloneObject);
}
