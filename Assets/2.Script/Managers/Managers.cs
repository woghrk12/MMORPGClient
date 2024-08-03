using UnityEngine;

public class Managers : SingletonMonobehaviour<Managers>
{
    #region Variables

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

    protected override void Awake()
    {
        base.Awake();

        Init();
    }

    private void Update()
    {
        network.Update();
    }

    private void OnDestroy()
    {
        network.Clear();
    }

    #endregion Unity Events

    #region Methods

    private static void Init()
    {
        Network.Init();
        Sound.Init();
        Pool.Init();
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
