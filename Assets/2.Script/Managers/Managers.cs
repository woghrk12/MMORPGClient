using UnityEngine;

public class Managers : MonoBehaviour
{
    #region Variables

    private static Managers instance = null;

    private DataManager data = new();
    private PoolManager pool = new();
    private ResourceManager resource = new();
    private SceneManager scene = new();
    private SoundManager sound = new();
    private UIManager ui = new();

    private MapManager map = new();
    private ObjectManager obj = new();
    private NetworkManager network = new();

    #endregion Variables

    #region Properties

    public static Managers Instance
    {
        get
        {
            if (ReferenceEquals(instance, null) == true)
            {
                GameObject go = GameObject.Find("@Managers");

                if (ReferenceEquals(go, null) == true)
                {
                    go = new GameObject { name = "@Managers" };
                    instance = go.AddComponent<Managers>();

                    DontDestroyOnLoad(go);
                }

                Init();
            }
            
            return instance;
        }
    }

    public static DataManager Data => Instance.data;

    public static PoolManager Pool => Instance.pool;

    public static ResourceManager Resource => Instance.resource;

    public static SceneManager Scene => Instance.scene;

    public static SoundManager Sound => Instance.sound;

    public static UIManager UI => Instance.ui;

    public static MapManager Map => Instance.map;

    public static ObjectManager Obj => Instance.obj;

    public static NetworkManager Network => Instance.network;

    #endregion Properties

    #region Unity Events

    private void Update()
    {
        network.Update();
    }

    #endregion Unity Events
    
    #region Methods

    private static void Init()
    {
        instance.network.Init();
        instance.sound.Init();
        instance.pool.Init();
    }

    public static void Clear()
    {
        Scene.Clear();
        Sound.Clear();
        UI.Clear();
        Pool.Clear();
    }

    #endregion Methods
}
