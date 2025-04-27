using System;
using System.Collections.Generic;
using UnityEngine;

namespace MinishootRandomizer;

public class NotificationManager
{
    private readonly IObjectFinder _objectFinder;
    private readonly IMessageDispatcher _messageDispatcher;

    private NotificationHUDViewComponent _notificationHUDViewComponent = null;

    public NotificationManager(IObjectFinder objectFinder, IMessageDispatcher messageDispatcher)
    {
        _objectFinder = objectFinder;
        _messageDispatcher = messageDispatcher;
    }

    public void OnItemCollected(Item item)
    {
        if (item is DungeonRewardItem)
        {
            return;
        }

        NotificationHUDViewComponent notificationComponent = GetNotificationComponent();

        _messageDispatcher.Dispatch(
            new ShowItemNotificationMessage(notificationComponent, item),
            new List<IStamp> {
                new MustBeInGameStamp(),
                new CanNotifyItem(notificationComponent)
            }
        );
    }

    public void OnExitingGame()
    {
        _notificationHUDViewComponent = null;
    }

    private NotificationHUDViewComponent GetNotificationComponent()
    {
        if (_notificationHUDViewComponent == null)
        {
            GameObject notificationHUDViewObject = _objectFinder.FindObject(new ByComponent(typeof(NotificationHUDViewComponent)));
            if (notificationHUDViewObject == null)
            {
                throw new Exception("NotificationHUDViewComponent not found!");
            }

            _notificationHUDViewComponent = notificationHUDViewObject.GetComponent<NotificationHUDViewComponent>();
        }

        return _notificationHUDViewComponent;
    }
}
