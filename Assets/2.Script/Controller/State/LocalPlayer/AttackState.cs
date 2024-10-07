using Google.Protobuf.Protocol;

namespace LocalPlayerState
{
    public class AttackState : State
    {
        #region Variables

        private Data.AttackStat attackStat = null;

        #endregion Variables

        #region Properties

        public sealed override EObjectState StateID => EObjectState.Attack;

        #endregion Properties

        #region Methods

        public void SetAttackType(int attackID)
        {
            attackStat = Managers.Data.AttackStatDictionary[attackID];
        }

        public override void OnEnter(EPlayerInput input)
        {
            animator.SetTrigger(attackStat.AnimationKey);
        }

        public override void OnExit(EPlayerInput input)
        {
            attackStat = null;
        }

        #endregion Methods
    }
}