namespace MinishootRandomizer;

public class SpiritItem : Item
{
    public SpiritItem(string identifier, ItemCategory category) : base(identifier, category)
    {
    }

    public override void Collect()
    {
        throw new System.NotImplementedException();
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
}
