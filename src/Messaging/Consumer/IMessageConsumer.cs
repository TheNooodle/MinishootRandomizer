namespace MinishootRandomizer;

public interface IMessageConsumer
{
    void Consume();
    void AddHandler<T>(IMessageHandler handler);
}
