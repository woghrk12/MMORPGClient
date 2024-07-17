using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : CreatureController
{
    #region Variables

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

    #endregion Methods
}
