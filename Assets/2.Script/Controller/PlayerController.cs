using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EMoveDirection
{ 
    NONE = -1,
    UP,
    DOWN,
    LEFT,
    RIGHT
}

public class PlayerController : MonoBehaviour
{
    #region Variables

    private Animator animator = null;

    [SerializeField] private float moveSpeed = 0f;

    private Vector3Int cellPos = Vector3Int.zero;
    private EMoveDirection moveDirection = EMoveDirection.NONE;
    private bool isMoving = false;

    #endregion Variables

    #region Properties

    public EMoveDirection MoveDirection
    {
        private set
        {
            if (moveDirection == value) return;

            switch (value)
            {
                case EMoveDirection.UP:
                    animator.Play("Move");
                    break;

                case EMoveDirection.DOWN:
                    animator.Play("Move");
                    break;

                case EMoveDirection.LEFT:
                    animator.Play("Move");
                    transform.localScale = new Vector3(-1f, 1f, 1f);
                    break;

                case EMoveDirection.RIGHT:
                    animator.Play("Move");
                    transform.localScale = new Vector3(1f, 1f, 1f);
                    break;

                case EMoveDirection.NONE:
                    animator.Play("Idle");
                    break;
            }

            moveDirection = value;
        }
        get => moveDirection;
    }

    #endregion Properties

    #region Unity Events

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        Vector3 pos = Managers.Map.CurrentGrid.CellToWorld(cellPos) + new Vector3(0.5f, 0.5f, 0);
        transform.position = pos;
    }

    private void Update()
    {
        GetInputDirection();
        UpdateMoveState();
    }

    private void FixedUpdate()
    {
        UpdatePosition();
    }

    #endregion Unity Events

    #region Methods

    private void GetInputDirection()
    {
        if (isMoving == true) return;

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

    private void UpdateMoveState()
    {
        if (isMoving == true) return;

        switch (MoveDirection)
        {
            case EMoveDirection.UP:
                cellPos += Vector3Int.up;
                isMoving = true;
                break;

            case EMoveDirection.DOWN:
                cellPos += Vector3Int.down;
                isMoving = true;
                break;

            case EMoveDirection.LEFT:
                cellPos += Vector3Int.left;
                isMoving = true;
                break;

            case EMoveDirection.RIGHT:
                cellPos += Vector3Int.right;
                isMoving = true;
                break;
        }
    }

    private void UpdatePosition()
    {
        if (isMoving == false) return;

        Vector3 destPos = Managers.Map.CurrentGrid.CellToWorld(cellPos) + new Vector3(0.5f, 0.5f, 0);

        if ((destPos - transform.position).sqrMagnitude < (moveSpeed * Time.fixedDeltaTime) * (moveSpeed * Time.fixedDeltaTime))
        {
            transform.position = destPos;
            isMoving = false;
        }
        else
        {
            switch (MoveDirection)
            {
                case EMoveDirection.UP:
                    transform.position += moveSpeed * Time.fixedDeltaTime * Vector3.up;
                    isMoving = true;
                    break;

                case EMoveDirection.DOWN:
                    transform.position += moveSpeed * Time.fixedDeltaTime * Vector3.down;
                    isMoving = true;
                    break;

                case EMoveDirection.LEFT:
                    transform.position += moveSpeed * Time.fixedDeltaTime * Vector3.left;
                    isMoving = true;
                    break;

                case EMoveDirection.RIGHT:
                    transform.position += moveSpeed * Time.fixedDeltaTime * Vector3.right;
                    isMoving = true;
                    break;
            }
        }
    }

    #endregion Methods
}
