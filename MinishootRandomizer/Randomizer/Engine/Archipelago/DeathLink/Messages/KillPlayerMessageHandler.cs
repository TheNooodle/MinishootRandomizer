using System;
using UnityEngine;

namespace MinishootRandomizer;

public class KillPlayerMessageHandler : IMessageHandler
{
    private readonly IObjectFinder _objectFinder;
    private readonly INotificationManager _notificationManager;
    private readonly ILogger _logger;

    public KillPlayerMessageHandler(IObjectFinder objectFinder, INotificationManager notificationManager, ILogger logger = null)
    {
        _objectFinder = objectFinder;
        _notificationManager = notificationManager;
        _logger = logger ?? new NullLogger();
    }

    public void Handle(IMessage message)
    {
        if (!(message is KillPlayerMessage))
        {
            throw new ArgumentException("Message is not of type KillPlayerMessage");
        }

        GameObject playerObject = _objectFinder.FindObject(new ByComponent(typeof(Player)));
        if (playerObject == null)
        {
            _logger.LogWarning("Player object not found in the scene!");
            return;
        }

        Destroyable destroyable = playerObject.GetComponent<Destroyable>();
        if (destroyable == null)
        {
            _logger.LogWarning("Destroyable component not found on player object!");
            return;
        }
        // Use the source "DeathLink" to avoid triggering the death link again.
        // Alos, notification is sent here because we need to be on the main thread to access Unity objects
        // (which is not the case with the MultiClient).
        destroyable.GetDamaged(999f, "DeathLink", byPassModuleCheck: true);
        _notificationManager.OnDeathLinkReceived(((KillPlayerMessage)message).Source);
    }
}
