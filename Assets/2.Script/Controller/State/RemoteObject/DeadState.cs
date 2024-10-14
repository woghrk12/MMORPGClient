using Google.Protobuf.Protocol;
using UnityEngine;

namespace RemoteObjectState
{
    public class DeadState : State
    {
        #region Properties

        public sealed override EObjectState StateID => EObjectState.Dead;

        #endregion Properties

        #region Methods

        public override void OnEnter()
        {
            controller.SpriteRenderer.enabled = false;
            controller.IsCollidable = false;

            GameObject effect = Managers.Resource.Instantiate("Effect/DieEffect");
            effect.transform.position = controller.transform.position;

            Destroy(effect, 0.5f);
        }

        public override void OnExit()
        {
            controller.SpriteRenderer.enabled = true;
            controller.IsCollidable = true;
        }

        #endregion Methods
    }
}