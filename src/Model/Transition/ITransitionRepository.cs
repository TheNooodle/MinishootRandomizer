using System.Collections.Generic;

namespace MinishootRandomizer;

public interface ITransitionRepository
{
    Transition GetTransition(string name);
    List<Transition> GetFromOriginRegion(Region region);
}
