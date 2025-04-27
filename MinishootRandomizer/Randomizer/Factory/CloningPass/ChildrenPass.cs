using UnityEngine;

namespace MinishootRandomizer;

public class ChildrenPass : ICloningPass
{
    public void Apply(GameObject existingObject, GameObject cloneObject)
    {
        foreach (Transform child in existingObject.transform)
        {
            GameObject childClone = UnityEngine.Object.Instantiate(child.gameObject);
            childClone.transform.SetParent(cloneObject.transform);
        }
    }

    public bool CanApply(CloningType cloningType)
    {
        return cloningType == CloningType.Recreate;
    }
}
