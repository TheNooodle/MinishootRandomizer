using System;
using System.Collections.Generic;

namespace MinishootRandomizer;

public class VanillaRandomizerEngine : IRandomizerEngine
{
    public Item CheckLocation(Location location)
    {
        throw new ArgumentException("CheckLocation should not be called on VanillaRandomizerEngine.");
    }

    public void CompleteGoal(Goals goal)
    {
        throw new ArgumentException("CompleteGoal should not be called on VanillaRandomizerEngine.");
    }

    public bool IsGoalCompleted(Goals goal)
    {
        throw new ArgumentException("IsGoalCompleted should not be called on VanillaRandomizerEngine.");
    }

    public void Dispose()
    {
        // no-op
    }

    public List<Location> GetRandomizedLocations()
    {
        throw new ArgumentException("GetRandomizedLocations should not be called on VanillaRandomizerEngine.");
    }

    public T GetSetting<T>() where T : ISetting
    {
        throw new ArgumentException("GetSetting should not be called on VanillaRandomizerEngine.");
    }

    public List<ISetting> GetSettings()
    {
        throw new ArgumentException("GetSettings should not be called on VanillaRandomizerEngine.");
    }

    public List<LocationPool> GetLocationPools()
    {
        throw new ArgumentException("GetLocationPools should not be called on VanillaRandomizerEngine.");
    }

    public void Initialize()
    {
        // no-op
    }

    public bool IsLocationChecked(Location location)
    {
        throw new ArgumentException("IsLocationChecked should not be called on VanillaRandomizerEngine.");
    }

    public bool IsRandomized()
    {
        return false;
    }

    public Item PeekLocation(Location location)
    {
        throw new ArgumentException("PeekLocation should not be called on VanillaRandomizerEngine.");
    }

    public void SetContext(RandomizerContext context)
    {
        // no-op
    }
}
