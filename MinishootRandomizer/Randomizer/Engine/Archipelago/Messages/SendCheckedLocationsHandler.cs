using System;
using System.Collections.Generic;

namespace MinishootRandomizer;

public class SendCheckedLocationsHandler : IMessageHandler
{
    private readonly IArchipelagoClient _client;

    public SendCheckedLocationsHandler(IArchipelagoClient client)
    {
        _client = client;
    }

    public void Handle(IMessage message)
    {
        if (!(message is SendCheckedLocationsMessage))
        {
            throw new ArgumentException("Message is not of type SendCheckedLocationsMessage");
        }
        List<string> checkedLocationNames = new();
        foreach (Location location in ((SendCheckedLocationsMessage)message).CheckedLocation)
        {
            checkedLocationNames.Add(location.Identifier);
        }

        _client.CheckLocations(checkedLocationNames);
    }
}
