using System;
using System.Collections.Generic;

namespace MinishootRandomizer;

public class ScarabMarker : AbstractMarker
{
    private readonly Dictionary<string, Location> _scarabLocations;
    private bool _mustShow = false;

    public Dictionary<string, Location> ScarabLocations => _scarabLocations;

    public ScarabMarker(Dictionary<string, Location> scarabLocations)
    {
        _scarabLocations = scarabLocations;
    }

    public override void ComputeVisibility(IRandomizerEngine engine, ILogicChecker logicChecker)
    {
        ScarabSanity scarabSanity = engine.GetSetting<ScarabSanity>();
        if (scarabSanity.Enabled)
        {
            _mustShow = false;
            return;
        }

        bool mustShow = false;
        foreach (KeyValuePair<string, Location> scarabLocation in _scarabLocations)
        {
            bool owned = WorldState.Get(scarabLocation.Key);
            bool accessible = logicChecker.CheckLocationLogic(scarabLocation.Value) == LogicAccessibility.InLogic;

            if (!owned && accessible)
            {
                mustShow = true;
                break;
            }
        }

        _mustShow = mustShow;
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
        return _mustShow;
    }
}
