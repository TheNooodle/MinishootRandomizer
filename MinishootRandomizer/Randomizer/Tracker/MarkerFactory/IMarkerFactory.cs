using System.Collections.Generic;
using UnityEngine;

namespace MinishootRandomizer;

public interface IMarkerFactory
{
    List<GameObject> CreateMarkerObjects();
}
