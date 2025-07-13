namespace MinishootRandomizer;

public class SpiritItem : Item
{
    public SpiritItem(string identifier, ItemCategory category) : base(identifier, category)
    {
    }

    public override void Collect()
    {
        for (int i = 0; i < 8; i++)
        {
            if (!WorldState.Get("NpcTiny" + i))
            {
                WorldState.Set("NpcTiny" + i, true);
            }
        }
    }

    public override int GetOwnedQuantity()
    {
        int count = 0;
        for (int i = 0; i < 8; i++)
        {
            if (WorldState.Get("NpcTiny" + i))
            {
                count++;
            }
        }

        return count;
    }

    public override string GetSpriteIdentifier()
    {
        return "Spirit";
    }
}
