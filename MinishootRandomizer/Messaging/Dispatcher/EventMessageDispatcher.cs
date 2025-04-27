using System.Collections.Generic;

namespace MinishootRandomizer;

public class EventMessageDispatcher : IMessageDispatcher
{
    private readonly IMessageDispatcher _innerDispatcher;

    public delegate void BeforeMessageDispatchHandler(IMessage message, List<IStamp> stamps);
    public event BeforeMessageDispatchHandler BeforeMessageDispatch;

    public delegate void AfterMessageDispatchHandler(IMessage message, List<IStamp> stamps);
    public event AfterMessageDispatchHandler AfterMessageDispatch;

    public EventMessageDispatcher(IMessageDispatcher innerDispatcher)
    {
        _innerDispatcher = innerDispatcher;
    }

    public void Dispatch(IMessage message, List<IStamp> stamps = null)
    {
        BeforeMessageDispatch?.Invoke(message, stamps);
        _innerDispatcher.Dispatch(message, stamps);
        AfterMessageDispatch?.Invoke(message, stamps);
    }
}
