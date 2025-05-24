using System;

namespace MinishootRandomizer;

public class NpcMarker : AbstractMarker
{
    private Location _location;
    private string _npcIdentifier;

    protected bool _owned = false;
    protected LogicAccessibility _accessibility = LogicAccessibility.Inaccessible;

    public Location Location => _location;
    public string NpcIdentifier => _npcIdentifier;

    public NpcMarker(Location location, string npcIdentifier)
    {
        _location = location;
        _npcIdentifier = npcIdentifier;
    }

    public override void ComputeVisibility(IRandomizerEngine engine, ILocationLogicChecker logicChecker, ILogicStateProvider logicStateProvider)
    {
        NpcSanity npcSanity = engine.GetSetting<NpcSanity>();
        if (npcSanity.Enabled)
        {
            _owned = true;
            return;
        }

        LogicState logicState = logicStateProvider.GetLogicState();
        _owned = WorldState.Get(_npcIdentifier);
        _accessibility = logicChecker.CheckLocationLogic(logicState, _location);
    }

    public override MarkerSpriteInfo GetSpriteInfo()
    {
        return new MarkerSpriteInfo("NpcMarker", new Tuple<float, float>(1.5f, 1.1f));
    }

    public override bool MustShow()
    {
        return !_owned && _accessibility == LogicAccessibility.InLogic;
    }

    public override int GetSortIndex()
    {
        return 1;
    }

    public override float GetAnimationAmplitude()
    {
        return AbstractMarker.IN_LOGIC_ANIMATION_AMPLITUDE;
    }
}
