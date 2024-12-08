using Google.Protobuf.Protocol;

public class LocalCharacterIdleState : BaseIdleState<LocalCharacter>, IInputHandler
{
    #region Constructor

    public LocalCharacterIdleState(LocalCharacter controller) : base(controller) { }

    #endregion Constructor

    #region Methods

    public override void OnUpdate()
    {
        HandleInput(controller.PlayerInput);
    }

    #region IInputHandler Implement

    public void HandleInput(EPlayerInput input)
    {
        EPlayerInput attackInput = input & (EPlayerInput.ATTACK | EPlayerInput.SKILL);

        if (attackInput != EPlayerInput.NONE)
        {
            PerformAttackRequest packet = new()
            {
                AttackID = attackInput == EPlayerInput.ATTACK ? 1 : 2
            };

            Managers.Network.Send(packet);

            return;
        }

        EPlayerInput directionInput = input & (EPlayerInput.UP | EPlayerInput.DOWN | EPlayerInput.LEFT | EPlayerInput.RIGHT);

        if (directionInput != EPlayerInput.NONE)
        {
            controller.CurState = ECreatureState.Move;
            return;
        }
    }

    #endregion IInputHandler Implement

    #endregion Methods
}
