using System;
using System.Collections.Generic;

namespace MinishootRandomizer;

public class ScarabMarker : AbstractMarker
{
    private readonly Dictionary<string, Location> _scarabLocations;

    protected LogicAccessibility _accessibility = LogicAccessibility.Inaccessible;

    public Dictionary<string, Location> ScarabLocations => _scarabLocations;

    public ScarabMarker(Dictionary<string, Location> scarabLocations)
    {
        _scarabLocations = scarabLocations;
    }

    public override void ComputeVisibility(IRandomizerEngine engine, ILocationLogicChecker logicChecker, ILogicStateProvider logicStateProvider)
    {
        ScarabSanity scarabSanity = engine.GetSetting<ScarabSanity>();
        LogicAccessibility newAccessibility = LogicAccessibility.Inaccessible;
        if (scarabSanity.Enabled)
        {
            _accessibility = LogicAccessibility.Inaccessible;
            return;
        }

        LogicState logicState = logicStateProvider.GetLogicState();
        foreach (KeyValuePair<string, Location> scarabLocation in _scarabLocations)
        {
            bool owned = WorldState.Get(scarabLocation.Key);

            if (!owned)
            {
                if (logicChecker.CheckLocationLogic(logicState, scarabLocation.Value) == LogicAccessibility.InLogic)
                {
                    newAccessibility = LogicAccessibility.InLogic;
                    break;
                }
                else if (logicChecker.CheckLocationLogic(logicState, scarabLocation.Value) == LogicAccessibility.OutOfLogic)
                {
                    newAccessibility = LogicAccessibility.OutOfLogic;
                }
            }
        }

        _accessibility = newAccessibility;
    }

    public override int GetSortIndex()
    {
        return 2;
    }

    public override MarkerSpriteInfo GetSpriteInfo()
    {
        return new MarkerSpriteInfo("ScarabMarker", new Tuple<float, float>(1.5f, 1.1f));
    }

    public override bool MustShow()
    {
        return _accessibility == LogicAccessibility.InLogic;
    }

    public override float GetAnimationAmplitude()
    {
        return AbstractMarker.IN_LOGIC_ANIMATION_AMPLITUDE;
    }
}
