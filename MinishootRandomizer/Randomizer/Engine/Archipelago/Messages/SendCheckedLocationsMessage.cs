using System.Collections.Generic;

namespace MinishootRandomizer;

public class SendCheckedLocationsMessage : IMessage
{
    private List<Location> _checkedLocation;

    public List<Location> CheckedLocation => _checkedLocation;

    public MessageQueue MessageQueue => MessageQueue.MainThread;

    public SendCheckedLocationsMessage(List<Location> checkedLocation)
    {
        _checkedLocation = checkedLocation;
    }
}
