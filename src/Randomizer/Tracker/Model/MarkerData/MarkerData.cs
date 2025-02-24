using System;
using System.Collections.Generic;

namespace MinishootRandomizer;

public class MarkerData
{
    private readonly List<string> _locationNames;
    private readonly List<Tuple<float, float>> _coordinates;
    private readonly string _markerName;
    private readonly NpcMarkerData _npcMarkerData = null;
    private readonly ScarabMarkerData _scarabMarkerData = null;
    private readonly SpiritMarkerData _spiritMarkerData = null;
    private readonly ObjectiveMarkerData _objectiveMarkerData = null;

    public List<string> LocationNames => _locationNames;
    public List<Tuple<float, float>> Coordinates => _coordinates;
    public string MarkerName => _markerName;
    public NpcMarkerData NpcMarkerData => _npcMarkerData;
    public ScarabMarkerData ScarabMarkerData => _scarabMarkerData;
    public SpiritMarkerData SpiritMarkerData => _spiritMarkerData;
    public ObjectiveMarkerData ObjectiveMarkerData => _objectiveMarkerData;

    public MarkerData(
        List<string> locationNames,
        List<Tuple<float, float>> coordinates,
        string markerName = null,
        NpcMarkerData npcMarkerData = null,
        ScarabMarkerData scarabMarkerData = null,
        SpiritMarkerData spiritMarkerData = null,
        ObjectiveMarkerData objectiveMarkerData = null
    ) {
        _locationNames = locationNames;
        _coordinates = coordinates;
        _markerName = markerName;
        _npcMarkerData = npcMarkerData;
        _scarabMarkerData = scarabMarkerData;
        _spiritMarkerData = spiritMarkerData;
        _objectiveMarkerData = objectiveMarkerData;
    }

    public override string ToString()
    {
        return $"LocationMarker: {MarkerName ?? LocationNames[0]}";
    }
}
