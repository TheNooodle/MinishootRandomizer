using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
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
            IServiceContainer serviceContainer = Plugin.ServiceContainer;
            ILogger logger = serviceContainer.Has<ILogger>() ? serviceContainer.Get<ILogger>() : new NullLogger();

            var codes = new List<CodeInstruction>(instructions);
            bool foundCorrespondingIf = false;
            int rangeStart = -1;
            int rangeEnd = -1;

            for (int i = 0; i < codes.Count; i++)
            {
                logger.LogWarning($"Processing instruction {i}: {codes[i]}");
                if (!foundCorrespondingIf && codes[i].opcode == OpCodes.Brfalse)
                {
                    logger.LogWarning($"Found corresponding if at instruction {i}: {codes[i]}");
                    foundCorrespondingIf = true;
                    rangeStart = i + 1; // Start after the if condition
                }
                if (foundCorrespondingIf && codes[i].opcode == OpCodes.Ldc_I4_1)
                {
                    logger.LogWarning($"Found Ldc_I4_1 at instruction {i}, rangeStart: {rangeStart}");
                    rangeEnd = i + 1; // End at the instruction after Ldc_I4_1
                    break;
                }
            }

            // We can now safely remove the range of instructions
            if (rangeStart != -1 && rangeEnd != -1)
            {
                logger.LogWarning($"Removing instructions from {rangeStart} to {rangeEnd}");
                codes.RemoveRange(rangeStart, rangeEnd - rangeStart + 1);
            }
            else
            {
                logger.LogWarning("No valid range found to remove.");
            }

            return codes.AsEnumerable();
        }
    }

    [HarmonyPatch(typeof(NpcTiny))]
    [HarmonyPatch("EndRaceWon", MethodType.Normal)]
    public static class NpcTiny_EndRaceWon_Patch
    {
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
