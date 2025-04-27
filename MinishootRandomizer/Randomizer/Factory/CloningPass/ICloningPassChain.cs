using UnityEngine;

namespace MinishootRandomizer;

public interface ICloningPassChain
{
    void AddPass(ICloningPass pass);
    void ApplyPasses(GameObject existingObject, GameObject cloneObject, CloningType cloningType);
}
