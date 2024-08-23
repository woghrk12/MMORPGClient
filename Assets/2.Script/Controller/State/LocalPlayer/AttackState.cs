using Google.Protobuf.Protocol;
using UnityEngine;

namespace LocalPlayer
{
    public class AttackState : Creature.AttackState<LocalPlayerController>
    {
        #region Variables

        [SerializeField] private float attackDelay = 1f;

        private float curDelay = 0f;

        #endregion Variables

        #region Methods

        public override void OnStart()
        {
            animator.SetTrigger(AnimatorKey.Creature.DO_ATTACK_HASH);

            curDelay = 0f;

            GameObject go = Managers.Obj.Find(this.controller.GetFrontCellPos());
            if (ReferenceEquals(go, null) == false && go.TryGetComponent(out CreatureController controller) == true)
            {
                controller.OnDamaged();
            }
        }

        public override void OnUpdate()
        {
            curDelay += Time.deltaTime;

            if (curDelay > attackDelay)
            {
                controller.SetState(ECreatureState.Idle);
            }
        }

        #endregion Methods
    }
}
