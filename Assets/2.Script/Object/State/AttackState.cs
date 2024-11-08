using Google.Protobuf.Protocol;

public class AttackState : CreatureState
{
    #region Properties

    public override ECreatureState StateID => ECreatureState.Move;

    #endregion Properties

    #region Methods

    public override void OnEnter()
    {
        animator.SetTrigger(controller.AttackStat.AnimationKey);
    }

    public override void OnExit()
    {
        controller.AttackStat = null;
    }

    #endregion Methods
}
