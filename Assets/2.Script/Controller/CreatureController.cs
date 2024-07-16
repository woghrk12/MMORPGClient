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

public class CreatureController : MonoBehaviour
{
    #region Variables

    protected Animator animator = null;

    [SerializeField] protected float moveSpeed = 0f;

    protected Vector3Int cellPos = Vector3Int.zero;
    protected EMoveDirection moveDirection = EMoveDirection.NONE;
    protected bool isMoving = false;

    #endregion Variables

    #region Properties

    public EMoveDirection MoveDirection
    {
        protected set
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

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
    }

    protected virtual void Start()
    {
        Vector3 pos = Managers.Map.CurrentGrid.CellToWorld(cellPos) + new Vector3(0.5f, 0.5f, 0);
        transform.position = pos;
    }

    protected virtual void Update()
    {
        UpdateMoveState();
    }

    protected virtual void FixedUpdate()
    {
        UpdatePosition();
    }

    #endregion Unity Events

    #region Methods

    private void UpdateMoveState()
    {
        if (isMoving == true) return;

        Vector3Int cellPos = this.cellPos;

        switch (MoveDirection)
        {
            case EMoveDirection.UP:
                cellPos += Vector3Int.up;
                break;

            case EMoveDirection.DOWN:
                cellPos += Vector3Int.down;
                break;

            case EMoveDirection.LEFT:
                cellPos += Vector3Int.left;
                break;

            case EMoveDirection.RIGHT:
                cellPos += Vector3Int.right;
                break;
        }

        if (Managers.Map.CheckCanMove(cellPos) == false) return;

        this.cellPos = cellPos;
        isMoving = true;
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
