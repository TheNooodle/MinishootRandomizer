using System;

namespace MinishootRandomizer;

public class OolObjectiveMarker : ObjectiveMarker
{
    public OolObjectiveMarker(Location location, Goals goal) : base(location, goal)
    {
    }

    public override int GetSortIndex()
    {
        return 9;
    }

    public override MarkerSpriteInfo GetSpriteInfo()
    {
        return new MarkerSpriteInfo("SkullMarkerSimple", new Tuple<float, float>(1.5f, 1.1f));
    }

    public override bool MustShow()
    {
        return !_isChecked && _logicAccessibility == LogicAccessibility.OutOfLogic;
    }

    public override float GetAnimationAmplitude()
    {
        return AbstractMarker.OUT_OF_LOGIC_ANIMATION_AMPLITUDE;
    }
}
