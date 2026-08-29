using HarmonyLib;

namespace MinishootRandomizer;

public class PrimordialModePatcher
{
    [HarmonyPatch(typeof(PlayerHelper))]
    [HarmonyPatch("PrimordialMode", MethodType.Getter)]
    public static class PlayerHelper_PrimordialMode_Patch
    {
        public static bool Prefix(ref bool __result)
        {
            IRandomizerEngine randomizerEngine = Plugin.ServiceContainer.Get<IRandomizerEngine>();
            if (!randomizerEngine.IsRandomized())
            {
                // Skip the patch
                return true;
            }

            PrimordialCrystalActivationThreshold threshold = randomizerEngine.GetSetting<PrimordialCrystalActivationThreshold>();
            bool patchedResult = false;
            if (PlayerState.Modules[Modules.PrimordialCrystal])
            {
                patchedResult = Player.Instance.Destroyable.HpRatio >= threshold.Value / 100f;
            }
            __result = patchedResult;

            return false;
        }
    }
}
