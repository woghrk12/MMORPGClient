using Google.Protobuf.Protocol;

public class AttackState : LocalCharacterState
{
    #region Properties

    public sealed override ECreatureState StateID => ECreatureState.Attack;

    #endregion Properties
}
