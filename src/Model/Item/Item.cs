namespace MinishootRandomizer;

public enum ItemCategory
{
    Filler,
    Helpful,
    Progression,
    Token,
    Trap
}

abstract public class Item
{
    public const string AbyssMap = "Abyss Map";
    public const string Academician = "Academician";
    public const string AdvancedEnergy = "Advanced Energy";
    public const string AncientAstrolabe = "Ancient Astrolabe";
    public const string AncientTablet = "Ancient Tablet";
    public const string Bard = "Bard";
    public const string BeachMap = "Beach Map";
    public const string Blacksmith = "Blacksmith";
    public const string BlueForestMap = "Blue Forest Map";
    public const string Boost = "Boost";
    public const string Compass = "Compass";
    public const string CrystalBullet = "Crystal Bullet";
    public const string DarkHeart = "Dark Heart";
    public const string DarkKey = "Dark Key";
    public const string Dash = "Dash";
    public const string DesertMap = "Desert Map";
    public const string EnchantedHeart = "Enchanted Heart";
    public const string EnchantedPowers = "Enchanted Powers";
    public const string EnergyCrystalShard = "Energy Crystal Shard";
    public const string Explorer = "Explorer";
    public const string FamilyChild = "Family Child";
    public const string FamilyParent1 = "Family Parent 1";
    public const string FamilyParent2 = "Family Parent 2";
    public const string GreenMap = "Green Map";
    public const string Healer = "Healer";
    public const string HpCrystalShard = "HP Crystal Shard";
    public const string IdolOfProtection = "Idol of protection";
    public const string IdolOfSpirits = "Idol of spirits";
    public const string IdolOfTime = "Idol of time";
    public const string JunkyardMap = "Junkyard Map";
    public const string LuckyHeart = "Lucky Heart";
    public const string Mercant = "Mercant";
    public const string Overcharge = "Overcharge";
    public const string PowerOfProtection = "Power of protection";
    public const string PowerOfSpirits = "Power of spirits";
    public const string PowerOfTime = "Power of time";
    public const string PrimordialCrystal = "Primordial Crystal";
    public const string ProgressiveCannon = "Progressive Cannon";
    public const string RestorationEnhancer = "Restoration Enhancer";
    public const string Scarab = "Scarab";
    public const string ScarabCollector = "Scarab Collector";
    public const string ScarabKey = "Scarab Key";
    public const string Spirit = "Spirit";
    public const string SpiritDash = "Spirit Dash";
    public const string SunkenCityMap = "Sunken City Map";
    public const string Supershot = "Supershot";
    public const string Surf = "Surf";
    public const string SwampMap = "Swamp Map";
    public const string VengefulTalisman = "Vengeful Talisman";
    public const string VillageStar = "Village Star";
    public const string WoundedHeart = "Wounded Heart";


    private string _identifier;
    private ItemCategory _category;

    public string Identifier => _identifier;
    public ItemCategory Category => _category;

    public Item(string identifier, ItemCategory category)
    {
        _identifier = identifier;
        _category = category;
    }

    abstract public void Collect();

    public virtual string GetSpriteIdentifier()
    {
        return _identifier;
    }
}
