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

    private void GetInputDirection()
    {
        if (Input.GetKey(KeyCode.W))
        {
            moveDirection = EMoveDirection.UP;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            moveDirection = EMoveDirection.DOWN;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            moveDirection = EMoveDirection.LEFT;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            moveDirection = EMoveDirection.RIGHT;
        }
        else
        {
            moveDirection = EMoveDirection.NONE;
        }
    }

    private void GetAttackInput()
    {
        if (isActing == true) return;

        if (Input.GetKey(KeyCode.Space))
        {
            coSkill = StartCoroutine(StartBaseAttack());
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

    #endregion Methods
}
