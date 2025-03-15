using UnityEngine;

namespace MinishootRandomizer;

public class UnlockStrongDoorAction : IPatchAction
{
    private readonly GameObject _door;

    public UnlockStrongDoorAction(GameObject door)
    {
        _door = door;
    }

    public void Dispose()
    {
        // no-op
    }

    public void Patch()
    {
        WorldState.Set(_door.name, true);
        StrongDoor component = _door.GetComponent<StrongDoor>();
        component.ForceUpdateState();
    }

    public void Unpatch()
    {
        // no-op
    }
}
