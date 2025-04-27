using System.Collections.Generic;

namespace MinishootRandomizer;

public class InMemoryEnvelopeStorage : IEnvelopeStorage
{
    private readonly List<Envelope> _envelopes = new();

    public void Store(Envelope envelope)
    {
        _envelopes.Add(envelope);
    }

    public List<Envelope> GetAll()
    {
        return _envelopes;
    }

    public void Remove(Envelope envelope)
    {
        _envelopes.Remove(envelope);
    }

    public void Clear()
    {
        _envelopes.Clear();
    }
}
