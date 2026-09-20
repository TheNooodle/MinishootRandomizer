using System.Collections.Generic;

namespace MinishootRandomizer;

// Owns the creation of the tracker map assets (markers + map image) per TrackerMap.
// Assets can be created in one shot (InitializeNow) or unit by unit (ProcessNextUnit)
// to allow background preloading without freezing the game.
public interface ITrackerMapAssetsInitializer
{
    IReadOnlyList<string> MapIdentifiers { get; }
    bool IsInitialized(TrackerMap map);
    bool HasPendingUnits();

    // Processes the next pending unit (one MarkerData batch, or one map image).
    // Returns false when there is nothing left to process.
    bool ProcessNextUnit();

    // Fully initializes the given map (resumes from the units already processed).
    void InitializeNow(TrackerMap map);

    // Discards all state (callers are responsible for destroying the created objects).
    void Reset();
}
