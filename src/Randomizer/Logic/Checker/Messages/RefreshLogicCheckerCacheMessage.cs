namespace MinishootRandomizer;

public class RefreshLogicCheckerCacheMessage : IMessage
{
    public MessageQueue MessageQueue => MessageQueue.BackgroundThread;

    private readonly CachedLogicChecker _cachedLogicChecker;

    public CachedLogicChecker CachedLogicChecker => _cachedLogicChecker;

    public RefreshLogicCheckerCacheMessage(CachedLogicChecker cachedLogicChecker)
    {
        _cachedLogicChecker = cachedLogicChecker;
    }
}
