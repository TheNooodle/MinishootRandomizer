using System;

namespace MinishootRandomizer;

public class ObjectiveMarker : AbstractMarker
{
    private Location _location;
    private Goals _goal;

    private bool _isChecked = false;
    private LogicAccessibility _logicAccessibility = LogicAccessibility.Inaccessible;

    public ObjectiveMarker(Location location, Goals goal)
    {
        _location = location;
        _goal = goal;
    }

    public override void ComputeVisibility(IRandomizerEngine engine, ILogicChecker logicChecker)
    {
        CompletionGoals completionGoals = engine.GetSetting<CompletionGoals>();
        if (engine.IsGoalCompleted(_goal) || (completionGoals.Goal != Goals.Both && completionGoals.Goal != _goal))
        {
            return;
        }

        _logicAccessibility = logicChecker.CheckLocationLogic(_location);
        _isChecked = engine.IsLocationChecked(_location);
    }

    public override int GetSortIndex()
    {
        return -1;
    }

    public override MarkerSpriteInfo GetSpriteInfo()
    {
        return new MarkerSpriteInfo(_logicAccessibility == LogicAccessibility.OutOfLogic ? "SkullMarkerSimple" : "SkullMarker", new Tuple<float, float>(1.5f, 1.1f));
    }

    public override bool MustShow()
    {
        return !_isChecked && _logicAccessibility != LogicAccessibility.Inaccessible;
    }
}
