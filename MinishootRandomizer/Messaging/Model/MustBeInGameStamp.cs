namespace MinishootRandomizer;

public class MustBeInGameStamp : IStamp
{
    public bool CanHandle(IMessage message)
    {
        return GameManager.State == GameState.Game && SGTime.GameFreezeScale > 0f && !PlayerState.Crystalized;
    }
}
