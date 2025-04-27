using UnityEngine;

namespace MinishootRandomizer;

public class ColliderPass : ICloningPass
{
    public void Apply(GameObject existingObject, GameObject cloneObject)
    {
        foreach (Component component in existingObject.GetComponents<Component>())
        {
            if (component is BoxCollider2D)
            {
                BoxCollider2D originalCollider = (BoxCollider2D)component;
                BoxCollider2D colliderClone = cloneObject.AddComponent<BoxCollider2D>();
                colliderClone.autoTiling = originalCollider.autoTiling;
                colliderClone.edgeRadius = originalCollider.edgeRadius;
                colliderClone.size = originalCollider.size;
                colliderClone.enabled = originalCollider.enabled;
                colliderClone.isTrigger = originalCollider.isTrigger;
                colliderClone.offset = originalCollider.offset;
                colliderClone.tag = originalCollider.tag;

                return;
            }
            else if (component is CircleCollider2D)
            {
                CircleCollider2D originalCollider = (CircleCollider2D)component;
                CircleCollider2D colliderClone = cloneObject.AddComponent<CircleCollider2D>();
                colliderClone.radius = originalCollider.radius;
                colliderClone.enabled = originalCollider.enabled;
                colliderClone.isTrigger = originalCollider.isTrigger;
                colliderClone.offset = originalCollider.offset;
                colliderClone.tag = originalCollider.tag;

                return;
            }
        }
    }

    public bool CanApply(CloningType cloningType)
    {
        return cloningType == CloningType.Recreate;
    }
}
