using System;

namespace MinishootRandomizer;

public class NpcMarker : AbstractMarker
{
    private Location _location;
    private string _npcIdentifier;

    private bool _mustShow = false;

    public Location Location => _location;
    public string NpcIdentifier => _npcIdentifier;

    public NpcMarker(Location location, string npcIdentifier)
    {
        _location = location;
        _npcIdentifier = npcIdentifier;
    }

    public override void ComputeVisibility(IRandomizerEngine engine, ILogicChecker logicChecker)
    {
        NpcSanity npcSanity = engine.GetSetting<NpcSanity>();
        if (npcSanity.Enabled)
        {
            _mustShow = false;
            return;
        }

        bool owned = WorldState.Get(_npcIdentifier);
        bool accessible = logicChecker.CheckLocationLogic(_location) == LogicAccessibility.InLogic;

        _mustShow = !owned && accessible;
    }

    public override MarkerSpriteInfo GetSpriteInfo()
    {
        return new MarkerSpriteInfo("NpcMarker", new Tuple<float, float>(1.5f, 1.1f));
    }

    public override bool MustShow()
    {
        return _mustShow;
    }

    public override int GetSortIndex()
    {
        return 1;
    }
}
