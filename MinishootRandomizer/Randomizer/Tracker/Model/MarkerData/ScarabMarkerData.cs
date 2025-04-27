using System.Collections.Generic;

namespace MinishootRandomizer;

public class ScarabMarkerData
{
    private readonly Dictionary<string, string> _scarabLocationMap;

    public Dictionary<string, string> ScarabLocationMap => _scarabLocationMap;

    public ScarabMarkerData(Dictionary<string, string> scarabLocationMap)
    {
        _scarabLocationMap = scarabLocationMap;
    }
}
