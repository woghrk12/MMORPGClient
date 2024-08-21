using Google.Protobuf.Protocol;
using UnityEngine;

public class LocalPlayerController : PlayerController
{
    #region Variables

    private Camera mainCamera = null;

    #endregion Variables

    #region Properties

    public EMoveDirection InputMoveDirection { private set; get; } = EMoveDirection.None;

    #endregion Properties

    #region Unity Events

    protected override void Awake()
    {
        base.Awake();

        mainCamera = Camera.main;
    }

    protected override void Update()
    {
        GetInputDirection();

        base.Update();
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();

        mainCamera.transform.position = new Vector3(transform.position.x, transform.position.y, -10f);
    }

    #endregion Unity Events

    #region Methods

    public void GetInputDirection()
    {
        if (Input.GetKey(KeyCode.W))
        {
            InputMoveDirection = EMoveDirection.Up;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            InputMoveDirection = EMoveDirection.Down;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            InputMoveDirection = EMoveDirection.Left;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            InputMoveDirection = EMoveDirection.Right;
        }
        else
        {
            InputMoveDirection = EMoveDirection.None;
        }
    }

    public void GetInputAttack()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            Debug.Log("Attack");
            SetState(ECreatureState.ATTACK);
        }
    }

    #endregion Methods
}
