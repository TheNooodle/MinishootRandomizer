using System.Collections.Generic;

namespace MinishootRandomizer;

public class ChainPrefabCollector : IPrefabCollector
{
    private readonly List<IPrefabCollector> _collectors = new List<IPrefabCollector>();

    public void AddCollector(IPrefabCollector collector)
    {
        _collectors.Add(collector);
    }

    public void AddPrefab(string prefabIdentifier, ISelector selector, CloningType cloningType)
    {
        foreach (IPrefabCollector collector in _collectors)
        {
            collector.AddPrefab(prefabIdentifier, selector, cloningType);
        }
    }
}
