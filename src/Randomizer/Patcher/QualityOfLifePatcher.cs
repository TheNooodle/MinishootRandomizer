using System.Collections.Generic;
using UnityEngine;

namespace MinishootRandomizer;

public class QualityOfLifePatcher
{
    private readonly IRandomizerEngine _randomizerEngine;
    private readonly IObjectFinder _objectFinder;
    private readonly ILogger _logger = new NullLogger();

    private IPatchAction _patchAction = null;

    private List<NpcIds> _npcsToPatch = new()
    {
        NpcIds.Bard,
        NpcIds.Blacksmith,
        NpcIds.ScarabCollector,
        NpcIds.MercantHub,
        NpcIds.UnchosenBlue,
        NpcIds.Explorer,
        NpcIds.Healer,
        NpcIds.Academician
    };

    public QualityOfLifePatcher(IRandomizerEngine randomizerEngine, IObjectFinder objectFinder, ILogger logger = null)
    {
        _randomizerEngine = randomizerEngine;
        _objectFinder = objectFinder;
        _logger = logger ?? new NullLogger();
    }

    public void OnEnteringGameLocation(string location)
    {
        if (!_randomizerEngine.IsRandomized())
        {
            return;
        }

        if (_patchAction == null)
        {
            _patchAction = CreatePatch();
            _patchAction.Patch();
        }
    }

    public void OnExitingGame()
    {
        if (_patchAction != null)
        {
            _patchAction.Dispose();
            _patchAction = null;
        }
    }

    private IPatchAction CreatePatch()
    {
        CompositeAction compositeAction = new CompositeAction("QualityOfLife");

        // Remove the control mod select popup
        GameObject trigger = _objectFinder.FindObject(new ByName("PopUpControlModeTrigger"));
        if (trigger != null)
        {
            compositeAction.Add(new RemoveGameObjectAction(trigger));
        }

        // Introduce all npcs to skip their cutscenes
        foreach (NpcIds npcId in _npcsToPatch)
        {
            compositeAction.Add(new SetWorldStateAction(npcId.Str() + "Introduced"));
        }

        // Patch the cutscene that plays when you meet the primordial scarab.
        compositeAction.Add(new SetWorldStateAction("PrimordialDialogsMeet"));

        return new LoggableAction(compositeAction, _logger);
    }
}
