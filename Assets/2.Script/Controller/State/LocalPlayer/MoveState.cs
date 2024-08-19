namespace LocalPlayer
{
    public class MoveState : Creature.MoveState<LocalPlayerController>
    {
        #region Methods

        protected override void SetNextPos()
        {
            controller.MoveDirection = controller.InputMoveDirection;

            base.SetNextPos();
        }

        #region Events

        public override void OnUpdate()
        {
            controller.GetInputDirection();
        }

        #endregion Events

        #endregion Methods
    }
}
