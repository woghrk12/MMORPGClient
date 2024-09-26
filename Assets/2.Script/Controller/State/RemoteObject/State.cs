using Google.Protobuf.Protocol;
using UnityEngine;

namespace RemoteObjectState
{
    public abstract class State : MonoBehaviour
    {
        #region Variables

        protected Animator animator = null;
        protected RemoteObject controller = null;

        #endregion Variables

        #region Properties

        public abstract EObjectState StateID { get; }

        #endregion Properties

        #region Unity Events

        protected virtual void Awake()
        {
            animator = GetComponent<Animator>();
            controller = GetComponent<RemoteObject>();
        }

        #endregion Unity Events

        #region Methods

        public virtual void OnEnter() { }

        public virtual void OnUpdate() { }

        public virtual void OnExit() { }

        #endregion Methods
    }
}
