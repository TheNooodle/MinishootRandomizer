namespace MinishootRandomizer;

// Don't try this at home, kids. This is a bad idea.
public class PlayerStateItemCounter : IItemCounter
{
    public int GetCount()
    {
        return PlayerState.DungeonKeys[6];
    }

    public void Increment()
    {
        PlayerState.DungeonKeys[6]++;
    }
}
