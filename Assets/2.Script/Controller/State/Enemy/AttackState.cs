using UnityEngine;

namespace Monster
{
    public class AttackState : Creature.AttackState<MonsterController>
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

            if (ReferenceEquals(controller.Target, null) == false)
            {
                controller.Target.OnDamaged();
            }
        }

        public override void OnUpdate()
        {
            curDelay += Time.deltaTime;

            if (curDelay > attackDelay)
            {
                controller.SetState(ECreatureState.MOVE);
            }
        }

        #endregion Methods
    }
}
