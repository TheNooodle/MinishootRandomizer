using System;
using System.Collections.Generic;

namespace MinishootRandomizer;

public class ContextualRandomizerEngine : IRandomizerEngine
{
    private readonly IRandomizerEngine _archipelagoEngine;
    private readonly IRandomizerEngine _vanillaEngine;
    private readonly IRandomizerEngine _dummyEngine;
    private readonly IRandomizerContextProvider _contextProvider;
    private readonly ILogger _logger = new NullLogger();

    private RandomizerContext _context;

    public ContextualRandomizerEngine(
        IRandomizerEngine archipelagoEngine,
        IRandomizerEngine vanillaEngine,
        IRandomizerEngine dummyEngine,
        IRandomizerContextProvider contextProvider,
        ILogger logger = null
    ) {
        _archipelagoEngine = archipelagoEngine;
        _vanillaEngine = vanillaEngine;
        _dummyEngine = dummyEngine;
        _contextProvider = contextProvider;
        _logger = logger ?? new NullLogger();
    }

    private IRandomizerEngine GetEngine()
    {
        if (Plugin.UseDummyEngine)
        {
            return _dummyEngine;
        }

        if (_context == null)
        {
            throw new ArgumentException("ContextualRandomizerEngine has not been initialized.");
        }

        if (_context is ArchipelagoContext)
        {
            return _archipelagoEngine;
        }

        if (_context is VanillaContext)
        {
            return _vanillaEngine;
        }

        throw new ArgumentException("ContextualRandomizerEngine does not support the provided context.");
    }

    public Item CheckLocation(Location location)
    {
        return GetEngine().CheckLocation(location);
    }

    public void CompleteGoal(Goals goal)
    {
        GetEngine().CompleteGoal(goal);
    }

    public bool IsGoalCompleted(Goals goal)
    {
        return GetEngine().IsGoalCompleted(goal);
    }

    public List<Location> GetRandomizedLocations()
    {
        return GetEngine().GetRandomizedLocations();
    }

    public T GetSetting<T>() where T : ISetting
    {
        return GetEngine().GetSetting<T>();
    }

    public List<ISetting> GetSettings()
    {
        return GetEngine().GetSettings();
    }

    public List<LocationPool> GetLocationPools()
    {
        return GetEngine().GetLocationPools();
    }

    public bool IsLocationChecked(Location location)
    {
        return GetEngine().IsLocationChecked(location);
    }

    public Item PeekLocation(Location location)
    {
        return GetEngine().PeekLocation(location);
    }

    public bool IsRandomized()
    {
        return GetEngine().IsRandomized();
    }

    public void SetContext(RandomizerContext context)
    {
        throw new ArgumentException("ContextualRandomizerEngine does not support setting context directly.");
    }

    public void Initialize()
    {
        _context = _contextProvider.GetContext();
        IRandomizerEngine engine = GetEngine();
        _logger.LogInfo($"Initializing ContextualRandomizerEngine with context {_context.GetType().Name}");
        engine.SetContext(_context);
        engine.Initialize();
    }

    public void Dispose()
    {
        if (_context == null)
        {
            return;
        }

        _logger.LogInfo($"Disposing ContextualRandomizerEngine with context {_context.GetType().Name}");
        GetEngine().Dispose();
        _context = null;
    }
}
