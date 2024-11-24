using System.Collections.Generic;

namespace MinishootRandomizer;

public class DeduplicatingMessageDispatcher: IMessageDispatcher
{
    private readonly IMessageDispatcher _innerDispatcher;
    private readonly IEnvelopeStorage _storage;
    
    public DeduplicatingMessageDispatcher(IMessageDispatcher innerDispatcher, IEnvelopeStorage storage)
    {
        _innerDispatcher = innerDispatcher;
        _storage = storage;
    }

    public void Dispatch(IMessage message, List<IStamp> stamps = null)
    {
        if (message is IDeduplicatable deduplicatableMessage)
        {
            List<Envelope> allEnvelopes = _storage.GetAll();
            List<Envelope> matchingEnvelopes = allEnvelopes.FindAll(envelope => envelope.Message.GetType() == message.GetType());
            Envelope newEnvelope = deduplicatableMessage.Deduplicate(matchingEnvelopes);

            foreach (Envelope oldEnvelope in matchingEnvelopes)
            {
                _storage.Remove(oldEnvelope);
            }
            _innerDispatcher.Dispatch(newEnvelope.Message, newEnvelope.Stamps);
        }
        else
        {
            _innerDispatcher.Dispatch(message, stamps);
        }
    }
}
