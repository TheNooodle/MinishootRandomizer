using System;

namespace MinishootRandomizer;

public class SpiritMarker : AbstractMarker
{
    private readonly Location _location;
    private readonly string _spiritIdentifier;

    protected bool _isChecked = false;
    protected LogicAccessibility _logicAccessibility = LogicAccessibility.Inaccessible;

    public Location Location => _location;
    public string SpiritIdentifier => _spiritIdentifier;

    public SpiritMarker(Location location, string spiritIdentifier)
    {
        _location = location;
        _spiritIdentifier = spiritIdentifier;
    }

    public override void ComputeVisibility(IRandomizerEngine engine, ILocationLogicChecker logicChecker, ILogicStateProvider logicStateProvider)
    {
        SpiritSanity spiritSanity = engine.GetSetting<SpiritSanity>();
        if (spiritSanity.Enabled)
        {
            _isChecked = true;
            return;
        }

        _isChecked = WorldState.Get(_spiritIdentifier);
        LogicState logicState = logicStateProvider.GetLogicState();
        _logicAccessibility = logicChecker.CheckLocationLogic(logicState, _location);
    }

    public override int GetSortIndex()
    {
        return 3;
    }

    public override MarkerSpriteInfo GetSpriteInfo()
    {
        return new MarkerSpriteInfo("SpiritMarker", new Tuple<float, float>(1.5f, 1.1f));
    }

    public override bool MustShow()
    {
        return !_isChecked && _logicAccessibility == LogicAccessibility.InLogic;
    }
    
    public override float GetAnimationAmplitude()
    {
        return AbstractMarker.IN_LOGIC_ANIMATION_AMPLITUDE;
    }
}
