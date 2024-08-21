using Google.Protobuf.Protocol;
using UnityEngine;

namespace LocalPlayer
{
    public class MoveState : Creature.MoveState<LocalPlayerController>
    {
        #region Methods
        
        protected override void NotifyMoveCompleted()
        {
            CreatureMoveRequest packet = new();

            packet.PosX = controller.CellPos.x;
            packet.PosY = controller.CellPos.y;
            packet.MoveDirection = controller.InputMoveDirection;

            Managers.Network.Send(packet);
        }

        #endregion Methods
    }
}
