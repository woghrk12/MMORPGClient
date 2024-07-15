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

    #endregion Variables

    #region Properties

    public static Managers Instance
    {
        get
        {
            Init();
            
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

    #endregion Properties

    #region Unity Events

    private void Awake()
    {
        Init();
    }

    #endregion Unity Events
    
    #region Methods

    private static void Init()
    {
        if (ReferenceEquals(instance, null))
        {
            GameObject go = GameObject.Find("@Managers");

            if (ReferenceEquals(go, null))
            {
                go = new GameObject { name = "@Managers" };
                go.AddComponent<Managers>();
            }

            DontDestroyOnLoad(go);
            instance = go.GetComponent<Managers>();
        }

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
