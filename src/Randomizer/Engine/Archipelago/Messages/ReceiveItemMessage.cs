namespace MinishootRandomizer;

public class ReceiveItemMessage: IMessage
{
    public Item Item { get; }

    public ReceiveItemMessage(Item item)
    {
        Item = item;
    }
}
