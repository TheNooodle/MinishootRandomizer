namespace MinishootRandomizer;

public class ShowDeathLinkNotificationMessage : IMessage
{
    public MessageQueue MessageQueue => MessageQueue.MainThread;

    private readonly NotificationHUDViewComponent _notificationHUDViewComponent;
    private readonly string _source;

    public NotificationHUDViewComponent NotificationHUDViewComponent => _notificationHUDViewComponent;
    public string Source => _source;

    public ShowDeathLinkNotificationMessage(NotificationHUDViewComponent notificationHUDViewComponent, string source)
    {
        _notificationHUDViewComponent = notificationHUDViewComponent;
        _source = source;
    }
}
