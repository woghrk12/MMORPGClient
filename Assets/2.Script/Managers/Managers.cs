using System;

public class Managers : SingletonMonobehaviour<Managers>
{
    #region Variables

    private TaskQueue taskQueue = new();

    private DataManager data = new();
    private PoolManager pool = new();
    private ResourceManager resource = new();
    private SceneManager scene = new();
    private SoundManager sound = new();
    private UIManager ui = new();

    private MapManager map = new();
    private ObjectManager obj = new();
    private NetworkManager network = new();
    private InventoryManager inventory = new();

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

    public static InventoryManager Inventory => Instance.inventory;

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

        taskQueue.Flush();
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

    public static void Push(Action action, int afterTick) => Instance.taskQueue.Push(action, afterTick);
    public static void Push<P1>(Action<P1> action, P1 p1, int afterTick) => Instance.taskQueue.Push(action, p1, afterTick);
    public static void Push<P1, P2>(Action<P1, P2> action, P1 p1, P2 p2, int afterTick) => Instance.taskQueue.Push(action, p1, p2, afterTick);
    public static void Push<P1, P2, P3>(Action<P1, P2, P3> action, P1 p1, P2 p2, P3 p3, int afterTick) => Instance.taskQueue.Push(action, p1, p2, p3, afterTick);
    public static void Push<P1, P2, P3, P4>(Action<P1, P2, P3, P4> action, P1 p1, P2 p2, P3 p3, P4 p4, int afterTick) => Instance.taskQueue.Push(action, p1, p2, p3, p4, afterTick);

    #endregion Methods
}
