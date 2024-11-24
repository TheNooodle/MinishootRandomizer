namespace MinishootRandomizer
{
    public interface IRegionRepository
    {
        Region Get(string identifier);
    }

    public class RegionNotFoundException : System.Exception
    {
        public RegionNotFoundException(string identifier) : base($"Region not found: {identifier}") { }
    }
}
