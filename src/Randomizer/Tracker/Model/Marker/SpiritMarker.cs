using System;

namespace MinishootRandomizer;

public class SpiritMarker : AbstractMarker
{
    private readonly Location _location;
    private readonly string _spiritIdentifier;

    private bool _isChecked = false;
    private LogicAccessibility _logicAccessibility = LogicAccessibility.Inaccessible;

    public Location Location => _location;
    public string SpiritIdentifier => _spiritIdentifier;

    public SpiritMarker(Location location, string spiritIdentifier)
    {
        _location = location;
        _spiritIdentifier = spiritIdentifier;
    }

    public override void ComputeVisibility(IRandomizerEngine engine, ILogicChecker logicChecker)
    {
        SpiritSanity spiritSanity = engine.GetSetting<SpiritSanity>();
        if (spiritSanity.Enabled)
        {
            _isChecked = true;
            return;
        }

        _isChecked = WorldState.Get(_spiritIdentifier);
        _logicAccessibility = logicChecker.CheckLocationLogic(_location);
    }

    public override int GetSortIndex()
    {
        return 3;
    }

    public override MarkerSpriteInfo GetSpriteInfo()
    {
        return new MarkerSpriteInfo(_logicAccessibility == LogicAccessibility.OutOfLogic ? "SpiritMarkerSimple" : "SpiritMarker", new Tuple<float, float>(1.5f, 1.1f));
    }

    public override bool MustShow()
    {
        return !_isChecked && _logicAccessibility != LogicAccessibility.Inaccessible;
    }
}
