using Google.Protobuf.Protocol;

public class BaseDeadState<T> : BaseState<T> where T : Creature
{
    #region Properties  

    public override ECreatureState StateID => ECreatureState.Dead;

    #endregion Properties

    #region Constructor

    public BaseDeadState(T controller) : base(controller) { }

    #endregion Constructor
}
