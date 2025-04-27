namespace MinishootRandomizer;

public class ReceiveItemMessage: IMessage
{
    public Item Item { get; }

    public MessageQueue MessageQueue => MessageQueue.MainThread;

    public ReceiveItemMessage(Item item)
    {
        Item = item;
    }
}
