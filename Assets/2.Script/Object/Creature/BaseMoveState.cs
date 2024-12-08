using Google.Protobuf.Protocol;

public class BaseMoveState<T> : BaseState<T> where T : Creature
{
    #region Properties

    public sealed override ECreatureState StateID => ECreatureState.Move;

    #endregion Properties

    #region Constructor

    public BaseMoveState(T controller) : base(controller) { }

    #endregion Constructor

    #region Methods

    public override void OnEnter()
    {
        cachedAnimator.SetBool(AnimatorKey.Creature.IS_MOVE_HASH, true);
    }

    public override void OnExit()
    {
        cachedAnimator.SetBool(AnimatorKey.Creature.IS_MOVE_HASH, false);
    }

    #endregion Methods
}
