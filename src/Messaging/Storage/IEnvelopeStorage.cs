using System.Collections.Generic;

namespace MinishootRandomizer;

public interface IEnvelopeStorage
{
    void Store(Envelope envelope);
    List<Envelope> GetAll();
    void Remove(Envelope envelope);
}
