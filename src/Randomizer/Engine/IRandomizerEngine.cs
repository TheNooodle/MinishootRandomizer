using System.Collections.Generic;

namespace MinishootRandomizer
{
    public interface IRandomizerEngine
    {
        public T GetSetting<T>() where T : ISetting;
        public List<Location> GetRandomizedLocations();
        public Item PeekLocation(Location location);
        public Item CheckLocation(Location location);
        public bool IsLocationChecked(Location location);
        public void CompleteGoal(Goals goal);
        public bool IsRandomized();
        public void SetContext(RandomizerContext context);
        public void Initialize();
        public void Dispose();
    }

    public class SettingNotSupported : System.Exception
    {
        public SettingNotSupported(string message) : base(message) { }
    }
}
