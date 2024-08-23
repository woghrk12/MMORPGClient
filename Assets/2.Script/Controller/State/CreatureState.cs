using Google.Protobuf.Protocol;
using UnityEngine;

public abstract class CreatureState : MonoBehaviour
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

    #region Methods

    public virtual void OnStart() { }

    public virtual void OnUpdate() { }

    public virtual void OnFixedUpdate() { }

    public virtual void OnLateUpdate() { }

    public virtual void OnExit() { }

    #endregion Methods
}
