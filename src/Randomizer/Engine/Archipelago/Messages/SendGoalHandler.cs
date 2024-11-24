using System;

namespace MinishootRandomizer;

public class SendGoalHandler : IMessageHandler
{
    private readonly IArchipelagoClient _client;

    public SendGoalHandler(IArchipelagoClient client)
    {
        _client = client;
    }

    public void Handle(IMessage message)
    {
        if (!(message is SendGoalMessage))
        {
            throw new ArgumentException("Message is not of type SendGoalMessage");
        }

        if (((SendGoalMessage)message).SendCompletion)
        {
            _client.SendCompletion();
        }
    }
}
