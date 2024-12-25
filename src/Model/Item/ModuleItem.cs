namespace MinishootRandomizer;

public class ModuleItem : Item
{
    public ModuleItem(string identifier, ItemCategory itemCategory, Modules module) : base(identifier, itemCategory)
    {
        Module = module;
    }

    public Modules Module { get; }

    public override void Collect()
    {
        PlayerState.SetModule(Module, true);
    }

    public override int GetOwnedQuantity()
    {
        return PlayerState.Modules.TryGetValue(Module, out bool owned) && owned ? 1 : 0;
    }
}
