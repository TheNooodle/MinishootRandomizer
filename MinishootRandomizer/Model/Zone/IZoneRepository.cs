using System.Collections.Generic;

namespace MinishootRandomizer
{
    public interface IZoneRepository
    {
        Zone Get(string identifier);
        List<Zone> GetByGameLocationName(string gameLocationName);
    }

    public class ZoneNotFoundException : System.Exception
    {
        public ZoneNotFoundException(string identifier) : base($"Zone not found: {identifier}") { }
    }
}
