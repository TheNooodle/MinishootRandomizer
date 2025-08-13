namespace MinishootRandomizer;

public class IsNotificationComponentBusy : IStamp
{
    private readonly NotificationHUDViewComponent _notificationHUDViewComponent;

    public IsNotificationComponentBusy(NotificationHUDViewComponent notificationHUDViewComponent)
    {
        _notificationHUDViewComponent = notificationHUDViewComponent;
    }

    public bool CanHandle(IMessage message)
    {
        return !_notificationHUDViewComponent.IsBusy();
    }
}
