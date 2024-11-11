using Google.Protobuf.Protocol;
using UnityEngine;

namespace CreatureState
{
    public class MoveState : BaseState<Creature>
    {
        #region Properties

        public sealed override ECreatureState StateID => ECreatureState.Move;

        #endregion Properties

        #region Methods

        public override void OnEnter()
        {
            animator.SetBool(AnimatorKey.Creature.IS_MOVE_HASH, true);
        }

        public override void OnUpdate()
        {
            Vector3 destPos = new Vector3(controller.Position.x, controller.Position.y) + new Vector3(0.5f, 0.5f);
            Vector3 moveDir = destPos - transform.position;

            if (moveDir.sqrMagnitude < (controller.MoveSpeed * Time.deltaTime) * (controller.MoveSpeed * Time.deltaTime))
            {
                transform.position = destPos;
            }
            else
            {
                transform.position += controller.MoveSpeed * Time.deltaTime * moveDir.normalized;
            }
        }

        public override void OnExit()
        {
            animator.SetBool(AnimatorKey.Creature.IS_MOVE_HASH, false);
        }

        #endregion Methods
    }
}
