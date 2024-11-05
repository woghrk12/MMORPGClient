using Google.Protobuf.Protocol;
using System;

namespace RemoteObjectState
{
    public class AttackState : State
    {
        #region Variables

        private Data.AttackStat attackStat = null;

        #endregion Variables

        #region Properties

        public sealed override ECreatureState StateID => ECreatureState.Attack;

        #endregion Properties

        #region Methods

        public void SetAttackType(int attackID)
        {
            attackStat = Managers.Data.AttackStatDictionary[attackID];
        }

        public override void OnEnter()
        {
            animator.SetTrigger(attackStat.AnimationKey);
        }

        public override void OnExit()
        {
            attackStat = null;
        }

        #endregion Methods
    }
}
