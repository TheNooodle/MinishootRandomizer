namespace MinishootRandomizer;

public class RefreshLogicCheckerCacheMessage : IMessage
{
    public MessageQueue MessageQueue => MessageQueue.BackgroundThread;

    private readonly CachedLocationLogicChecker _cachedLogicChecker;
    private readonly LogicState _logicState;

    public CachedLocationLogicChecker CachedLogicChecker => _cachedLogicChecker;
    public LogicState LogicState => _logicState;

    public RefreshLogicCheckerCacheMessage(CachedLocationLogicChecker cachedLogicChecker, LogicState logicState = null)
    {
        _cachedLogicChecker = cachedLogicChecker;
        _logicState = logicState;
    }
}
