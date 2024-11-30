using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MinishootRandomizer;

public class SceneCrawlerComponent : MonoBehaviour
{
    private IPrefabCollector _collector = null;
    private ILogger _logger = new NullLogger();

    private readonly List<CrawlableScene> _crawlableScenes = new List<CrawlableScene>()
    {
        new CrawlableScene("Cave", new List<CloningData>()
        {
            new CloningData(Item.AbyssMap, new ByName("MapPickupAbyss", typeof(MapPickup))),
            new CloningData(Item.AdvancedEnergy, new ByName("PickupModuleBoostCost", typeof(ModulePickup))),
            new CloningData(Item.AncientTablet, new ByName("LorePickup1", typeof(LorePickup))),
            new CloningData(Item.BlueForestMap, new ByName("MapPickupForest", typeof(MapPickup))),
            new CloningData(Item.CrystalBullet, new ByName("PickupModuleBlueBullet", typeof(ModulePickup))),
            new CloningData(Item.DarkHeart, new ByName("PickupKeyFinalBoss", typeof(KeyUnique))),
            new CloningData(Item.ScarabKey, new ByName("PickupKeyScarab", typeof(KeyUnique))),
            new CloningData(Item.EnchantedHeart, new ByName("PickupModuleHeartCrystal", typeof(ModulePickup))),
            new CloningData(Item.JunkyardMap, new ByName("MapPickupJunkyard", typeof(MapPickup))),
            new CloningData(Item.LuckyHeart, new ByName("PickupModuleHpDrop", typeof(ModulePickup))),
            new CloningData(Item.PrimordialCrystal, new ByName("PickupModulePrimordialCrystal", typeof(ModulePickup))),
            new CloningData(Item.ProgressiveCannon, new ByName("CaveBulletNumber0", typeof(StatsPickup))),
            new CloningData(Item.RestorationEnhancer, new ByName("PickupModuleXpGain", typeof(ModulePickup))),
            new CloningData(Item.SwampMap, new ByName("MapPickupSwamp", typeof(MapPickup))),
            new CloningData(Item.WoundedHeart, new ByName("PickupModuleRage", typeof(ModulePickup))),
            new CloningData("Xp Crystals", new ByName("CaveDestroyable24", typeof(Destroyable)), CloningType.Copy),
            new CloningData("Academician NPC", new ByName("Academician", typeof(Npc)), CloningType.Copy),
            new CloningData("Family Parent 1 NPC", new ByName("Familly1", typeof(Npc)), CloningType.Copy),
            new CloningData("Family Parent 2 NPC", new ByName("Familly2", typeof(Npc)), CloningType.Copy),
            new CloningData("Family Child NPC", new ByName("Familly3", typeof(Npc)), CloningType.Copy),
        }),
        new CrawlableScene("Overworld", new List<CloningData>()
        {
            new CloningData(Item.AncientAstrolabe, new ByName("PickupModuleCollectableScan", typeof(ModulePickup))),
            new CloningData(Item.BeachMap, new ByName("MapPickupBeach", typeof(MapPickup))),
            new CloningData(Item.Boost, new ByName("SkillBoost", typeof(SkillPickup))),
            new CloningData(Item.Compass, new ByName("PickupModuleCompass", typeof(ModulePickup))),
            new CloningData(Item.DesertMap, new ByName("MapPickupDesert", typeof(MapPickup))),
            new CloningData(Item.EnergyCrystalShard, new ByName("OverworldEnergy0", typeof(StatsPickup))),
            new CloningData(Item.GreenMap, new ByName("MapPickupGreen", typeof(MapPickup))),
            new CloningData(Item.HpCrystalShard, new ByName("OverworldHp0", typeof(StatsPickup))),
            new CloningData(Item.Overcharge, new ByName("PickupModuleOvercharge", typeof(ModulePickup))),
            new CloningData(Item.SunkenCityMap, new ByName("MapPickupSunkenCity", typeof(MapPickup))),
            new CloningData(Item.VengefulTalisman, new ByName("PickupModuleRetaliation", typeof(ModulePickup))),
            new CloningData(Item.VillageStar, new ByName("PickupModuleTeleport", typeof(ModulePickup))),
            new CloningData(Item.IdolOfProtection, new ByName("PickupModuleIdolBomb", typeof(ModulePickup))),
            new CloningData(Item.IdolOfTime, new ByName("PickupModuleIdolSlow", typeof(ModulePickup))),
            new CloningData(Item.IdolOfSpirits, new ByName("PickupModuleIdolAlly", typeof(ModulePickup))),
            new CloningData("Rock Destroyable", new ByName("OverworldDestroyable351", typeof(RockDestroyable)), CloningType.Copy),
            new CloningData("Bard NPC", new ByName("Bard", typeof(Npc)), CloningType.Copy),
            new CloningData("Blacksmith NPC", new ByName("Blacksmith", typeof(Npc)), CloningType.Copy),
            new CloningData("Explorer NPC", new ByName("Explorer", typeof(Npc)), CloningType.Copy),
            new CloningData("Healer NPC", new ByName("Healer", typeof(Npc)), CloningType.Copy),
            new CloningData("Mercant NPC", new ByName("MercantHub", typeof(Npc)), CloningType.Copy),
            new CloningData("Scarab Collector NPC", new ByName("ScarabCollector", typeof(Npc)), CloningType.Copy),
            new CloningData(Item.EnchantedPowers, new ByName("PickupModuleFreePower", typeof(ModulePickup))),
        }),
        new CrawlableScene("Dungeon1", new List<CloningData>()
        {
            new CloningData("Boss Key", new ByName("Dungeon1BossKey", typeof(BossKey))),
            new CloningData("Small Key", new ByName("Dungeon1CrystalKey0", typeof(CrystalKey))),
            new CloningData(Item.Dash, new ByName("SkillDash", typeof(SkillPickup))),
        }),
        new CrawlableScene("Dungeon2", new List<CloningData>()
        {
            new CloningData(Item.Supershot, new ByName("SkillSupershot", typeof(SkillPickup))),
        }),
        new CrawlableScene("Dungeon3", new List<CloningData>()
        {
            new CloningData(Item.Surf, new ByName("SkillHover", typeof(SkillPickup))),
        }),
        new CrawlableScene("Dungeon5", new List<CloningData>()
        {
            new CloningData(Item.DarkKey, new ByName("PickupKeyDarker", typeof(KeyUnique))),
        }),
        new CrawlableScene("Temple1", new List<CloningData>()
        {
            new CloningData(Item.PowerOfProtection, new ByName("CavePowerBombLevel0", typeof(StatsPickup))),
        }),
        new CrawlableScene("Temple2", new List<CloningData>()
        {
            new CloningData(Item.PowerOfTime, new ByName("Temple2PowerSlowLevel0", typeof(StatsPickup))),
        }),
        new CrawlableScene("Temple3", new List<CloningData>()
        {
            new CloningData(Item.PowerOfSpirits, new ByName("Temple3PowerAllyLevel0", typeof(StatsPickup))),
        }),
        new CrawlableScene("Tower", new List<CloningData>()
        {
            new CloningData(Item.SpiritDash, new ByName("PickupModuleSpiritDash", typeof(ModulePickup))),
        }),
    };

