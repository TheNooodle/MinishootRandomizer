using System.Collections.Generic;

namespace MinishootRandomizer
{
    public class Region
    {
        private List<string> _locationNames = new List<string>();

        public string Name { get; }

        public Region(string name)
        {
            Name = name;
        }

        public void AddLocationName(string locationName)
        {
            _locationNames.Add(locationName);
        }

        public void RemoveLocationName(string locationName)
        {
            _locationNames.Remove(locationName);
        }

        public IReadOnlyList<string> GetLocationNames()
        {
            return _locationNames.AsReadOnly();
        }
    }
}
