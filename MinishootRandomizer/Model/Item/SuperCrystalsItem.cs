namespace MinishootRandomizer;

public class SuperCrystalsItem : Item
{
    public int CrystalsAmount { get; }

    public SuperCrystalsItem(string identifier, ItemCategory itemCategory, int crystalsAmount) : base(identifier, itemCategory)
    {
        CrystalsAmount = crystalsAmount;
    }

    public override void Collect()
    {
        DropManager.Pop(Drop.Type.CrystalSuper, Player.Instance.transform.position, CrystalsAmount);
    }

    public override string GetSpriteIdentifier()
    {
        return "SuperCrystal";
    }
}
