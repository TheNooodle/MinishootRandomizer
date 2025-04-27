namespace MinishootRandomizer;

public class CanNotifyItem : IStamp
{
    private readonly NotificationHUDViewComponent _notificationHUDViewComponent;

    public CanNotifyItem(NotificationHUDViewComponent notificationHUDViewComponent)
    {
        _notificationHUDViewComponent = notificationHUDViewComponent;
    }

    public bool CanHandle(IMessage message)
    {
        return !_notificationHUDViewComponent.IsBusy();
    }
}
