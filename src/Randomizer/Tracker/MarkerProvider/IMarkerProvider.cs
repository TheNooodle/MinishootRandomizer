using System.Collections.Generic;
using UnityEngine;

namespace MinishootRandomizer;

public interface IMarkerProvider
{
    List<GameObject> GetMarkerObjects();
}
