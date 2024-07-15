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

    [SerializeField] private float moveSpeed = 0f;

    [SerializeField] private Grid grid = null;

    private Vector3Int cellPos = Vector3Int.zero;
    private EMoveDirection moveDirection = EMoveDirection.NONE;
    private bool isMoving = false;

    #endregion Variables

    #region Unity Events

    private void Start()
    {
        Vector3 pos = grid.CellToWorld(cellPos) + new Vector3(0.5f, 0.5f, 0);
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

    private void UpdateMoveState()
    {
        if (isMoving == true) return;

        switch (moveDirection)
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

        Vector3 destPos = grid.CellToWorld(cellPos) + new Vector3(0.5f, 0.5f, 0);

        if ((destPos - transform.position).sqrMagnitude < (moveSpeed * Time.fixedDeltaTime) * (moveSpeed * Time.fixedDeltaTime))
        {
            transform.position = destPos;
            isMoving = false;
        }
        else
        {
            switch (moveDirection)
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
