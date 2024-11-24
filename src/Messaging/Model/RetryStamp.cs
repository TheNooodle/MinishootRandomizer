using System;

namespace MinishootRandomizer;

public class RetryStamp : IStamp
{
    public static readonly TimeSpan DefaultRetryInterval = TimeSpan.FromSeconds(5);

    private DateTime _nextRetryTime;

    public DateTime NextRetryTime => _nextRetryTime;

    public RetryStamp()
    {
        _nextRetryTime = DateTime.UtcNow + DefaultRetryInterval;
    }

    public bool CanHandle(IMessage message)
    {
        return DateTime.UtcNow >= _nextRetryTime;
    }
}
