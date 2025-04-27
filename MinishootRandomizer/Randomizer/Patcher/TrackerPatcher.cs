using System.Collections.Generic;
using UnityEngine;

namespace MinishootRandomizer;

public class TrackerPatcher
{
    private readonly IRandomizerEngine _randomizerEngine;
    private readonly IObjectFinder _objectFinder;
    private readonly IMarkerFactory _markerFactory;
    private readonly ILogger _logger = new NullLogger();

    private IPatchAction _patchAction = null;
    private List<RandomizerTrackerMarkerComponent> _markers = new List<RandomizerTrackerMarkerComponent>();

    public TrackerPatcher(IRandomizerEngine randomizerEngine, IObjectFinder objectFinder, IMarkerFactory markerFactory, ILogger logger = null)
    {
        _randomizerEngine = randomizerEngine;
        _objectFinder = objectFinder;
        _markerFactory = markerFactory;
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

    public void OnItemCollected(Item item)
    {
        foreach (RandomizerTrackerMarkerComponent marker in _markers)
        {
            marker.CheckActivation();
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

        // Add the new markers
        List<GameObject> newMarkers = _markerFactory.CreateMarkerObjects();
        foreach (GameObject newMarker in newMarkers)
        {
            RandomizerTrackerMarkerComponent randomizerLocationMarker = newMarker.GetComponent<RandomizerTrackerMarkerComponent>();
            if (randomizerLocationMarker == null)
            {
                throw new InvalidActionException($"Replacement GameObject {newMarker.name} does not have a RandomizerLocationMarker component");
            }
            _markers.Add(randomizerLocationMarker);
            compositeAction.Add(new CreateGameObjectAction(newMarker));
        }

        return new LoggableAction(compositeAction, _logger);
    }
}
