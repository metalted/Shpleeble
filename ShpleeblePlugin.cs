using BepInEx;
using HarmonyLib;

namespace Shpleeble
{
    [BepInPlugin(pluginGUID, pluginName, pluginVersion)]
    public class ShpleeblePlugin : BaseUnityPlugin
    {
        public const string pluginGUID = "com.metalted.zeepkist.shpleeble";
        public const string pluginName = "Shpleeble";
        public const string pluginVersion = "1.0.1";

        public static ShpleeblePlugin Instance;

        // =========================
        // Internal systems
        // =========================
        private ShpleeblePrefabCache _prefabCache;
        private ShpleebleFactory _factory;

        // =========================
        // Public API
        // =========================
        public static bool IsReady =>
            Instance != null &&
            Instance._prefabCache != null &&
            Instance._prefabCache.IsReady;

        /// <summary>
        /// Creates a new Shpleeble instance and returns its handle.
        /// Throws if the system is not ready yet.
        /// </summary>
        public static ShpleebleHandle Create()
        {
            if (Instance == null)
                throw new System.InvalidOperationException("Shpleeble plugin not initialized");

            if (!IsReady)
                throw new System.InvalidOperationException(
                    "Shpleeble prefab not ready yet. Wait until main menu has loaded."
                );

            ShpleebleHandle handle = Instance._factory.Create();
            handle.SetMode(CharacterMode.Race);
            return handle;
        }

        public static ShpleebleData GetLocalShpleebleData()
        {
            if(Instance == null)
            {
                throw new System.InvalidOperationException("Shpleeble plugin not initialized");
            }

            return ShpleebleUtils.GetLocalData();
        }

        // =========================
        // Lifecycle
        // =========================
        private void Awake()
        {
            Instance = this;

            _prefabCache = new ShpleeblePrefabCache();
            _factory = new ShpleebleFactory(_prefabCache);

            Harmony harmony = new Harmony(PluginInfo.PLUGIN_GUID);
            harmony.PatchAll();

            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_NAME} loaded");
        }

        internal void EnteredMainMenu()
        {
            if (_prefabCache.IsReady)
                return;

            _prefabCache.TryCaptureFromMainMenu();
        }
    }

    // =========================
    // Harmony hook
    // =========================
    [HarmonyPatch(typeof(MainMenuUI), "Awake")]
    internal class ShpleebleMainMenuUIAwakePatch
    {
        public static void Prefix()
        {
            ShpleeblePlugin.Instance?.EnteredMainMenu();
        }
    }
}
