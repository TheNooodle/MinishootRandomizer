namespace MinishootRandomizer;

// This class acts as a facade between the randomizer engine and the game events that occur during the game.
// It exists to decouple the randomizer engine from the game events (handles by various managers), for better maintainability.
public class GameEventDispatcher
{
    private ILogger _logger = new NullLogger();

    // Loading a save file means the player is starting a new game or loading a saved game.
    public delegate void LoadingSaveFileHandler(bool isNewGame);
    public event LoadingSaveFileHandler LoadingSaveFile;

    // Entering a game location means the player is entering a new area in the game (e.g. the overworld, a dungeon, a cave, etc.)
    // Note that this event will be dispatched when a save file is loaded (whether it's a new game or a saved game).
    public delegate void EnteringGameLocationHandler(string locationName);
    public event EnteringGameLocationHandler EnteringGameLocation;

    // Exiting a game means the player is returning to the main menu.
    public delegate void ExitingGameHandler();
    public event ExitingGameHandler ExitingGame;

    // NpcFreed means the player has either freed an NPC or received an NPC item.
    public delegate void NpcFreedHandler();
    public event NpcFreedHandler NpcFreed;

    // PlayerStatsChanged means the player's stats have changed (e.g. health, upgrades, etc.).
    public delegate void PlayerStatsChangedHandler();
    public event PlayerStatsChangedHandler PlayerStatsChanged;

    // PlayerCurrencyChanged means the player's currency has changed (e.g. super crystals, scarabs, lore tablets).
    public delegate void PlayerCurrencyChangedHandler(Currency currency);
    public event PlayerCurrencyChangedHandler PlayerCurrencyChanged;

    // Entering an encounter means the player is entering a battle against waves of enemies.
    public delegate void EnteringEncounterHandler();
    public event EnteringEncounterHandler EnteringEncounter;

    // Exiting an encounter means the player has defeated all waves of enemies or died in battle.
    public delegate void ExitingEncounterHandler();
    public event ExitingEncounterHandler ExitingEncounter;

    // An item has been collected, whether it was by the player themselves or received remotely.
    // This event is fired after the player state has been updated with the new item.
    // So it is safe to assume that the player has the item in their inventory.
    public delegate void ItemCollectedHandler(Item item);
    public event ItemCollectedHandler ItemCollected;

    public delegate void RaceWonHandler(int raceIndex);
    public event RaceWonHandler RaceWon;

    public delegate void OnPlayerDeathHandler(string source);
    public event OnPlayerDeathHandler OnPlayerDeath;

    public GameEventDispatcher(ILogger logger)
    {
        _logger = logger ?? new NullLogger();
    }

    public void DispatchLoadingSaveFile(bool isNewGame)
    {
        _logger.LogDebug("Dispatching LoadingSaveFile event with isNewGame: " + isNewGame);
        LoadingSaveFile?.Invoke(isNewGame);
    }

    public void DispatchEnteringGameLocation(string locationName)
    {
        _logger.LogDebug($"Dispatching EnteringGameLocation event with location name: {locationName}");
        EnteringGameLocation?.Invoke(locationName);
    }

    public void DispatchExitingGame()
    {
        _logger.LogDebug("Dispatching ExitingGame event");
        ExitingGame?.Invoke();
    }

    public void DispatchNpcFreed()
    {
        _logger.LogDebug("Dispatching NpcFreed event");
        NpcFreed?.Invoke();
    }

    public void DispatchPlayerStatsChanged()
    {
        _logger.LogDebug("Dispatching PlayerStatsChanged event");
        PlayerStatsChanged?.Invoke();
    }

    public void DispatchPlayerCurrencyChanged(Currency currency)
    {
        _logger.LogDebug("Dispatching PlayerCurrencyChanged event");
        PlayerCurrencyChanged?.Invoke(currency);
    }

    public void DispatchEnteringEncounter()
    {
        _logger.LogDebug("Dispatching EnteringEncounter event");
        EnteringEncounter?.Invoke();
    }

    public void DispatchExitingEncounter()
    {
        _logger.LogDebug("Dispatching ExitingEncounter event");
        ExitingEncounter?.Invoke();
    }

    public void DispatchItemCollected(Item item)
    {
        _logger.LogDebug($"Dispatching ItemCollected event with item: {item}");
        ItemCollected?.Invoke(item);
    }

    public void DispatchRaceWon(int raceIndex)
    {
        _logger.LogDebug($"Dispatching RaceWon event with index: {raceIndex}");
        RaceWon?.Invoke(raceIndex);
    }

    public void DispatchOnPlayerDeath(string source)
    {
        _logger.LogDebug("Dispatching OnPlayerDeath event");
        OnPlayerDeath?.Invoke(source);
    }
}
