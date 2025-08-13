using HarmonyLib;

namespace MinishootRandomizer;

public class DestroyableHarmonyPatcher
{
    [HarmonyPatch(typeof(Destroyable))]
    [HarmonyPatch("GetDestroyed", MethodType.Normal)]
    public static class Destroyable_GetDestroyed_Patch
    {
        public static void Prefix(Destroyable __instance, ref string damagerTag)
        {
            Player player = __instance.GetComponent<Player>();
            if (player == null)
            {
                return;
            }

            IServiceContainer serviceContainer = Plugin.ServiceContainer;
            if (!serviceContainer.Has<GameEventDispatcher>())
            {
                return;
            }
            GameEventDispatcher eventDispatcher = serviceContainer.Get<GameEventDispatcher>();

            eventDispatcher.DispatchOnPlayerDeath(damagerTag);
        }
    }
}
