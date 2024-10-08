using Google.Protobuf.Protocol;
using UnityEngine;

namespace LocalPlayerState
{
    public abstract class State : MonoBehaviour
    {
        #region Variables

        protected Animator animator = null;
        protected LocalPlayer controller = null;

        #endregion Variables

        #region Properties
        
        public abstract EObjectState StateID { get; }

        #endregion Properties

        #region Unity Events

        protected virtual void Awake()
        {
            animator = GetComponent<Animator>();
            controller = GetComponent<LocalPlayer>();
        }

        #endregion Unity Events

        #region Methods

        public virtual void OnEnter(EPlayerInput input) { }

        public virtual void OnUpdate(EPlayerInput input) { }

        public virtual void OnExit(EPlayerInput input) { }

        #endregion Methods
    }
}