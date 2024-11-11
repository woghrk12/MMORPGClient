using Google.Protobuf.Protocol;
using UnityEngine;

public abstract class State : MonoBehaviour
{
    #region Variables

    protected Animator animator = null;

    #endregion Variables

    #region Properties

    public abstract ECreatureState StateID { get; }

    #endregion Properties

    #region Unity Events

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
    }

    #endregion Unity Events

    #region Event Methods

    public virtual void OnEnter() { }
    
    public virtual void OnUpdate() { }
    
    public virtual void OnExit() { }

    #endregion Event Methods
}
