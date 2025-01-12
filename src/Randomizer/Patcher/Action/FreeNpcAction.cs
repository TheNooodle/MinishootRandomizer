namespace MinishootRandomizer;

public class FreeNpcAction : IPatchAction
{
    private readonly NpcIds _npcId;

    public FreeNpcAction(NpcIds npcId)
    {
        _npcId = npcId;
    }

    public void Dispose()
    {
        // no-op
    }

    public void Patch()
    {
        WorldState.Set(_npcId.Str() + "Introduced", true);
        WorldState.Set(_npcId.Str(), true);
        ReflectionHelper.InvokeStaticAction(typeof(CrystalNpc), "Freed");
    }

    public void Unpatch()
    {
        WorldState.Set(_npcId.Str(), false);
    }
}
