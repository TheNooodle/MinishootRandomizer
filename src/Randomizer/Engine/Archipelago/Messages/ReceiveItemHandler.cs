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

        switch (item.Category)
        {
            case ItemCategory.Progression:
                Player.Emote.Play(Emotes.Happy, 0.8f);
                break;
            case ItemCategory.Trap:
                Player.Emote.Play(Emotes.Shameful, 0.8f);
                break;
            default:
                Player.Emote.Play(Emotes.Ok, 0.8f);
                break;
        }
    }
}
