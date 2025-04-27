using UnityEngine;

namespace MinishootRandomizer;

public class DoorUnlockPatcher
{
    private readonly IRandomizerEngine _randomizerEngine;
    private readonly IObjectFinder _objectFinder;
    private readonly ILogger _logger = new NullLogger();

    private IPatchAction _d1PatchAction = null;
    private IPatchAction _d3PatchAction = null;
    private IPatchAction _cavePatchAction = null;

    public DoorUnlockPatcher(IRandomizerEngine randomizerEngine, IObjectFinder objectFinder, ILogger logger = null)
    {
        _randomizerEngine = randomizerEngine;
        _objectFinder = objectFinder;
        _logger = logger ?? new NullLogger();
    }

    public void OnEnteringGameLocation(string locationName)
    {
        if (!_randomizerEngine.IsRandomized())
        {
            return;
        }

        if (locationName == "Dungeon1" && _d1PatchAction == null)
        {
            PatchDungeon1();
        }
        else if (locationName == "Dungeon3" && _d3PatchAction == null)
        {
            PatchDungeon3();
        }
        else if (locationName == "Cave" && _cavePatchAction == null)
        {
            PatchCave();
        }
    }

    public void OnExitingGame()
    {
        _d1PatchAction?.Dispose();
        _d3PatchAction?.Dispose();
        _cavePatchAction?.Dispose();
    }

    private void PatchDungeon1()
    {
        CompositeAction patchActions = new("Dungeon1Patch");

        GameObject door = _objectFinder.FindObject(new ByName("Dungeon1DoorLocked0"));
        patchActions.Add(new RemoveGameObjectAction(door));

        GameObject unlocker = _objectFinder.FindObject(new ByName("Dungeon1Unlocker3"));
        patchActions.Add(new RemoveGameObjectAction(unlocker));

        _d1PatchAction = new LoggableAction(patchActions, _logger);
        _d1PatchAction.Patch();
    }

    private void PatchDungeon3()
    {
        CompositeAction patchActions = new("Dungeon3Patch");

        GameObject unlocker1 = _objectFinder.FindObject(new ByName("Dungeon3Unlocker6"));
        patchActions.Add(new RemoveGameObjectAction(unlocker1));

        GameObject unlocker2 = _objectFinder.FindObject(new ByName("Dungeon3Unlocker7"));
        patchActions.Add(new MoveGameObjectAction(unlocker2, new Vector3(921f, -54f, 0)));

        GameObject strongDoor = _objectFinder.FindObject(new ByName("Dungeon3StrongDoor0"));
        patchActions.Add(new UnlockStrongDoorAction(strongDoor));

        _d3PatchAction = new LoggableAction(patchActions, _logger);
        _d3PatchAction.Patch();
    }

    private void PatchCave()
    {
        CompositeAction patchActions = new("CavePatch");

        GameObject strongDoor = _objectFinder.FindObject(new ByName("CaveStrongDoor4"));
        patchActions.Add(new UnlockStrongDoorAction(strongDoor));

        _cavePatchAction = new LoggableAction(patchActions, _logger);
        _cavePatchAction.Patch();
    }
}
