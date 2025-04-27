namespace MinishootRandomizer;

public class XpCrystalsItem : Item
{
    public XpCrystalsItem(string identifier, ItemCategory itemCategory, int xpAmount) : base(identifier, itemCategory)
    {
        XpAmount = xpAmount;
    }

    // In game amounts for a single crystal are : 5, 10, 20 and 50.
    // One "randomizer item" can (and will) be multiple crystals.
    // @see PlayerData::DestroyableCrystalValue[] in the game's code.
    public int XpAmount { get; }

    public override void Collect()
    {
        DropManager.CrystalXp(XpAmount, Player.Instance.transform.position, 4);
    }

    public override string GetSpriteIdentifier()
    {
        return "Xp Crystals";
    }
}
