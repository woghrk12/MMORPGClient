using Google.Protobuf.Protocol;

namespace LocalPlayerState
{
    public class IdleState : State
    {
        #region Properties

        public sealed override ECreatureState StateID => ECreatureState.Idle;

        #endregion Properties

        #region Methods

        public override void OnUpdate(EPlayerInput input)
        {
            EPlayerInput directionInput = input & (EPlayerInput.UP | EPlayerInput.DOWN | EPlayerInput.LEFT | EPlayerInput.RIGHT);

            if (directionInput != EPlayerInput.NONE)
            {
                controller.SetState(ECreatureState.Move, directionInput);
                return;
            }
        }

        #endregion Methods
    }
}