using System.Collections.Generic;

namespace MinishootRandomizer;

public class DeathLinkManager
{
    private readonly IMessageDispatcher _messageDispatcher;
    private readonly IObjectFinder _objectFinder;
    private readonly ILogger _logger;

    public DeathLinkManager(IMessageDispatcher messageDispatcher, IObjectFinder objectFinder, ILogger logger = null)
    {
        _messageDispatcher = messageDispatcher;
        _objectFinder = objectFinder;
        _logger = logger ?? new NullLogger();
    }

    public void OnPlayerDeath(string source)
    {
        if (source == "DeathLink")
        {
            return; // Ignore death links from the DeathLink system itself
        }

        _logger.LogInfo("Player has died, sending death link message.");
        _messageDispatcher.Dispatch(new SendDeathLinkMessage());
    }

    public void OnDeathLink(string playerName)
    {
        _logger.LogInfo($"Death link received from player: {playerName}");
        _messageDispatcher.Dispatch(new KillPlayerMessage(playerName), new List<IStamp>
        {
            new MustBeInGameStamp(),
            new CanKillPlayer(_objectFinder) // We pass the object finder to the stamp to find the player object on the main thread.
        });
    }
}
