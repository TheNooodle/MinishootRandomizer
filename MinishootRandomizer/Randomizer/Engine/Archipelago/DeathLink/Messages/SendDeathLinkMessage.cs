namespace MinishootRandomizer;

public class SendDeathLinkMessage : IMessage
{
    public MessageQueue MessageQueue => MessageQueue.BackgroundThread;
}
