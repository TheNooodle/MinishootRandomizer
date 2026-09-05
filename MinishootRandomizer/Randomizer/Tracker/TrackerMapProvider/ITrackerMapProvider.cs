using System.Collections.Generic;

namespace MinishootRandomizer;

public interface ITrackerMapProvider
{
    TrackerMap GetTrackerMap(string Identifier);
    List<TrackerMap> GetAllTrackerMaps();
}
