using Google.Protobuf.Protocol;

public class BaseIdleState<T> : BaseState<T> where T : Creature
{
    #region Properties

    public sealed override ECreatureState StateID => ECreatureState.Idle;

    #endregion Properties

    #region Constructor

    public BaseIdleState(T controller) : base(controller) { }

    #endregion Constructor
}
