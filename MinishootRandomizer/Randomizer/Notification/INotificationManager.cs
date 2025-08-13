namespace MinishootRandomizer;

public interface INotificationManager
{
    void OnItemCollected(Item item);
    void OnDeathLinkReceived(string source);
}
