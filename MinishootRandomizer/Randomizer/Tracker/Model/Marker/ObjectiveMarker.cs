using System;

namespace MinishootRandomizer;

public class ObjectiveMarker : AbstractMarker
{
    private Location _location;
    private Goals _goal;

    protected bool _isChecked = false;
    protected LogicAccessibility _logicAccessibility = LogicAccessibility.Inaccessible;

    public ObjectiveMarker(Location location, Goals goal)
    {
        _location = location;
        _goal = goal;
    }

    public override void ComputeVisibility(IRandomizerEngine engine, ILocationLogicChecker logicChecker, ILogicStateProvider logicStateProvider)
    {
        CompletionGoals completionGoals = engine.GetSetting<CompletionGoals>();
        if (engine.IsGoalCompleted(_goal) || (completionGoals.Goal != Goals.Dungeon5AndSnow && completionGoals.Goal != _goal))
        {
            return;
        }

        LogicState logicState = logicStateProvider.GetLogicState();
        _logicAccessibility = logicChecker.CheckLocationLogic(logicState, _location);
        _isChecked = engine.IsLocationChecked(_location);
    }

    public override int GetSortIndex()
    {
        return -1;
    }

    public override MarkerSpriteInfo GetSpriteInfo()
    {
        return new MarkerSpriteInfo("SkullMarker", new Tuple<float, float>(1.5f, 1.1f));
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
