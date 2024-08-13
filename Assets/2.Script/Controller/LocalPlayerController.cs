using Google.Protobuf.Protocol;
using UnityEngine;

public class LocalPlayerController : PlayerController
{
    #region Variables

    private EMoveDirection inputMoveDirection = EMoveDirection.None;

    #endregion Variables

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

        if (Input.GetKey(KeyCode.Space))
        {
            //coSkill = StartCoroutine(StartBaseAttack());
            coSkill = StartCoroutine(StartSkillAttack());
        }

        base.UpdateIdleState();
    }

    #endregion States

    protected override void MoveToNextPos()
    {
        MoveDirection = inputMoveDirection;

        base.MoveToNextPos();
    }

    private void GetInputDirection()
    {
        if (Input.GetKey(KeyCode.W))
        {
            inputMoveDirection = EMoveDirection.Up;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            inputMoveDirection = EMoveDirection.Down;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            inputMoveDirection = EMoveDirection.Left;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            inputMoveDirection = EMoveDirection.Right;
        }
        else
        {
            inputMoveDirection = EMoveDirection.None;
        }
    }

    #endregion Methods
}
