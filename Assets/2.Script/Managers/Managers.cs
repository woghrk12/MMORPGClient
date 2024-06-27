using UnityEngine;

public class Managers : MonoBehaviour
{
    #region Variables

    private static Managers instance = null;

    private ResourceManager resource = new();

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

    public static ResourceManager Resource => Instance.resource;

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
    }

    #endregion Methods
}
