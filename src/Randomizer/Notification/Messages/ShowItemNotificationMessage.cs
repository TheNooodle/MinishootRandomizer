namespace MinishootRandomizer;

public class ShowItemNotificationMessage: IMessage
{
    private readonly NotificationHUDViewComponent _notificationHUDViewComponent;
    private readonly Item _item;

    public NotificationHUDViewComponent NotificationHUDViewComponent => _notificationHUDViewComponent;
    public Item Item => _item;

    // Notifications interacts with Unity objects, so it should be executed in the main thread.
    public MessageQueue MessageQueue => MessageQueue.MainThread;

    public ShowItemNotificationMessage(NotificationHUDViewComponent component, Item item)
    {
        _notificationHUDViewComponent = component;
        _item = item;
    }
}
