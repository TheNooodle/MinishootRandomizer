using System.Collections.Generic;

namespace MinishootRandomizer
{
    public interface ILocationRepository
    {
        public Location Get(string identifier);
        public List<Location> GetAll();
    }

    public class LocationNotFoundException : System.Exception
    {
        private readonly string _locationName;

        public string LocationName => _locationName;

        public LocationNotFoundException(string locationName) : base($"Location with identifier {locationName} not found")
        {
            _locationName = locationName;
        }
    }
}
