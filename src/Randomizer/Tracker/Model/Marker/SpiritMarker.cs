using System;

namespace MinishootRandomizer;

public class SpiritMarker : AbstractMarker
{
    private readonly Location _location;
    private readonly string _spiritIdentifier;

    private bool _mustShow = false;

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
            _mustShow = false;
            return;
        }

        bool owned = WorldState.Get(_spiritIdentifier);
        bool accessible = logicChecker.CheckLocationLogic(_location) == LogicAccessibility.InLogic;

        _mustShow = !owned && accessible;
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
        return _mustShow;
    }
}
