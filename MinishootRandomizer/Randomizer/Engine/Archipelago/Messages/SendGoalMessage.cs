namespace MinishootRandomizer;

public class SendGoalMessage : IMessage
{
    private Goals _goal;
    private bool _sendCompletion = false;

    public Goals Goal => _goal;
    public bool SendCompletion => _sendCompletion;

    public MessageQueue MessageQueue => MessageQueue.MainThread;

    public SendGoalMessage(Goals goal, bool sendCompletion = false)
    {
        _goal = goal;
        _sendCompletion = sendCompletion;
    }
}