    public CrawlingState State { get; private set; } = CrawlingState.DidNotStart;

    void Awake()
    {
        _collector = Plugin.ServiceContainer.Get<IPrefabCollector>();
        _logger = Plugin.ServiceContainer.Get<ILogger>();
        _logger.LogInfo("SceneCrawler is created!");
    }

    void LateUpdate()
    {
        if (State != CrawlingState.Finished)
        {
            State = CrawlingState.Crawling;
            Scene currentScene = SceneManager.GetActiveScene();
            _logger.LogInfo("Current scene : " + currentScene.name);

            CrawlableScene currentCrawlableScene = _crawlableScenes.Find(crawlableScene => crawlableScene.SceneName == currentScene.name);
            if (currentCrawlableScene != null && currentCrawlableScene.IsCrawled == false) {
                _logger.LogInfo($"Crawling scene: {currentCrawlableScene.SceneName}");

                foreach (CloningData cloningData in currentCrawlableScene.CloningData)
                {
                    try {
                        _logger.LogInfo($"Cloning {cloningData.ItemName} to {cloningData.Selector}");
                        _collector.AddPrefab(cloningData.ItemName, cloningData.Selector, cloningData.CloningType);
                    } catch (System.Exception e) {
                        _logger.LogError(e.Message);
                    }
                }

                currentCrawlableScene.IsCrawled = true;
            }

            CrawlableScene nextCrawlableScene = _crawlableScenes.Find(crawlableScene => !crawlableScene.IsCrawled);
            if (nextCrawlableScene == null)
            {
                State = CrawlingState.Finished;
                _logger.LogInfo("Crawling finished!");
                Cleanup();
                SceneManager.LoadScene("-Main");
                
                return;
            }
            _logger.LogInfo($"Loading scene for crawling : {nextCrawlableScene.SceneName}");
            SceneManager.LoadScene(nextCrawlableScene.SceneName);
        }
    }

    private void Cleanup()
    {
        ReflectionHelper.ClearActionInvocationList(typeof(PlayerState), "KeysChanged");
    }
}

public enum CrawlingState
{
    DidNotStart,
    Crawling,
    Finished
}

public class CrawlableScene
{
    public string SceneName { get; set; }
    public List<CloningData> CloningData { get; set; }
    public bool IsCrawled { get; set; } = false;

    public CrawlableScene(string sceneName, List<CloningData> cloningData)
    {
        SceneName = sceneName;
        CloningData = cloningData;
    }
}

public class CloningData
{
    public string ItemName { get; set; }
    public ISelector Selector { get; set; }
    public CloningType CloningType { get; set; }

    public CloningData(string itemName, ISelector selector, CloningType cloningType = CloningType.Recreate)
    {
        ItemName = itemName;
        Selector = selector;
        CloningType = cloningType;
    }
}

public class CrawlingException : System.Exception
{
    public CrawlingException(string message) : base(message) { }
}
