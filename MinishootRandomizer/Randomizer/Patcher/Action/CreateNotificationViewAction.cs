using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MinishootRandomizer;

public class CreateNotificationViewAction : IPatchAction
{
    private readonly INotificationObjectFactory _notificationObjectFactory;

    private GameObject _notificationViewObject = null;

    public CreateNotificationViewAction(INotificationObjectFactory notificationObjectFactory)
    {
        _notificationObjectFactory = notificationObjectFactory;
    }
    
    public void Dispose()
    {
        if (_notificationViewObject != null)
        {
            GameObject.Destroy(_notificationViewObject);
            _notificationViewObject = null;
        }
    }

    public void Patch()
    {
        _notificationViewObject = _notificationObjectFactory.CreateNotificationView();
    }

    public void Unpatch()
    {
        // no-op
    }
}
