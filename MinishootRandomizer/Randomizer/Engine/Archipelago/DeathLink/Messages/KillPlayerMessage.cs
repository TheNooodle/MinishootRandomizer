namespace MinishootRandomizer;

public class KillPlayerMessage : IMessage
{
    public MessageQueue MessageQueue => MessageQueue.MainThread;

    private readonly string _source;

    public string Source => _source;

    public KillPlayerMessage(string source)
    {
        _source = source;
    }
}
