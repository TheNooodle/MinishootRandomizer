using System.Collections.Generic;
using HarmonyLib;

namespace MinishootRandomizer;

public class SpiritHarmonyPatcher
{
    [HarmonyPatch(typeof(NpcTiny))]
    [HarmonyPatch("OnTriggerEnter2D", MethodType.Normal)]
    public static class NpcTiny_OnTriggerEnter2D_Patch
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            CodeInstructionList codeInstructionList = new CodeInstructionList(instructions);
            codeInstructionList.RemoveMethodCall(typeof(WorldState).GetMethod("Set"), 3);

            return codeInstructionList.GetInstructions();
        }
    }

    [HarmonyPatch(typeof(NpcTiny))]
    [HarmonyPatch("OnGameStateLoaded", MethodType.Normal)]
    public static class NpcTiny_OnGameStateLoaded_Patch
    {
        public static bool Prefix(NpcTiny __instance)
        {
            string name = __instance.gameObject.name;
            if (name.Length <= 0 || !char.IsDigit(name[name.Length - 1]))
            {
                ILogger logger = Plugin.ServiceContainer.Get<ILogger>();
                logger.LogWarning($"NpcTiny.OnGameStateLoaded called on an NPC with an invalid name: {name}. Expected a name ending with a digit.");
                return true;
            }

            // We replace the original logic with our own to handle a separate flag.
            int index = name[name.Length - 1] - '0';
            __instance.gameObject.SetActive(!WorldState.Get("WonNpcTiny" + index));
            ReflectionHelper.InvokePrivateMethod(__instance, "Restore");

            return false; // Skip original method execution
        }
    }

    [HarmonyPatch(typeof(NpcTiny))]
    [HarmonyPatch("EndRaceWon", MethodType.Normal)]
    public static class NpcTiny_EndRaceWon_Patch
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            CodeInstructionList codeInstructionList = new CodeInstructionList(instructions);
            codeInstructionList.RemoveMethodCall(typeof(TextMessage).GetMethod("Pop"), 7);

            return codeInstructionList.GetInstructions();
        }

        public static void Postfix(NpcTiny __instance)
        {
            IServiceContainer serviceContainer = Plugin.ServiceContainer;
            if (!serviceContainer.Has<GameEventDispatcher>())
            {
                return;
            }
            GameEventDispatcher eventDispatcher = serviceContainer.Get<GameEventDispatcher>();

            string name = __instance.gameObject.name;
            if (name.Length > 0 && char.IsDigit(name[name.Length - 1]))
            {
                int index = name[name.Length - 1] - '0';

                // In all cases, we setup an additional flag to persist race completion.
                WorldState.Set("WonNpcTiny" + index, true);

                eventDispatcher.DispatchRaceWon(index);
            }
            else
            {
                ILogger logger = serviceContainer.Has<ILogger>() ? serviceContainer.Get<ILogger>() : new NullLogger();
                logger.LogWarning($"NpcTiny.EndRaceWon called on an NPC with an invalid name: {name}. Expected a name ending with a digit.");
            }
        }
    }
}
