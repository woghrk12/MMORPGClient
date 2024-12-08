using Google.Protobuf.Protocol;

public class BaseAttackState<T> : BaseState<T> where T : Creature
{
    #region Properties

    public sealed override ECreatureState StateID => ECreatureState.Attack;

    #endregion Properties

    #region Constructor

    public BaseAttackState(T controller) : base(controller) { }

    #endregion Constructor
}
