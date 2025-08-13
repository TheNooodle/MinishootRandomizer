using UnityEngine;

namespace MinishootRandomizer;

public class CanKillPlayer : IStamp
{
    private IObjectFinder _objectFinder;

    public CanKillPlayer(IObjectFinder objectFinder)
    {
        _objectFinder = objectFinder;
    }

    public bool CanHandle(IMessage message)
    {
        GameObject playerObject = _objectFinder.FindObject(new ByComponent(typeof(Player)));
        if (playerObject == null)
        {
            return false;
        }
        Destroyable destroyable = playerObject.GetComponent<Destroyable>();
        if (destroyable == null)
        {
            return false;
        }

        return !destroyable.Invincible;
    }
}
