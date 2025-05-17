using System;
using System.Collections.Generic;

namespace MinishootRandomizer;

/// <summary>
/// Decorator for LogicState that considers certain item counts and settings as "out of logic".
/// It is still a LogicState, but will serve to categorize certain locations accessible as out of logic.
/// </summary>
public class OutOfLogicStateDecorator : LogicState
{
    private readonly LogicState _baseLogicState;
    
    public OutOfLogicStateDecorator(LogicState baseLogicState)
    {
        _baseLogicState = baseLogicState;
    }

    private Dictionary<Type, ISetting> _outOfLogicSettings = new Dictionary<Type, ISetting>()
    {
        {typeof(IgnoreCannonLevelRequirements), new IgnoreCannonLevelRequirements(true)},
        {typeof(BoostlessSpringboards), new BoostlessSpringboards(true)},
        {typeof(BoostlessSpiritRaces), new BoostlessSpiritRaces(true)},
        {typeof(BoostlessTorchRaces), new BoostlessTorchRaces(true)},
        {typeof(EnablePrimordialCrystalLogic), new EnablePrimordialCrystalLogic(true)},
        {typeof(DashlessGaps), new DashlessGaps(DashlessGapsValue.NeedsNeither)},
    };

    public override bool HasItem(Item item, int count = 1)
    {
        return _baseLogicState.HasItem(item, count);
    }

    public override T GetSetting<T>()
    {
        if (_outOfLogicSettings.TryGetValue(typeof(T), out ISetting setting))
        {
            return (T)setting;
        }

        return _baseLogicState.GetSetting<T>();
    }

    public override string GetCacheKey()
    {
        return LogicTolerance.Lenient.ToString();
    }
}
