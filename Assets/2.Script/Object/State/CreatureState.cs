using Google.Protobuf.Protocol;
using UnityEngine;

public abstract class CreatureState : MonoBehaviour
{
    #region Variables

    protected Animator animator = null;

    protected Creature controller = null;

    #endregion Variables

    #region Properties

    public abstract ECreatureState StateID { get; }

    #endregion Properties

    #region Unity Events

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();

        controller = GetComponent<Creature>();
    }

    #endregion Unity Events

    #region Event Methods

    public virtual void OnEnter() { }
    
    public virtual void OnUpdate() { }
    
    public virtual void OnExit() { }

    #endregion Event Methods
}
