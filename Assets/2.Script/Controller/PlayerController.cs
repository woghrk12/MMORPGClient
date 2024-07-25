using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : CreatureController
{
    #region Variables

    private Coroutine coSkill = null;
    private EMoveDirection inputMoveDirection = EMoveDirection.NONE;

    #endregion Variables

    #region Properties

    #endregion Properties

    #region Unity Events

    protected override void Update()
    {
        GetInputDirection();

        base.Update();
    }

    private void LateUpdate()
    {
        Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, -10f);
    }

    #endregion Unity Events

    #region Methods

    #region States

    protected override void UpdateIdleState()
    {
        MoveDirection = inputMoveDirection;

        if (MoveDirection != EMoveDirection.NONE)
        {
            State = ECreatureState.MOVE;
            return;
        }

        if (Input.GetKey(KeyCode.Space))
        {
            //coSkill = StartCoroutine(StartBaseAttack());
            coSkill = StartCoroutine(StartSkillAttack());
        }
    }

    #endregion States

    protected override void UpdateAnimation()
    {
        base.UpdateAnimation();

        if (state == ECreatureState.SKILL)
        {
            animator.SetTrigger(AnimatorKey.Creature.DO_SKILL_HASH);
        }
    }

    protected override void MoveToNextPos()
    {
        MoveDirection = inputMoveDirection;

        base.MoveToNextPos();
    }

    private void GetInputDirection()
    {
        if (Input.GetKey(KeyCode.W))
        {
            inputMoveDirection = EMoveDirection.UP;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            inputMoveDirection = EMoveDirection.DOWN;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            inputMoveDirection = EMoveDirection.LEFT;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            inputMoveDirection = EMoveDirection.RIGHT;
        }
        else
        {
            inputMoveDirection = EMoveDirection.NONE;
        }
    }

    private IEnumerator StartBaseAttack()
    {
        State = ECreatureState.ATTACK;

        GameObject go = Managers.Obj.Find(GetFrontCellPos());
        if (ReferenceEquals(go, null) == false && go.TryGetComponent(out CreatureController controller) == true)
        {
            controller.OnDamaged();
        }

        yield return new WaitForSeconds(0.5f);

        State = ECreatureState.IDLE;

        coSkill = null;
    }

    private IEnumerator StartSkillAttack()
    {
        State = ECreatureState.SKILL;

        GameObject go = Managers.Resource.Instantiate("Creature/BigSwordHero3_Skill1");
        ProjectileController controller = go.GetComponent<ProjectileController>();
        controller.SetProjectile(LastMoveDirection);
        controller.CellPos = CellPos;

        yield return new WaitForSeconds(0.3f);

        State = ECreatureState.IDLE;

        coSkill = null;
    }

    #endregion Methods
}
