using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Managers : MonoBehaviour
{
    public static bool Initialized { get; set; }
    private static Managers s_instance;
    private static Managers Instance { get { Init(); return s_instance; } }

    // Core
    private PoolManager _pool = new PoolManager();
    private ResourceManager _resource = new ResourceManager();
    private SceneManagerEx _scene = new SceneManagerEx();
    private UIManager _ui = new UIManager();

    public static PoolManager Pool => Instance?._pool;
    public static ResourceManager Resource => Instance?._resource;
    public static SceneManagerEx Scene => Instance?._scene;
    public static UIManager UI => Instance?._ui;

    // Contents
    private GameManager _game = new GameManager();
    private MapManager _map = new MapManager();
    private ObjectManager _object = new ObjectManager();

    public static GameManager Game => Instance?._game;
    public static MapManager Map => Instance?._map;
    public static ObjectManager Object => Instance?._object;

    public static void Init()
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
        //Event.Clear();
        //Scene.Clear();
        //UI.Clear();
        //Object.Clear();
        //Pool.Clear();
    }
}
