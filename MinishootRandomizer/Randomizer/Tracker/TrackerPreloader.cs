using System.Diagnostics;

namespace MinishootRandomizer;

// Drives the background preloading of the tracker map assets, spread over multiple
// frames with a time budget so that opening the map never freezes the game.
public class TrackerPreloader
{
    private const float StartDelaySeconds = 3f;
    private const double FrameBudgetMilliseconds = 8.0;

    private enum Phase
    {
        Off,
        Delayed,
        Running,
        Done,
    }

    private readonly ITrackerMapAssetsInitializer _assetsInitializer;
    private readonly IRandomizerEngine _randomizerEngine;
    private readonly ILogger _logger;
    private readonly Stopwatch _stopwatch = new Stopwatch();

    private Phase _phase = Phase.Off;
    private float _delayTimer = 0f;

    public TrackerPreloader(
        ITrackerMapAssetsInitializer assetsInitializer,
        IRandomizerEngine randomizerEngine,
        ILogger logger = null
    )
    {
        _assetsInitializer = assetsInitializer;
        _randomizerEngine = randomizerEngine;
        _logger = logger ?? new NullLogger();
    }

    // Called once the tracker patch has been applied (the Map object and marker parent exist).
    public void NotifySessionStarted()
    {
        _delayTimer = 0f;
        _phase = Phase.Delayed;
        _logger.LogInfo("Tracker preload scheduled.");
    }

    public void Reset()
    {
        _phase = Phase.Off;
        _delayTimer = 0f;
        _assetsInitializer.Reset();
    }

    // Must be called once per frame by a persistent MonoBehaviour host.
    public void Tick(float deltaTime)
    {
        switch (_phase)
        {
            case Phase.Delayed:
                _delayTimer += deltaTime;
                if (_delayTimer >= StartDelaySeconds)
                {
                    _phase = Phase.Running;
                }
                break;

            case Phase.Running:
                TickRunning();
                break;
        }
    }

    private void TickRunning()
    {
        if (!_randomizerEngine.IsRandomized())
        {
            _phase = Phase.Off;
            return;
        }

        _stopwatch.Restart();
        while (_stopwatch.Elapsed.TotalMilliseconds < FrameBudgetMilliseconds)
        {
            if (!_assetsInitializer.ProcessNextUnit())
            {
                _stopwatch.Stop();
                _phase = Phase.Done;
                _logger.LogInfo("Tracker preload finished.");
                return;
            }
        }
        _stopwatch.Stop();
    }
}
