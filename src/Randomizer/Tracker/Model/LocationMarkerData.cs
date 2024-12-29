using System;
using System.Collections.Generic;

namespace MinishootRandomizer;

public class LocationMarkerData
{
    private readonly List<string> _locationNames;
    private readonly List<Tuple<float, float>> _coordinates;

    public List<string> LocationNames => _locationNames;
    public List<Tuple<float, float>> Coordinates => _coordinates;

    public LocationMarkerData(List<string> locationNames, List<Tuple<float, float>> coordinates)
    {
        _locationNames = locationNames;
        _coordinates = coordinates;
    }
}
