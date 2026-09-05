using System;
using HarmonyLib;
using UnityEngine;

namespace MinishootRandomizer;

public static class SplitSupershotHarmonyPatcher
{
    [HarmonyPatch(typeof(Destroyable))]
    [HarmonyPatch("CheckCollision", MethodType.Normal)]
    public static class Destroyable_CheckCollision_Patch
    {
        public static bool Prefix(Destroyable __instance, GameObject incomingGo, float damage, Vector2 contact, bool triggerFx = true)
        {
            if (!IsSplitSupershotEnabled())
            {
                // Splitshot is not enabled : fall back to the original method.
                return true;
            }

            // Splitshot is enabled : this is rewritten version of the original method, with the addition of a check for the BlastshotUnlocked world state.
            if (__instance.IsDestroyed)
            {
                return false;
            }

            string[] getHitTags = ReflectionHelper.GetPrivateFieldValue<string[]>(__instance, "getHitBy");
            bool haveBlastShot = WorldState.Get("BlastshotUnlocked");
            string incomingTag = incomingGo.tag;
            bool isPrimordial = incomingGo.GetComponent<Bullet>()?.IsPrimordial ?? false;
            if (incomingTag == "PlayerEmissionSuper" && !haveBlastShot && !isPrimordial)
            {
                incomingTag = "PlayerEmission";
            }
            foreach (string damagerTag in getHitTags)
            {
                if (incomingTag == damagerTag && !__instance.Recovering)
                {
                    __instance.GetDamaged(damage, damagerTag, contact, triggerFx);
                }
            }

            return false;
        }
    }

    [HarmonyPatch(typeof(UnlockerTorch))]
    [HarmonyPatch("Lite", MethodType.Normal)]
    public static class UnlockerTorch_Lite_Patch
    {
        public static bool Prefix()
        {
            return !IsSplitSupershotEnabled() || WorldState.Get("FlameshotUnlocked");
        }
    }

    [HarmonyPatch(typeof(global::Transition))]
    [HarmonyPatch("Unlock", MethodType.Normal)]
    public static class Transition_Unlock_Patch
    {
        public static bool Prefix(global::Transition __instance)
        {
            if (!IsSplitSupershotEnabled() || !__instance.ByShooting)
            {
                return true;
            }

            return WorldState.Get("BlastshotUnlocked");
        }
    }

    private static bool IsSplitSupershotEnabled()
    {
        IServiceContainer serviceContainer = Plugin.ServiceContainer;
        if (serviceContainer == null || !serviceContainer.Has<IRandomizerEngine>())
        {
            return false;
        }

        IRandomizerEngine randomizerEngine = serviceContainer.Get<IRandomizerEngine>();
        return randomizerEngine.IsRandomized() && randomizerEngine.GetSetting<SplitSupershot>().Enabled;
    }
}
