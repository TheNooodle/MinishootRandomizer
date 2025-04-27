namespace MinishootRandomizer;

public interface IMessageProcessor
{
    MessageProcessingResult ProcessMessage(Envelope envelope, IMessageHandler handler);
}
