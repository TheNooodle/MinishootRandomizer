using System;
using System.Collections.Generic;

namespace MinishootRandomizer;

public class OolScarabMarker : ScarabMarker
{
    public OolScarabMarker(Dictionary<string, Location> scarabLocations) : base(scarabLocations)
    {
    }

    public override int GetSortIndex()
    {
        return 12;
    }

    public override MarkerSpriteInfo GetSpriteInfo()
    {
        return new MarkerSpriteInfo("ScarabMarkerSimple", new Tuple<float, float>(1.5f, 1.1f));
    }

    public override bool MustShow()
    {
        return _accessibility == LogicAccessibility.OutOfLogic;
    }

    public override float GetAnimationAmplitude()
    {
        return AbstractMarker.OUT_OF_LOGIC_ANIMATION_AMPLITUDE;
    }
}
