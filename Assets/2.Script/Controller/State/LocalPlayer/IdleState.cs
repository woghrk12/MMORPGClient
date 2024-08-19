using Google.Protobuf.Protocol;

namespace LocalPlayer
{
    public class IdleState : Creature.IdleState<LocalPlayerController>
    {
        #region Methods

        #region Events

        public override void OnUpdate()
        {
            controller.GetInputDirection();
            controller.GetInputAttack();

            if (controller.InputMoveDirection != EMoveDirection.None)
            {
                controller.SetState(ECreatureState.MOVE);
            }
        }

        #endregion Events

        #endregion Methods
    }
}
