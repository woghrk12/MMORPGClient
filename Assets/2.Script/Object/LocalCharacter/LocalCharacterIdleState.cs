using Google.Protobuf.Protocol;
using System;

public class LocalCharacterIdleState : BaseIdleState<LocalCharacter>, IInputHandler
{
    #region Variables

    private int nextInputTick = 0;

    #endregion Variables

    #region Constructor

    public LocalCharacterIdleState(LocalCharacter controller) : base(controller) { }

    #endregion Constructor

    #region Methods

    public override void OnEnter()
    {
        base.OnEnter();

        nextInputTick = Environment.TickCount;
    }

    public override void OnUpdate()
    {
        HandleInput(controller.PlayerInput);
    }

    #region IInputHandler Implement

    public void HandleInput(EPlayerInput input)
    {
        EPlayerInput attackInput = input & (EPlayerInput.ATTACK | EPlayerInput.SKILL);

        if (nextInputTick > Environment.TickCount) return;

        if (attackInput != EPlayerInput.NONE)
        {
            nextInputTick = Environment.TickCount + 200;

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
