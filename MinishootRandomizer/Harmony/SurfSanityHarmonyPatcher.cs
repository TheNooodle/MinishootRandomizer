using HarmonyLib;

namespace MinishootRandomizer;

public class SurfSanityHarmonyPatcher
{
    [HarmonyPatch(typeof(PlayerControl))]
    [HarmonyPatch("MiniUpdate", MethodType.Normal)]
    public static class PlayerControl_MiniUpdate_Patch
    {
        public static void Postfix(PlayerControl __instance)
        {
            IRandomizerEngine randomizerEngine = Plugin.ServiceContainer.Get<IRandomizerEngine>();
            if (!randomizerEngine.IsRandomized() || !randomizerEngine.GetSetting<SurfSanity>().Enabled)
            {
                return;
            }

            SurfaceHandler surfaceHandler = __instance.SurfaceHandler;
            surfaceHandler.Water.CanOver = WorldState.Get(
                $"{surfaceHandler.WaterTypeOvered}HoverUnlocked"
            );
        }
    }
}
