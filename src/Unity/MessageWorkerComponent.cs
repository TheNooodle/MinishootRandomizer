using System.Collections.Generic;
using UnityEngine;

namespace MinishootRandomizer;

public class MessageWorkerComponent: MonoBehaviour
{
    private const float ConsumeInterval = 1f;
    private IMessageConsumer _messageConsumer = null;
    private EventMessageDispatcher _eventMessageDispatcher = null;

    private float _lastConsumeTime = 0;

    void Awake()
    {
        _messageConsumer = Plugin.ServiceContainer.Get<IMessageConsumer>();
        _eventMessageDispatcher = Plugin.ServiceContainer.Get<EventMessageDispatcher>();

        GameEventDispatcher gameEventDispatcher = Plugin.ServiceContainer.Get<GameEventDispatcher>();
        gameEventDispatcher.EnteringGameLocation += OnEnteringGameLocation;

        _eventMessageDispatcher.AfterMessageDispatch += OnAfterMessageDispatch;
    }

    void Update()
    {
        _lastConsumeTime += Time.deltaTime;
        if (_lastConsumeTime > ConsumeInterval)
        {
            _messageConsumer.Consume();
            _lastConsumeTime = 0;
        }
    }

    private void OnAfterMessageDispatch(IMessage message, List<IStamp> stamps)
    {
        Hurry();
    }

    private void OnEnteringGameLocation(string locationName)
    {
        Hurry();
    }

    private void Hurry()
    {
        _lastConsumeTime = ConsumeInterval;
    }
}
