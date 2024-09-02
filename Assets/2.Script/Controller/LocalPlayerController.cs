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
        EMoveDirection direction = InputMoveDirection;

        if (Input.GetKeyDown(KeyCode.W))
        {
            InputMoveDirection |= EMoveDirection.Up;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            InputMoveDirection |= EMoveDirection.Down;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            InputMoveDirection |= EMoveDirection.Left;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            InputMoveDirection |= EMoveDirection.Right;
        }

        if (Input.GetKeyUp(KeyCode.W))
        {
            InputMoveDirection &= ~EMoveDirection.Up;
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            InputMoveDirection &= ~EMoveDirection.Down;
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            InputMoveDirection &= ~EMoveDirection.Left;
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            InputMoveDirection &= ~EMoveDirection.Right;
        }

        if (direction != InputMoveDirection)
        {
            InputDirectionRequest packet = new()
            {
                MoveDirection = InputMoveDirection,
            };

            Managers.Network.Send(packet);
        }
    }


    #endregion Methods
}
