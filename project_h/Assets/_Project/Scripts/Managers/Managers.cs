using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Managers : MonoBehaviour
{
    private static bool Initialized { get; set; } = false;
    private static Managers s_instance;
    private static Managers Instance { get { Init(); return s_instance; } }

    // Core
    private PoolManager _pool = new PoolManager();
    private ResourceManager _resource = new ResourceManager();
    private SceneManagerEx _scene = new SceneManagerEx();
    private UIManager _ui = new UIManager();
    private LocaleManager _locale = new LocaleManager();
    private DataManager _data = new DataManager();
    private SaveLoadManager _saveLoad = new SaveLoadManager();

    public static PoolManager Pool => Instance?._pool;
    public static ResourceManager Resource => Instance?._resource;
    public static SceneManagerEx Scene => Instance?._scene;
    public static UIManager UI => Instance?._ui;
    public static LocaleManager Locale => Instance?._locale;
    public static DataManager Data => Instance?._data;
    public static SaveLoadManager SaveLoad => Instance?._saveLoad;

    // Contents
    private GameManager _game = new GameManager();
    private MapManager _map = new MapManager();
    private ObjectManager _object = new ObjectManager();
    private HeroManager _hero = new HeroManager();
    private DungeonManager _dungeon = new DungeonManager();
    private InventoryManager _inventory = new InventoryManager();

    public static GameManager Game => Instance?._game;
    public static MapManager Map => Instance?._map;
    public static ObjectManager Object => Instance?._object;
    public static HeroManager Hero => Instance?._hero;
    public static DungeonManager Dungeon => Instance?._dungeon;
    public static InventoryManager Inventory => Instance?._inventory;

    private static void Init()
    {
        if (s_instance == null && Initialized == false)
        {
            Initialized = true;

            Application.targetFrameRate = 60;
            Application.runInBackground = true;
            QualitySettings.vSyncCount = 0;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;

            GameObject managers = GameObject.Find("@Managers");
            if (managers == null)
            {
                managers = new GameObject { name = "@Managers" };
                managers.AddComponent<Managers>();
            }

            GameObject eventSystem = GameObject.Find("@EventSystem");
            if (eventSystem == null)
            {
                eventSystem = new GameObject { name = "@EventSystem" };
                eventSystem.AddComponent<EventSystem>();
                eventSystem.AddComponent<StandaloneInputModule>();
            }


            s_instance = managers.GetComponent<Managers>();

            DontDestroyOnLoad(managers);
            DontDestroyOnLoad(eventSystem);
        }
    }
    public static void Clear()
    {
        Scene.Clear();
        UI.Clear();
        Object.Clear();
        Pool.Clear();
    }
}
