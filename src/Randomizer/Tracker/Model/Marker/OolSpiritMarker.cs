using System;

namespace MinishootRandomizer;

public class OolSpiritMarker : SpiritMarker
{
    public OolSpiritMarker(Location location, string spiritIdentifier) : base(location, spiritIdentifier)
    {
    }

    public override int GetSortIndex()
    {
        return 13;
    }

    public override MarkerSpriteInfo GetSpriteInfo()
    {
        return new MarkerSpriteInfo("SpiritMarkerSimple", new Tuple<float, float>(1.5f, 1.1f));
    }

    public override bool MustShow()
    {
        return !_isChecked && _logicAccessibility == LogicAccessibility.OutOfLogic;
    }
}
