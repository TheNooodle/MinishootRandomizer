using System.Collections.Generic;

namespace MinishootRandomizer;

public interface ISettingsProvider
{
    List<ISetting> GetSettings();
}
