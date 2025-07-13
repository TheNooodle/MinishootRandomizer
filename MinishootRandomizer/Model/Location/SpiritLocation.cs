using System.Collections.Generic;

namespace MinishootRandomizer;

public class SpiritLocation : Location
{
    public static readonly Dictionary<int, string> IndexToNameMap = new Dictionary<int, string>
    {
        { 0, "Green Grotto - Race Reward" },
        { 1, "Scarab Temple - Race Reward" },
        { 2, "Forest Shop Race - Reward" },
        { 3, "Abyss Race - Reward" },
        { 4, "Swamp Race - Reward" },
        { 5, "Desert Race - Reward" },
        { 6, "Sunken City Race - Reward" },
        { 7, "Beach Race - Reward" }
    };

    private readonly int _index;

    public int Index => _index;

    public SpiritLocation(string identifier, string logicRule, LocationPool pool, int index) : base(identifier, logicRule, pool)
    {
        _index = index;
    }

    public override IPatchAction Accept(ILocationVisitor visitor, Item item)
    {
        return visitor.VisitSpirit(this, item);
    }
}
