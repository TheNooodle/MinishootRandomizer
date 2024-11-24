using System.Collections.Generic;

namespace MinishootRandomizer
{
    public class Zone
    {
        private List<string> _regionNames = new List<string>();

        public string Name { get; }
        public string GameLocationName { get; set; }

        public Zone(string name)
        {
            Name = name;
        }

        public void AddRegionName(string regionName)
        {
            _regionNames.Add(regionName);
        }

        public void RemoveRegionName(string regionName)
        {
            _regionNames.Remove(regionName);
        }

        public IReadOnlyList<string> GetRegionNames()
        {
            return _regionNames.AsReadOnly();
        }
    }
}
