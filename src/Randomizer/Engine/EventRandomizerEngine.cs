using System.Collections.Generic;

namespace MinishootRandomizer;

public class EventRandomizerEngine : IRandomizerEngine
{
    private readonly IRandomizerEngine _innerEngine;

    public delegate void OnCheckLocationHandler(Location location, Item item);
    public event OnCheckLocationHandler OnCheckLocation;

    public delegate void OnCompleteGoalHandler(Goals goal);
    public event OnCompleteGoalHandler OnCompleteGoal;

    public EventRandomizerEngine(IRandomizerEngine innerEngine)
    {
        _innerEngine = innerEngine;
    }

    public Item CheckLocation(Location location)
    {
        Item item = _innerEngine.CheckLocation(location);
        OnCheckLocation?.Invoke(location, item);

        return item;
    }

    public void CompleteGoal(Goals goal)
    {
        _innerEngine.CompleteGoal(goal);
        OnCompleteGoal?.Invoke(goal);
    }

    public void Dispose()
    {
        _innerEngine.Dispose();
    }

    public List<Location> GetRandomizedLocations()
    {
        return _innerEngine.GetRandomizedLocations();
    }

    public T GetSetting<T>() where T : ISetting
    {
        return _innerEngine.GetSetting<T>();
    }

    public List<ISetting> GetSettings()
    {
        return _innerEngine.GetSettings();
    }

    public void Initialize()
    {
        _innerEngine.Initialize();
    }

    public bool IsLocationChecked(Location location)
    {
        return _innerEngine.IsLocationChecked(location);
    }

    public bool IsRandomized()
    {
        return _innerEngine.IsRandomized();
    }

    public Item PeekLocation(Location location)
    {
        return _innerEngine.PeekLocation(location);
    }

    public void SetContext(RandomizerContext context)
    {
        _innerEngine.SetContext(context);
    }
}
