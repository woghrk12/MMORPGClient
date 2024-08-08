using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : CreatureController
{
    #region Variables

    protected Coroutine coSkill = null;

    #endregion Variables

    #region Methods

    protected override void UpdateIdleState()
    {
        if (MoveDirection != EMoveDirection.NONE)
        {
            State = ECreatureState.MOVE;
            return;
        }
    }

    protected override void UpdateAnimation()
    {
        base.UpdateAnimation();

        if (state == ECreatureState.SKILL)
        {
            animator.SetTrigger(AnimatorKey.Creature.DO_SKILL_HASH);
        }
    }

    protected IEnumerator StartBaseAttack()
    {
        State = ECreatureState.ATTACK;

        GameObject go = Managers.Obj.Find(GetFrontCellPos());
        if (ReferenceEquals(go, null) == false && go.TryGetComponent(out CreatureController controller) == true)
        {
            controller.OnDamaged();
        }

        yield return new WaitForSeconds(1f);

        State = ECreatureState.IDLE;

        coSkill = null;
    }

    protected IEnumerator StartSkillAttack()
    {
        State = ECreatureState.SKILL;

        GameObject go = Managers.Resource.Instantiate("Creature/BigSwordHero3_Skill1");
        ProjectileController controller = go.GetComponent<ProjectileController>();
        controller.SetProjectile(LastMoveDirection);
        controller.CellPos = CellPos;

        yield return new WaitForSeconds(1f);

        State = ECreatureState.IDLE;

        coSkill = null;
    }

    #region Events

    public override void OnDamaged()
    {
        Debug.Log("OnDamaged");
    }

    #endregion Events

    #endregion Methods
}
