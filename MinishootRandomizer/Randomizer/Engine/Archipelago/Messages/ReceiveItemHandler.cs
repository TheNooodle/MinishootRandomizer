using System;

namespace MinishootRandomizer;

public class ReceiveItemHandler : IMessageHandler
{
    private readonly GameEventDispatcher _gameEventDispatcher;

    public ReceiveItemHandler(GameEventDispatcher gameEventDispatcher)
    {
        _gameEventDispatcher = gameEventDispatcher;
    }

    public void Handle(IMessage message)
    {
        if (!(message is ReceiveItemMessage))
        {
            throw new ArgumentException("Message is not of type ReceiveItemMessage");
        }

        Item item = ((ReceiveItemMessage)message).Item;
        item.Collect();
        _gameEventDispatcher.DispatchItemCollected(item);
    }
}
