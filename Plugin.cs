using BepInEx;
using UnityEngine;

namespace MinishootRandomizer
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    [BepInProcess("Minishoot.exe")]
    public class Plugin : BaseUnityPlugin
    {
        private ILogger _logger;

        public static IServiceContainer ServiceContainer { get; private set; }

        public static bool IsDebug => true;
        public static bool UseDummyEngine = true;

        public static VersionNumber RandomizerVersion = new VersionNumber(PluginInfo.PLUGIN_VERSION);
        public static string ArchipelagoVersionConstraint = "~0.4.0";

        private void Awake()
        {
            ServiceContainer = new ComponentModelContainer(Logger);
            _logger = ServiceContainer.Get<ILogger>();

            // Plugin startup logic
            _logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
        }

        private void Start()
        {
            CreateSceneCrawler();
            CreateManager();
            CreateMessageWorker();
            CreateImgui();
        }

        private void CreateImgui()
        {
            GameObject imgui = new GameObject("RandomizerImgui");
            imgui.AddComponent<ImguiContextComponent>();
            DontDestroyOnLoad(imgui);
        }

        private void CreateMessageWorker()
        {
            GameObject messageWorker = new GameObject("RandomizerMessageWorker");
            messageWorker.AddComponent<MessageWorkerComponent>();
            DontDestroyOnLoad(messageWorker);
        }

        private GameObject CreateSceneCrawler()
        {
            GameObject sceneCrawler = new GameObject("SceneCrawler");
            sceneCrawler.AddComponent<SceneCrawlerComponent>();
            DontDestroyOnLoad(sceneCrawler);

            return sceneCrawler;
        }

        private GameObject CreateManager()
        {
            GameObject manager = new GameObject("RandomizerManager");
            manager.AddComponent<RandomizerManagerComponent>();
            DontDestroyOnLoad(manager);

            return manager;
        }
    }
}
