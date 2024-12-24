using System.Collections.Generic;

namespace MinishootRandomizer;

public class Region
{
    public const string DesertGrottoEastDrop = "Desert Grotto - East Drop";
    public const string DesertGrottoWestDrop = "Desert Grotto - West Drop";
    public const string Dungeon5Boss = "Dungeon 5 - Boss";
    public const string Dungeon5EastWing = "Dungeon 5 - East wing";
    public const string Dungeon5WestWing = "Dungeon 5 - West wing";
    public const string ScarabTempleBottomLeftTorch = "Scarab Temple - Bottom Left Torch";
    public const string ScarabTempleBottomRightTorch = "Scarab Temple - Bottom Right Torch";
    public const string ScarabTempleTopLeftTorch = "Scarab Temple - Top Left Torch";
    public const string ScarabTempleTopRightTorch = "Scarab Temple - Top Right Torch";
    public const string SunkenCityCity = "Sunken City - City";
    public const string SunkenCityEast = "Sunken City - East";
    public const string SunkenCityEastTorch = "Sunken City - East torch";
    public const string SunkenCityFountain = "Sunken City - Fountain";
    public const string SunkenCityWestIsland = "Sunken City - West Island";
    public const string SunkenCityWestTorch = "Sunken City - West torch";

    private List<string> _locationNames = new List<string>();

    public string Name { get; }

    public Region(string name)
    {
        Name = name;
    }

    public void AddLocationName(string locationName)
    {
        _locationNames.Add(locationName);
    }

    public void RemoveLocationName(string locationName)
    {
        _locationNames.Remove(locationName);
    }

    public IReadOnlyList<string> GetLocationNames()
    {
        return _locationNames.AsReadOnly();
    }
}
