using UnityEngine;

namespace MinishootRandomizer;

public class MetaCloningPass : ICloningPass
{
    public void Apply(GameObject existingObject, GameObject cloneObject)
    {
        cloneObject.name = "Randomizer Clone - " + existingObject.name;
        cloneObject.SetTag(existingObject.tag);
        cloneObject.SetLayer(existingObject.layer);
    }

    public bool CanApply(CloningType cloningType)
    {
        return true;
    }
}
