using System;
using System.Collections.Generic;
using System.Reflection;

namespace MinishootRandomizer;

public class EngineAwareSettingsProvider : ISettingsProvider
{
    private readonly IRandomizerEngine _randomizerEngine;

    public EngineAwareSettingsProvider(IRandomizerEngine randomizerEngine)
    {
        _randomizerEngine = randomizerEngine;
    }

    public List<ISetting> GetSettings()
    {
        List<Type> settingTypes = new List<Type> {
            typeof(NpcSanity),
            typeof(ScarabSanity),
            typeof(ShardSanity),
            typeof(KeySanity),
            typeof(BossKeySanity),
            typeof(SimpleTempleExit),
            typeof(BlockedForest),
            typeof(CannonLevelLogicalRequirements),
            typeof(BoostlessSpringboards),
            typeof(BoostlessSpiritRaces),
            typeof(BoostlessTorchRaces),
            typeof(CompletionGoals),
        };

        List<ISetting> settings = new List<ISetting>();
        foreach (Type settingType in settingTypes)
        {
            // Is there a better way to do this instead of reflection ?
            MethodInfo method = typeof(IRandomizerEngine).GetMethod("GetSetting").MakeGenericMethod(settingType);
            ISetting setting = (ISetting)method.Invoke(_randomizerEngine, null);
            settings.Add(setting);
        }

        return settings;
    }
}
