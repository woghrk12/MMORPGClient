using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum EScene
{ 
    UNKNOWN,
}

public abstract class BaseScene : MonoBehaviour
{
    #region Properties

    public EScene SceneType { protected set; get; } = EScene.UNKNOWN;

    #endregion Properties

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
