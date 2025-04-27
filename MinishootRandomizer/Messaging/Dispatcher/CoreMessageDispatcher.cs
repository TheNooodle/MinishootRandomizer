using System.Collections.Generic;

namespace MinishootRandomizer;

public class CoreMessageDispatcher : IMessageDispatcher
{
    private readonly IEnvelopeStorage _storage;

    public CoreMessageDispatcher(IEnvelopeStorage storage)
    {
        _storage = storage;
    }

    public void Dispatch(IMessage message, List<IStamp> stamps = null)
    {
        Envelope envelope = new Envelope(message);
        if (stamps != null)
        {
            foreach (IStamp stamp in stamps)
            {
                envelope.AddStamp(stamp);
            }
        }
        _storage.Store(envelope);
    }
}
