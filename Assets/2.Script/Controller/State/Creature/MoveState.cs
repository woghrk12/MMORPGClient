using Google.Protobuf.Protocol;
using UnityEngine;

namespace Creature
{
    public class MoveState<T> : CreatureState where T : CreatureController
    {
        #region Variables

        protected T controller = null;

        #endregion Variables

        #region Properties

        public sealed override ECreatureState StateID => ECreatureState.Move;

        #endregion Properties

        #region Unity Events

        protected override void Awake()
        {
            base.Awake();

            controller = GetComponent<T>();
        }

        #endregion Unity Events

        #region Methods

        public override void OnStart()
        {
            animator.SetBool(AnimatorKey.Creature.IS_MOVE_HASH, true);
        }

        public override void OnFixedUpdate()
        {
            Vector3 destPos = new Vector3(controller.CellPos.x, controller.CellPos.y, 0f) + new Vector3(0.5f, 0.5f, 0f);
            Vector3 moveDir = destPos - transform.position;

            transform.position += controller.MoveSpeed * Time.fixedDeltaTime * moveDir.normalized;
        }

        public override void OnExit()
        {
            animator.SetBool(AnimatorKey.Creature.IS_MOVE_HASH, false);
        }

        #endregion Methods
    }
}