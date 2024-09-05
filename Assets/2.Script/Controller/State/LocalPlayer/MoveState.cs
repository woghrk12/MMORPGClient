using Google.Protobuf.Protocol;
using UnityEngine;

namespace LocalPlayerState
{
    public class MoveState : State
    {
        #region Properties

        public sealed override ECreatureState StateID => ECreatureState.Move;

        #endregion Properties

        #region Methods

        public override void OnEnter(EPlayerInput input)
        {
            animator.SetBool(AnimatorKey.Creature.IS_MOVE_HASH, true);
        }

        public override void OnUpdate(EPlayerInput input)
        {
            Vector3 destPos = new Vector3(controller.CellPos.x, controller.CellPos.y, 0f) + new Vector3(0.5f, 0.5f, 0f);
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

        public override void OnExit(EPlayerInput input)
        {
            animator.SetBool(AnimatorKey.Creature.IS_MOVE_HASH, false);
        }

        #endregion Methods
    }
}