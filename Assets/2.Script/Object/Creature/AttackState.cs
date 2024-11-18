using Google.Protobuf.Protocol;

namespace CreatureState
{
    public class AttackState : BaseState<Creature>
    {
        #region Properties

        public sealed override ECreatureState StateID => ECreatureState.Attack;

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
}
