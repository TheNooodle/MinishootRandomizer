using System;

namespace MinishootRandomizer;

public class OolNpcMarker : NpcMarker
{
    public OolNpcMarker(Location location, string npcIdentifier) : base(location, npcIdentifier)
    {
    }

    public override MarkerSpriteInfo GetSpriteInfo()
    {
        return new MarkerSpriteInfo("NpcMarkerSimple", new Tuple<float, float>(1.5f, 1.1f));
    }

    public override bool MustShow()
    {
        return !_owned && _accessibility == LogicAccessibility.OutOfLogic;
    }

    public override int GetSortIndex()
    {
        return 11;
    }
}
