using System;

namespace MinishootRandomizer;

public class MessageProcessor : IMessageProcessor
{
    public MessageProcessingResult ProcessMessage(Envelope envelope, IMessageHandler handler)
    {
        try
        {
            handler.Handle(envelope.Message);
            return new MessageProcessingResult(true);
        }
        catch (Exception e)
        {
            HandleRetry(envelope);
            return new MessageProcessingResult(false, e.Message);
        }
    }

    private void HandleRetry(Envelope envelope)
    {
        RetryStamp retryStamp = envelope.GetStamp<RetryStamp>();
        if (retryStamp != null)
        {
            envelope.RemoveStamp(retryStamp);
        }
        retryStamp = new RetryStamp();
        envelope.AddStamp(retryStamp);
    }
}
