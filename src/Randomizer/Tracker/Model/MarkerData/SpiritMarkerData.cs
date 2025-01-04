namespace MinishootRandomizer;

public class SpiritMarkerData
{
    private readonly string _locationName;
    private readonly string _spiritIdentifier;

    public string LocationName => _locationName;
    public string SpiritIdentifier => _spiritIdentifier;

    public SpiritMarkerData(string locationName, string spiritIdentifier)
    {
        _locationName = locationName;
        _spiritIdentifier = spiritIdentifier;
    }
}
