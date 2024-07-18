using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : CreatureController
{
    #region Variables

    private Coroutine coSkill = null;

    #endregion Variables

    #region Properties

    #endregion Properties

    #region Unity Events

    protected override void Update()
    {
        GetAttackInput();
        GetInputDirection();

        base.Update();
    }

    private void LateUpdate()
    {
        Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, -10f);
    }

    #endregion Unity Events

    #region Methods

    protected override void UpdateAnimation()
    {
        base.UpdateAnimation();

        if (state == ECreatureState.SKILL)
        {
            animator.SetTrigger(AnimatorKey.Creature.DO_SKILL_HASH);
        }
    }

    private void GetInputDirection()
    {
        if (Input.GetKey(KeyCode.W))
        {
            MoveDirection = EMoveDirection.UP;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            MoveDirection = EMoveDirection.DOWN;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            MoveDirection = EMoveDirection.LEFT;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            MoveDirection = EMoveDirection.RIGHT;
        }
        else
        {
            MoveDirection = EMoveDirection.NONE;
        }
    }

    private void GetAttackInput()
    {
        if (isActing == true) return;

        if (Input.GetKey(KeyCode.Space))
        {
            //coSkill = StartCoroutine(StartBaseAttack());
            coSkill = StartCoroutine(StartSkillAttack());
        }
    }

    private IEnumerator StartBaseAttack()
    {
        State = ECreatureState.ATTACK;
        isActing = true;

        GameObject go = Managers.Obj.Find(GetFrontCellPos());
        if (go != null)
        {
            Debug.Log(go.name);
        }

        yield return new WaitForSeconds(0.5f);

        State = ECreatureState.IDLE;
        isActing = false;

        coSkill = null;
    }

    private IEnumerator StartSkillAttack()
    {
        State = ECreatureState.SKILL;
        isActing = true;

        GameObject go = Managers.Resource.Instantiate("Creature/BigSwordHero3_Skill1");
        ProjectileController controller = go.GetComponent<ProjectileController>();
        controller.MoveDirection = LastMoveDirection;
        controller.CellPos = CellPos;

        yield return new WaitForSeconds(0.3f);

        State = ECreatureState.IDLE;
        isActing = false;

        coSkill = null;
    }

    #endregion Methods
}
