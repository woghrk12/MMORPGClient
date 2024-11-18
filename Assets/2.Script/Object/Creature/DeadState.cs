using Google.Protobuf.Protocol;

namespace CreatureState
{
    public class DeadState : BaseState<Creature>
    {
        #region Properties

        public sealed override ECreatureState StateID => ECreatureState.Dead;

        #endregion Properties

        #region Methods

        public override void OnEnter()
        {
            animator.SetBool(AnimatorKey.Creature.IS_DEAD_HASH, true);
        }

        public override void OnExit()
        {
            animator.SetBool(AnimatorKey.Creature.IS_DEAD_HASH, false);
        }

        #endregion Methods
    }
}
