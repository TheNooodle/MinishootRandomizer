using System;
using System.Collections.Generic;
using System.Reflection;
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
            // @todo : remove the field loads and replace them with a call to Plugin.ServiceContainer.Get<TowerHandler>().GetSpiritCount()
            List<CodeInstruction> instructionsList = new List<CodeInstruction>(instructions);
            FieldInfo field = typeof(SpiritTowerUnlocker).GetField("spiritSlots");
            int loadsFound = 0;
            List<Tuple<int, int>> rangesToRemove = new List<Tuple<int, int>>();
            for (int i = 0; i < instructionsList.Count; i++)
            {
                CodeInstruction instruction = instructionsList[i];
                if (instruction.LoadsField(field))
                {
                    loadsFound++;
                    if (loadsFound > offset && rangesToRemove.Count < count)
                    {
                        rangesToRemove.Add(new Tuple<int, int>(i - paddingBefore, i + paddingAfter));
                    }
                }
            }

            if (rangesToRemove.Count == 0)
            {
                throw new InvalidOperationException($"No loads of field {field.Name} found in the instruction list.");
            }

            foreach (Tuple<int, int> range in rangesToRemove)
            {
                int start = range.Item1;
                int end = range.Item2;
                if (start < 0 || end >= _instructions.Count || start > end)
                {
                    throw new InvalidOperationException($"Invalid range to remove: {start} to {end}.");
                }
                _instructions.RemoveRange(start, end - start + 1);
            }
        }
    }
}
