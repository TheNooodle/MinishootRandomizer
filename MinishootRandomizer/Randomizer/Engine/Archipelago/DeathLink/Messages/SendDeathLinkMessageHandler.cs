using System;

namespace MinishootRandomizer;

public class SendDeathLinkMessageHandler : IMessageHandler
{
    private IArchipelagoClient _archipelagoClient;

    public SendDeathLinkMessageHandler(IArchipelagoClient archipelagoClient)
    {
        _archipelagoClient = archipelagoClient;
    }

    public void Handle(IMessage message)
    {
        if (!(message is SendDeathLinkMessage))
        {
            throw new ArgumentException("Message is not of type SendDeathLinkMessage");
        }

        _archipelagoClient.SendDeathLink();
    }
}
