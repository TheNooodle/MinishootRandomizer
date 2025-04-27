namespace MinishootRandomizer;

public class NpcMarkerData
{
    private readonly string _locationName;
    private readonly string _npcIdentifier;

    public string LocationName => _locationName;
    public string NpcIdentifier => _npcIdentifier;

    public NpcMarkerData(string locationName, string npcIdentifier)
    {
        _locationName = locationName;
        _npcIdentifier = npcIdentifier;
    }
}
