using System.Collections.Generic;

namespace MinishootRandomizer;

public class TrackerMap
{
    public string Identifier { get; }
    public string Name { get; }
    public MapSpriteData SpriteData { get; }
    public IReadOnlyList<MarkerData> MarkerDatas { get; }

    public TrackerMap(string identifier, string name, MapSpriteData spriteData, List<MarkerData> markers)
    {
        Identifier = identifier;
        Name = name;
        SpriteData = spriteData;
        MarkerDatas = markers;
    }
}
