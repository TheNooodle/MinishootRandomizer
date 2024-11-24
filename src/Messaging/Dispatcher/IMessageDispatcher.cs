using System.Collections.Generic;

namespace MinishootRandomizer;

public interface IMessageDispatcher
{
    void Dispatch(IMessage message, List<IStamp> stamps = null);
}
