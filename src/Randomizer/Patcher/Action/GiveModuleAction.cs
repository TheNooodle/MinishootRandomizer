namespace MinishootRandomizer;

public class GiveModuleAction : IPatchAction
{
    private readonly Modules _module;

    public GiveModuleAction(Modules module)
    {
        _module = module;
    }

    public void Dispose()
    {
        // no-op
    }

    public void Patch()
    {
        PlayerState.SetModule(_module, true);
    }

    public void Unpatch()
    {
        PlayerState.SetModule(_module, false);
    }
}
