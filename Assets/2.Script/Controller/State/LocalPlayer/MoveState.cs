using Google.Protobuf.Protocol;
using UnityEngine;

namespace LocalPlayerState
{
    public class MoveState : State
    {
        #region Properties

        public sealed override EObjectState StateID => EObjectState.Move;

        #endregion Properties

        #region Methods

        public override void OnEnter(EPlayerInput input)
        {
            animator.SetBool(AnimatorKey.Object.IS_MOVE_HASH, true);

            SetNextPos(input);
        }

        public override void OnUpdate(EPlayerInput input)
        {
            Vector3 destPos = new Vector3(controller.Position.x, controller.Position.y, 0f) + new Vector3(0.5f, 0.5f, 0f);
            Vector3 moveDir = destPos - transform.position;

            if (moveDir.sqrMagnitude < (controller.MoveSpeed * Time.deltaTime) * (controller.MoveSpeed * Time.deltaTime))
            {
                transform.position = destPos;
                SetNextPos(input);
            }
            else
            {
                transform.position += controller.MoveSpeed * Time.deltaTime * moveDir.normalized;
            }
        }

        public override void OnExit(EPlayerInput input)
        {
            animator.SetBool(AnimatorKey.Object.IS_MOVE_HASH, false);
        }

        private void SetNextPos(EPlayerInput input)
        {
            EMoveDirection moveDirection = controller.MoveDirection;
            EMoveDirection inputDirection = (EMoveDirection)input;
            PerformMoveRequest packet = new();

            if (inputDirection == EMoveDirection.None)
            {
                packet.MoveDirection = EMoveDirection.None;
                packet.CurPosX = controller.Position.x;
                packet.CurPosY = controller.Position.y;

                Managers.Network.Send(packet);

                controller.MoveDirection = EMoveDirection.None;
                controller.SetState(EObjectState.Idle, input);
                return;
            }

            // TODO : Handle the case where the player presses 3 or more directional keys
            if ((inputDirection & ~moveDirection) == EMoveDirection.None)
            {
                moveDirection = inputDirection;
            }
            else
            {
                moveDirection = inputDirection & ~moveDirection;
            }

            Vector3Int position = controller.Position;

            switch (moveDirection)
            {
                case EMoveDirection.Up:
                    position += new Vector3Int(0, 1, 0);
                    break;

                case EMoveDirection.Down:
                    position += new Vector3Int(0, -1, 0);
                    break;

                case EMoveDirection.Left:
                    position += new Vector3Int(-1, 0, 0);
                    break;

                case EMoveDirection.Right:
                    position += new Vector3Int(1, 0, 0);
                    break;
            }

            // When the character can move in the direction the player inputs
            if (Managers.Map.CheckCanMove(position) == true && Managers.Obj.TryFind(position, out GameObject obj) == false)
            {
                packet.MoveDirection = moveDirection;
                packet.CurPosX = controller.Position.x;
                packet.CurPosY = controller.Position.y;
                packet.TargetPosX = position.x;
                packet.TargetPosY = position.y;

                Managers.Network.Send(packet);

                controller.MoveDirection = moveDirection;
                controller.Position = position;
            }
            else
            {
                // When the character's movement direction is not EMoveDirection.None,
                // it indicates that the character is continuously attempting to move to an unreachable location
                // When the player's character attempts to move to an unreachable location for the first time
                if (controller.MoveDirection == EMoveDirection.None)
                {
                    packet.MoveDirection = moveDirection;
                    packet.CurPosX = controller.Position.x;
                    packet.CurPosY = controller.Position.y;

                    Managers.Network.Send(packet);

                    controller.MoveDirection = moveDirection;
                }
                // When the player inputs a new direction and the player's character is blocked from moving 
                else if (controller.MoveDirection != moveDirection)
                {
                    packet.MoveDirection = moveDirection;
                    packet.CurPosX = controller.Position.x;
                    packet.CurPosY = controller.Position.y;

                    Managers.Network.Send(packet);

                    controller.MoveDirection = moveDirection;
                }
            }
        }

        #endregion Methods
    }
}