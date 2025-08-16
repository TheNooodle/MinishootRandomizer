using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;

namespace MinishootRandomizer;

/// <summary>
/// Represents a list of <see cref="CodeInstruction"/> objects with helper methods for manipulation.
/// </summary>
/// <remarks>
/// This class is used in Harmony patches to make it easier to manipulate IL code instructions.
/// </remarks>
public class CodeInstructionList
{
    private readonly List<CodeInstruction> _instructions;

    public CodeInstructionList(IEnumerable<CodeInstruction> instructions)
    {
        _instructions = new List<CodeInstruction>(instructions);
    }

    /// <summary>
    /// Removes calls to a specific method from the instruction list.
    /// </summary>
    /// <param name="method">The method to search for calls to remove.</param>
    /// <param name="paddingBefore">Number of instructions to remove before the method call. Default is 0.</param>
    /// <param name="paddingAfter">Number of instructions to remove after the method call. Default is 0.</param>
    /// <param name="count">Maximum number of method call occurrences to remove. Default is 1.</param>
    /// <param name="offset">Number of method call occurrences to skip before removing. Default is 0.</param>
    /// <exception cref="InvalidOperationException">Thrown when no calls to the specified method are found or when the range to remove is invalid.</exception>
    public void RemoveMethodCall(MethodInfo method, int paddingBefore = 0, int paddingAfter = 0, int count = 1, int offset = 0)
    {
        int callsFound = 0;
        List<Tuple<int, int>> rangesToRemove = new List<Tuple<int, int>>();
        for (int i = 0; i < _instructions.Count; i++)
        {
            CodeInstruction instruction = _instructions[i];
            if (instruction.Calls(method))
            {
                callsFound++;
                if (callsFound > offset && rangesToRemove.Count < count)
                {
                    rangesToRemove.Add(new Tuple<int, int>(i - paddingBefore, i + paddingAfter));
                }
            }
        }

        if (rangesToRemove.Count == 0)
        {
            throw new InvalidOperationException($"No calls to method {method.Name} found in the instruction list.");
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

    /// <summary>
    /// Removes loads of a specific field from the instruction list.
    /// </summary>
    /// <param name="field">The field to search for loads to remove.</param>
    /// <param name="paddingBefore">Number of instructions to remove before the field load. Default is 0.</param>
    /// <param name="paddingAfter">Number of instructions to remove after the field load. Default is 0.</param>
    /// <param name="count">Maximum number of field load occurrences to remove. Default is 1.</param>
    /// <param name="offset">Number of field load occurrences to skip before removing. Default is 0.</param>
    /// <exception cref="InvalidOperationException">Thrown when no loads of the specified field are found or when the range to
    public void RemoveFieldLoading(FieldInfo field, int paddingBefore = 0, int paddingAfter = 0, int count = 1, int offset = 0)
    {
        int loadsFound = 0;
        List<Tuple<int, int>> rangesToRemove = new List<Tuple<int, int>>();
        for (int i = 0; i < _instructions.Count; i++)
        {
            CodeInstruction instruction = _instructions[i];
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

    /// <summary>
    /// Returns the list of code instructions as an enumerable collection.
    /// </summary>
    /// <returns>An enumerable collection of <see cref="CodeInstruction"/> objects.</returns>
    public IEnumerable<CodeInstruction> GetInstructions()
    {
        return _instructions.AsEnumerable();
    }
}
