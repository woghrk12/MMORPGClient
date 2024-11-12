using Google.Protobuf.Protocol;
using UnityEngine;

namespace LocalCharacterState
{
    public class MoveState : CreatureState.BaseState<LocalCharacter>
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

            if (moveDir.sqrMagnitude >= (controller.MoveSpeed * Time.deltaTime) * (controller.MoveSpeed * Time.deltaTime)) return;

            EPlayerInput directionInput = controller.PlayerInput & (EPlayerInput.UP | EPlayerInput.DOWN | EPlayerInput.LEFT | EPlayerInput.RIGHT);
            EMoveDirection moveDirection = EMoveDirection.None;
            Vector2Int targetPos = controller.Position;

            if ((directionInput & EPlayerInput.UP) != EPlayerInput.NONE)
            {
                moveDirection = EMoveDirection.Up;
                targetPos += Vector2Int.up;
            }
            else if ((directionInput & EPlayerInput.DOWN) != EPlayerInput.NONE)
            {
                moveDirection = EMoveDirection.Down;
                targetPos += Vector2Int.down;
            }
            else if ((directionInput & EPlayerInput.LEFT) != EPlayerInput.NONE)
            {
                moveDirection = EMoveDirection.Left;
                targetPos += Vector2Int.left;
            }
            else if ((directionInput & EPlayerInput.RIGHT) != EPlayerInput.NONE)
            {
                moveDirection = EMoveDirection.Right;
                targetPos += Vector2Int.right;
            }
            else
            {
                UpdateCreatureStateBroadcast updateCreatureStateBroadcastPacket = new()
                {
                    CreatureID = controller.ID,
                    NewState = ECreatureState.Idle
                };

                Managers.Network.Send(updateCreatureStateBroadcastPacket);

                controller.CurState = ECreatureState.Idle;
                return;
            }

            if (Managers.Map.CheckCanMove(targetPos) == false) return;

            MoveRequest packet = new()
            {
                MoveDirection = moveDirection,
                TargetPosX = targetPos.x,
                TargetPosY = targetPos.y
            };

            Managers.Network.Send(packet);
            Managers.Map.MoveObject(controller, targetPos);
        }

        public override void OnExit()
        {
            animator.SetBool(AnimatorKey.Creature.IS_MOVE_HASH, false);
        }

        #endregion Methods
    }
}
