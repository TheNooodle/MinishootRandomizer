namespace MinishootRandomizer;

// This class uses PlayerState to maintain a counter that will be persisted across game sessions inside the save file.
// This is a quick and dirty way to have a persisted counter without resorting to more complex solutions like writing custom data inside the save file.
public class PlayerStateItemCounter : IItemCounter
{
    private int _index = 6;

    public PlayerStateItemCounter(int index = 6)
    {
        _index = index;
    }

    public int GetCount()
    {
        return PlayerState.DungeonKeys[_index];
    }

    public void Increment()
    {
        PlayerState.DungeonKeys[_index]++;
    }
}
