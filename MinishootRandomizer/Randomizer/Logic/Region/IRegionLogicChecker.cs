using System.Collections.Generic;

namespace MinishootRandomizer;

/// <summary>
/// Interface for checking the logic accessibility of regions.
/// It does not determine if a region is in logic or out of logic, but rather if it can be reached with the passed logic state.
/// As such, it is used internally by the location logic checker, and should not be used directly by the user.
/// </summary>
public interface IRegionLogicChecker
{
    List<Region> GetReachableRegions(LogicState state);
    bool CanReachRegion(Region region, LogicState state);
}
