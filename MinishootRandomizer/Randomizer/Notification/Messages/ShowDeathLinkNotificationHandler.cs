using System;

namespace MinishootRandomizer;

public class ShowDeathLinkNotificationHandler : IMessageHandler
{
    public void Handle(IMessage message)
    {
        if (!(message is ShowDeathLinkNotificationMessage))
        {
            throw new ArgumentException("Message is not of type ShowDeathLinkNotificationMessage");
        }

        ShowDeathLinkNotificationMessage notificationMessage = (ShowDeathLinkNotificationMessage)message;
        notificationMessage.NotificationHUDViewComponent.NotifyDeathLink(notificationMessage.Source);
    }
}
