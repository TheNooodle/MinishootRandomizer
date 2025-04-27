using System;

namespace MinishootRandomizer;

public class ShowItemNotificationHandler : IMessageHandler
{
    public void Handle(IMessage message)
    {
        if (!(message is ShowItemNotificationMessage))
        {
            throw new ArgumentException("Message is not of type SendGoalMessage");
        }

        ShowItemNotificationMessage notificationMessage = (ShowItemNotificationMessage)message;
        notificationMessage.NotificationHUDViewComponent.NotifyItemCollection(notificationMessage.Item);
    }
}
