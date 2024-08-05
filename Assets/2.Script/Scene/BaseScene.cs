using UnityEngine;
using UnityEngine.EventSystems;

public enum EScene
{ 
    UNKNOWN,
    GAME,
}

public abstract class BaseScene : MonoBehaviour
{
    #region Properties

    public EScene SceneType { protected set; get; } = EScene.UNKNOWN;

    #endregion Properties

    #region Unity Events

    private void Start()
    {
        Init();
    }

    #endregion Unity Events

    #region Methods

    protected virtual void Init() 
    {
        Object obj = GameObject.FindObjectOfType(typeof(EventSystem));

        if (ReferenceEquals(obj, null))
        {
            Managers.Resource.Instantiate("UI/EventSystem").name = "@EventSystem";
        }
    }

    public abstract void Clear();

    #endregion Methods
}
