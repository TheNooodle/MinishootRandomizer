namespace MinishootRandomizer
{
    public interface IZoneFactory
    {
        Zone Create(string name, string regionsString);
    }
}
