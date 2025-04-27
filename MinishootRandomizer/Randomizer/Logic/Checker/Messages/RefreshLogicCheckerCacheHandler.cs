using System;

namespace MinishootRandomizer;

public class RefreshLogicCheckerCacheHandler : IMessageHandler
{
    public void Handle(IMessage message)
    {
        if (!(message is RefreshLogicCheckerCacheMessage))
        {
            throw new ArgumentException("Message is not of type RefreshLogicCheckerCacheMessage");
        }

        ((RefreshLogicCheckerCacheMessage)message).CachedLogicChecker.RefreshCache();
    }
}
