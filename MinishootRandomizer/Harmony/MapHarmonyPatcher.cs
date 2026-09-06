using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using UnityEngine;

namespace MinishootRandomizer;

public class MapHarmonyPatcher
{
    [HarmonyPatch(typeof(Map))]
    [HarmonyPatch("Open", MethodType.Normal)]
    public static class Map_Open_Patch
    {
        public static bool Prefix(Map __instance)
        {
            RandomizerMapComponent randomizerMapComponent = __instance.gameObject.GetComponent<RandomizerMapComponent>();
            if (randomizerMapComponent != null)
            {
                randomizerMapComponent.OnMapOpened();
            }

            return true; // Continue with original method execution
        }

        public static void Postfix(Map __instance)
        {
            RandomizerMapComponent randomizerMapComponent = __instance.gameObject.GetComponent<RandomizerMapComponent>();
            if (randomizerMapComponent != null)
            {
                randomizerMapComponent.OnAfterMapOpened();
            }
        }

        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            CodeInstructionList codeInstructionList = new CodeInstructionList(instructions);
            List<int> effectiveIndices = codeInstructionList.RemoveMethodCall(
                typeof(LocationManager).GetProperty(
                    "Current", 
                    BindingFlags.Public | BindingFlags.Static
                ).GetGetMethod(), 
                paddingBefore: 0,
                paddingAfter: 10
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
                                      new[] { typeof(CurrentMapHandler) })),
                new CodeInstruction(OpCodes.Callvirt,
                    AccessTools.Method(typeof(CurrentMapHandler), "IsCurrentMapAvailable"))
            });

            return codeInstructionList.GetInstructions();
        }
    }

    [HarmonyPatch(typeof(Map))]
    [HarmonyPatch("GetPosAtScale", MethodType.Normal)]
    public static class Map_GetPosAtScale_Patch
    {
        public static bool Prefix(ref Vector3 realPos, ref Vector2 __result)
        {
            IRandomizerEngine randomizerEngine = Plugin.ServiceContainer.Get<IRandomizerEngine>();
            TrackerMap currentMap = RandomizerMapComponent.CurrentMap;
            if (!randomizerEngine.IsRandomized() || currentMap == null)
            {
                // Vanilla behavior
                return true;
            }

            Vector3 center = new Vector3(currentMap.SpriteData.Center.X, currentMap.SpriteData.Center.Y, currentMap.SpriteData.Center.Z);
            float scale = RandomizerMapComponent.DebugScale != 0.0f ? RandomizerMapComponent.DebugScale : currentMap.SpriteData.Scale;
            __result = (realPos - center) / scale;

            return false;
        }
    }
}
