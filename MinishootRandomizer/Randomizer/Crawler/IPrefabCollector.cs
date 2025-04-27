using System.Collections.Generic;

namespace MinishootRandomizer;

public interface IPrefabCollector
{
    void AddPrefab(string prefabIdentifier, ISelector selector, CloningType cloningType);
}

public enum CloningType
{
    Copy,
    Recreate
}
