namespace MinishootRandomizer;

public class DialogIsNotOpenStamp : IStamp
{
    public bool CanHandle(IMessage message)
    {
        TextMessage textMessage = UIManager.TextMessage;
        
        return !textMessage.IsOpen;
    }
}
