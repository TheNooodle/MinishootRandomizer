using System.Collections.Generic;

namespace MinishootRandomizer;

public class ShopReplacementData
{
    private string _npcName;
    private List<string> _locationNames;

    public string NpcName => _npcName;
    public List<string> LocationNames => _locationNames;

    public ShopReplacementData(string npcName, List<string> locationNames)
    {
        _npcName = npcName;
        _locationNames = locationNames;
    }
}
