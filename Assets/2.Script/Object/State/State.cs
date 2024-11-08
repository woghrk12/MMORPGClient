using Google.Protobuf.Protocol;
using UnityEngine;

public abstract class State : MonoBehaviour
{
    #region Properties

    public abstract ECreatureState StateID { get; }

    #endregion Properties

    #region Event Methods

    public virtual void OnEnter() { }
    
    public virtual void OnUpdate() { }
    
    public virtual void OnExit() { }

    #endregion Event Methods
}
