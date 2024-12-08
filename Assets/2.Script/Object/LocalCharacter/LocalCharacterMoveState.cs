using Google.Protobuf.Protocol;
using UnityEngine;

public class LocalCharacterMoveState : BaseMoveState<LocalCharacter>, IInputHandler
{
    #region Constructor

    public LocalCharacterMoveState(LocalCharacter controller) : base(controller) { }

    #endregion Constructor

    #region Methods

    public override void OnUpdate()
    {
        HandleInput(controller.PlayerInput);
    }

    #region IInputHandler Implement

    public void HandleInput(EPlayerInput input)
    {
        Vector3 destPos = new Vector3(controller.Position.x, controller.Position.y) + new Vector3(0.5f, 0.5f);
        Vector3 moveDir = destPos - cachedTransform.position;

        if (moveDir.sqrMagnitude >= (controller.MoveSpeed * Time.deltaTime) * (controller.MoveSpeed * Time.deltaTime)) return;

        EPlayerInput directionInput = input & (EPlayerInput.UP | EPlayerInput.DOWN | EPlayerInput.LEFT | EPlayerInput.RIGHT);
        EMoveDirection moveDirection = EMoveDirection.None;
        Vector2Int targetPos = controller.Position;
        MoveRequest packet = new();

        if (directionInput == EPlayerInput.NONE)
        {
            packet.MoveDirection = EMoveDirection.None;
            packet.TargetPosX = targetPos.x;
            packet.TargetPosY = targetPos.y;

            Managers.Network.Send(packet);

            controller.CurState = ECreatureState.Idle;

            return;
        }

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

        if (Managers.Map.CheckCanMove(targetPos) == false) return;

        packet.MoveDirection = moveDirection;
        packet.TargetPosX = targetPos.x;
        packet.TargetPosY = targetPos.y;

        Managers.Network.Send(packet);

        controller.MoveDirection = moveDirection;
        Managers.Map.MoveObject(controller, targetPos);
    }

    #endregion IInputHandler Implement

    #endregion Methods
}
