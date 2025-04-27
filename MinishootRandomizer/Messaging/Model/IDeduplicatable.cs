using System.Collections.Generic;

namespace MinishootRandomizer;

public interface IDeduplicatable
{
    Envelope Deduplicate(List<Envelope> envelopes);
}
