using Google.Protobuf.Protocol;

namespace LocalPlayer
{
    public class IdleState : Creature.IdleState<LocalPlayerController>
    {
        #region Methods

        #region Events

        public override void OnUpdate()
        {
            controller.GetInputAttack();

            if (controller.InputMoveDirection != EMoveDirection.None)
            {
                controller.SetState(ECreatureState.Move);
            }
        }

        #endregion Events

        #endregion Methods
    }
}
