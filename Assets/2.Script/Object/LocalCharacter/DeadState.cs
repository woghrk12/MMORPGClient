using Google.Protobuf.Protocol;

public class DeadState : LocalCharacterState
{
    #region Properties

    public sealed override ECreatureState StateID => ECreatureState.Dead;

    #endregion Properties
}
