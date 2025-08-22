using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;

namespace MinishootRandomizer;

public class SpiritTowerHarmonyPatcher
{
    [HarmonyPatch(typeof(SpiritTowerUnlocker))]
    [HarmonyPatch("OnTriggerEnter2D", MethodType.Normal)]
    public static class SpiritTowerUnlocker_OnTriggerEnter2D_Patch
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            CodeInstructionList codeInstructionList = new CodeInstructionList(instructions);
            // Remove the field loading for spiritSlots inside the for loop, and replace it with a call to TowerHandler.GetSpiritCount().
            List<int> effectiveIndices = codeInstructionList.RemoveFieldLoading(
                typeof(SpiritTowerUnlocker).GetField(
                    "spiritSlots", 
                    BindingFlags.NonPublic | BindingFlags.Instance
                ), 
                paddingBefore: 1,
                paddingAfter: 2,
                offset: 2
            );
            codeInstructionList.InsertInstructions(effectiveIndices[0], new List<CodeInstruction>
            {
                new CodeInstruction(OpCodes.Call,
                    AccessTools.Method(typeof(Plugin),
                                      "get_ServiceContainer")),
                new CodeInstruction(OpCodes.Callvirt,
                    AccessTools.Method(typeof(IServiceContainer),
                                      "Get",
                                      new Type[0],
                                      new[] { typeof(TowerHandler) })),
                new CodeInstruction(OpCodes.Callvirt,
                    AccessTools.Method(typeof(TowerHandler), "GetSpiritCount"))
            });

            return codeInstructionList.GetInstructions();
        }
    }

    [HarmonyPatch(typeof(SpiritTowerUnlocker))]
    [HarmonyPatch("SetActiveAll", MethodType.Normal)]
    public static class SpiritTowerUnlocker_SetActiveAll_Patch
    {
        public static void Postfix(SpiritTowerUnlocker __instance)
        {
            IServiceContainer serviceContainer = Plugin.ServiceContainer;
            if (!serviceContainer.Has<TowerHandler>())
            {
                return;
            }

            // We want to disable the spirit slots that are not used, based on the desired spirit count.
            TowerHandler towerHandler = serviceContainer.Get<TowerHandler>();
            int spiritCount = towerHandler.GetSpiritCount();
            SpiritLockSlot[] spiritSlots = ReflectionHelper.GetPrivateFieldValue<SpiritLockSlot[]>(__instance, "spiritSlots");
            for (int i = spiritCount; i < spiritSlots.Length; i++)
            {
                spiritSlots[i].gameObject.SetActive(false);
            }
        }
    }

    [HarmonyPatch(typeof(SpiritTowerUnlocker))]
    [HarmonyPatch("CheckUnlock", MethodType.Normal)]
    public static class SpiritTowerUnlocker_CheckUnlock_Patch
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            CodeInstructionList codeInstructionList = new CodeInstructionList(instructions);
            // Remove the hardcoded "8" value, and replace it with a call to TowerHandler.GetSpiritCount().
            // Only remove the first occurrence, as the second one is used for emotes.
            List<int> effectiveIndices = codeInstructionList.RemoveOpCode(OpCodes.Ldc_I4_8);
            codeInstructionList.InsertInstructions(effectiveIndices[0], new List<CodeInstruction>
            {
                new CodeInstruction(OpCodes.Call,
                    AccessTools.Method(typeof(Plugin),
                                      "get_ServiceContainer")),
                new CodeInstruction(OpCodes.Callvirt,
                    AccessTools.Method(typeof(IServiceContainer),
                                      "Get",
                                      new Type[0],
                                      new[] { typeof(TowerHandler) })),
                new CodeInstruction(OpCodes.Callvirt,
                    AccessTools.Method(typeof(TowerHandler), "GetSpiritCount"))
            });

            return codeInstructionList.GetInstructions();
        }
    }
}
