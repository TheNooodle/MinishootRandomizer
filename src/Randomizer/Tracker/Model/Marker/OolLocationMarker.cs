using System;
using System.Collections.Generic;

namespace MinishootRandomizer;

public class OolLocationMarker : LocationMarker
{
    public OolLocationMarker(List<Location> locations) : base(locations)
    {
    }

    public override bool MustShow()
    {
        return _accessibility == LogicAccessibility.OutOfLogic;
    }

    public override MarkerSpriteInfo GetSpriteInfo()
    {
        return new MarkerSpriteInfo("LocationMarkerSimple", new Tuple<float, float>(1.0f, 1.0f));
    }

    public override int GetSortIndex()
    {
        return 10;
    }
}
