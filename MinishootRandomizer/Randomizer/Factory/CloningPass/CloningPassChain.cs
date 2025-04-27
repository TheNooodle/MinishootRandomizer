using System.Collections.Generic;
using UnityEngine;

namespace MinishootRandomizer;

public class CloningPassChain: ICloningPassChain
{
    private readonly List<ICloningPass> _passes = new List<ICloningPass>();

    public void AddPass(ICloningPass pass)
    {
        _passes.Add(pass);
    }

    public void ApplyPasses(GameObject existingObject, GameObject cloneObject, CloningType cloningType)
    {
        foreach (ICloningPass pass in _passes)
        {
            if (pass.CanApply(cloningType))
            {
                pass.Apply(existingObject, cloneObject);
            }
        }
    }
}
