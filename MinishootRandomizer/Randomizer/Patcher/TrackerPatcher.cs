using System.Collections.Generic;
using UnityEngine;

namespace MinishootRandomizer;

public class TrackerPatcher
{
    private readonly IRandomizerEngine _randomizerEngine;
    private readonly IObjectFinder _objectFinder;
    private readonly ILogger _logger = new NullLogger();

    private IPatchAction _patchAction = null;
    private List<RandomizerTrackerMarkerComponent> _markers = new List<RandomizerTrackerMarkerComponent>();

    public TrackerPatcher(IRandomizerEngine randomizerEngine, IObjectFinder objectFinder, ILogger logger = null)
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

        if (_patchAction == null)
        {
            _patchAction = CreatePatchAction();
            _patchAction.Patch();
        }
    }

    internal void OnExitingGame()
    {
        if (_patchAction != null)
        {
            _patchAction.Dispose();
            _patchAction = null;
            _markers.Clear();
        }
    }

    private IPatchAction CreatePatchAction()
    {
        CompositeAction compositeAction = new CompositeAction("Tracker");

        // Remove the existing markers
        GameObject[] markers = _objectFinder.FindObjects(new ByComponent(typeof(MapMarkerView)));
        foreach (GameObject marker in markers)
        {
            compositeAction.Add(new RemoveGameObjectAction(marker));
        }

        // Create the tracker component
        GameObject mapObject = _objectFinder.FindObject(new ByComponent(typeof(Map)));
        if (mapObject == null)
        {
            _logger.LogError("Could not find Map object");
        }
        else
        {
            AddComponentAction<RandomizerMapComponent> addMapComponentAction = new AddComponentAction<RandomizerMapComponent>(mapObject);
            compositeAction.Add(addMapComponentAction);
        }

        return new LoggableAction(compositeAction, _logger);
    }
}
