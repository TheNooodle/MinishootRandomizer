using UnityEngine;

namespace MinishootRandomizer;

public class ForceDeactivationComponent : MonoBehaviour
{
    public bool ForceDeactivation { get; set; } = false;

    void Update()
    {
        if (ForceDeactivation)
        {
            gameObject.SetActive(false);
        }
    }
}
