using System;
using System.Collections.Generic;

namespace MinishootRandomizer;

public class LocationMarker : AbstractMarker
{
    private List<Location> _locations = new List<Location>();
    protected LogicAccessibility _accessibility = LogicAccessibility.Inaccessible;

    public LocationMarker(List<Location> locations)
    {
        _locations = locations;
    }

    public override void ComputeVisibility(IRandomizerEngine engine, ILocationLogicChecker logicChecker, ILogicStateProvider logicStateProvider)
    {
        List<LocationPool> locationPools = engine.GetLocationPools();
        LogicAccessibility newAccessibility = LogicAccessibility.Inaccessible;
        LogicState logicState = logicStateProvider.GetLogicState();
        foreach (Location location in _locations)
        {
            bool isChecked = engine.IsLocationChecked(location);
            bool isInPool = locationPools.Contains(location.Pool);
            LogicAccessibility accessibility = logicChecker.CheckLocationLogic(logicState, location);
            if (!isChecked && isInPool)
            {
                if (accessibility == LogicAccessibility.InLogic)
                {
                    // If any location is in logic, the marker is in logic.
                    // We can stop checking the other locations.
                    newAccessibility = LogicAccessibility.InLogic;
                    break;
                }
                else if (accessibility == LogicAccessibility.OutOfLogic)
                {
                    // If no location is in logic, but it still accessible out of logic, the marker is out of logic.
                    // We continue to check the other locations to see if any of them is in logic.
                    newAccessibility = LogicAccessibility.OutOfLogic;
                }
            }
        }

        _accessibility = newAccessibility;
    }

    public override bool MustShow()
    {
        return _accessibility == LogicAccessibility.InLogic;
    }

    public override MarkerSpriteInfo GetSpriteInfo()
    {
        return new MarkerSpriteInfo("LocationMarker", new Tuple<float, float>(1.0f, 1.0f));
    }

    public override int GetSortIndex()
    {
        return 0;
    }

    public override float GetAnimationAmplitude()
    {
        return AbstractMarker.IN_LOGIC_ANIMATION_AMPLITUDE;
    }
}
