using UnityEngine;

namespace MinishootRandomizer;

public class RandomizerManagerComponent: MonoBehaviour
{
    private GameEventDispatcher _gameEventDispatcher = null;
    private ILogger _logger = new NullLogger();

    void Awake()
    {
        _gameEventDispatcher = Plugin.ServiceContainer.Get<GameEventDispatcher>();
        _logger = Plugin.ServiceContainer.Get<ILogger>();

        _logger.LogInfo("RandomizerManager is created!");
    }

    void Start()
    {
        _logger.LogInfo("RandomizerManager is started!");
        LocationManager.LocationSet += OnGameLocationLoaded;
        GameManager.GameStateLoaded += OnGameStateLoaded;
        GameManager.GameReset += OnGameReset;
        Player.StatsChanged += _gameEventDispatcher.DispatchPlayerStatsChanged;
        CrystalNpc.Freed += _gameEventDispatcher.DispatchNpcFreed;
        EncounterClose.StaticBegun += _gameEventDispatcher.DispatchEnteringEncounter;
        EncounterClose.StaticEnded += _gameEventDispatcher.DispatchExitingEncounter;
    }

    void OnDestory()
    {
        _logger.LogInfo("RandomizerManager is destroyed!");
    }

    private void OnGameLocationLoaded()
    {
        var currentLocation = LocationManager.Current;
        if (currentLocation != null)
        {
            _gameEventDispatcher.DispatchEnteringGameLocation(currentLocation.name);
        }
        else
        {
            _logger.LogInfo("Game location loaded: null");
        }
    }

    private void OnGameStateLoaded()
    {
        if (GameManager.State == GameState.Game)
        {
            return;
        }

        bool isNewGame = PlayerState.Crystalized;
        _gameEventDispatcher.DispatchLoadingSaveFile(isNewGame);
    }

    private void OnGameReset()
    {
        _gameEventDispatcher.DispatchExitingGame();
    }
}
