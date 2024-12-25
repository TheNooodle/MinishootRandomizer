using System.Collections.Generic;

namespace MinishootRandomizer;

public interface ITransitionRepository
{
    Transition Get(string identifier);
    List<Transition> GetFromOriginRegion(Region region);
}
