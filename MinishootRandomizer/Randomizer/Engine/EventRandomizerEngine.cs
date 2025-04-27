using System.Collections.Generic;

namespace MinishootRandomizer;

public class EventRandomizerEngine : IRandomizerEngine
{
    private readonly IRandomizerEngine _innerEngine;

    public delegate void GoalCompletedHandler(Goals goal);
    public event GoalCompletedHandler GoalCompleted;

    public EventRandomizerEngine(IRandomizerEngine innerEngine)
    {
        _innerEngine = innerEngine;
    }

    public T GetSetting<T>() where T : ISetting
    {
        return _innerEngine.GetSetting<T>();
    }

    public List<LocationPool> GetLocationPools()
    {
        return _innerEngine.GetLocationPools();
    }

    public List<ISetting> GetSettings()
    {
        return _innerEngine.GetSettings();
    }

    public List<Location> GetRandomizedLocations()
    {
        return _innerEngine.GetRandomizedLocations();
    }

    public Item PeekLocation(Location location)
    {
        return _innerEngine.PeekLocation(location);
    }

    public Item CheckLocation(Location location)
    {
        return _innerEngine.CheckLocation(location);
    }

    public bool IsLocationChecked(Location location)
    {
        return _innerEngine.IsLocationChecked(location);
    }

    public void CompleteGoal(Goals goal)
    {
        _innerEngine.CompleteGoal(goal);
        GoalCompleted?.Invoke(goal);
    }

    public bool IsGoalCompleted(Goals goal)
    {
        return _innerEngine.IsGoalCompleted(goal);
    }

    public bool IsRandomized()
    {
        return _innerEngine.IsRandomized();
    }

    public void SetContext(RandomizerContext context)
    {
        _innerEngine.SetContext(context);
    }

    public void Initialize()
    {
        _innerEngine.Initialize();
    }

    public void Dispose()
    {
        _innerEngine.Dispose();
    }
}
