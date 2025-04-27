using UnityEngine;

namespace MinishootRandomizer;

public class WorldStateChecker : MonoBehaviour, IActivationChecker
{
    public bool CheckActivation()
    {
        bool active = !WorldState.Get(name);
        gameObject.SetActive(active);

        return active;
    }
}
