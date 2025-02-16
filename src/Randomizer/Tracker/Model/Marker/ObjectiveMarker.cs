using System;

namespace MinishootRandomizer;

public class ObjectiveMarker : AbstractMarker
{
    private Location _location;
    private Goals _goal;
    private bool _mustShow = false;

    public ObjectiveMarker(Location location, Goals goal)
    {
        _location = location;
        _goal = goal;
    }

    public override void ComputeVisibility(IRandomizerEngine engine, ILogicChecker logicChecker)
    {
        _mustShow = false;
        CompletionGoals completionGoals = engine.GetSetting<CompletionGoals>();
        if (engine.IsGoalCompleted(_goal) || (completionGoals.Goal != Goals.Both && completionGoals.Goal != _goal))
        {
            return;
        }

        LogicAccessibility logicAccessibility = logicChecker.CheckLocationLogic(_location);
        bool isChecked = engine.IsLocationChecked(_location);

        _mustShow = logicAccessibility == LogicAccessibility.InLogic && !isChecked;
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
        return _mustShow;
    }
}
