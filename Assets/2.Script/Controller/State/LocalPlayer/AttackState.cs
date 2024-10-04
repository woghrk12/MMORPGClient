using Google.Protobuf.Protocol;
using System;

namespace LocalPlayerState
{
    public class AttackState : State
    {
        #region Variables

        private AttackInfo attackInfo = null;
        private long attackStartTicks = 0;

        #endregion Variables

        #region Properties

        public sealed override EObjectState StateID => EObjectState.Attack;

        #endregion Properties

        #region Methods

        public void SetAttackType(long attackStartTicks, AttackInfo attackInfo)
        {
            this.attackStartTicks = attackStartTicks;
            this.attackInfo = attackInfo;
        }

        public override void OnEnter(EPlayerInput input)
        {
            if (ReferenceEquals(attackInfo, null) == true)
            {
                controller.SetState(EObjectState.Idle, input);
                return;
            }

            if (attackInfo.AttackID == 1 || attackInfo.AttackID == 2)
            {
                animator.SetTrigger(AnimatorKey.Object.DO_ATTACK_HASH);
            }
        }

        public override void OnUpdate(EPlayerInput input)
        {
            if (DateTime.UtcNow.Ticks - attackStartTicks < 5 * 100 * 10000) return;

            controller.SetState(EObjectState.Idle, input);
        }

        public override void OnExit(EPlayerInput input)
        {
            attackInfo = null;

            AttackCompleteRequest packet = new();

            Managers.Network.Send(packet);
        }

        #endregion Methods
    }
}