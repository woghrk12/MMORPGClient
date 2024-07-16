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

public enum ECreatureState
{ 
    IDLE = 0,
    MOVE,
    ATTACK,
    DEAD,
}

public class CreatureController : MonoBehaviour
{
    #region Variables

    protected Animator animator = null;

    [SerializeField] protected float moveSpeed = 0f;

    protected Vector3Int cellPos = Vector3Int.zero;
    protected EMoveDirection moveDirection = EMoveDirection.NONE;

    protected ECreatureState state = ECreatureState.IDLE;

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

    public ECreatureState State
    {
        protected set
        {
            if (state == value) return;

            state = value;
        }
        get => state;
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
        if (State != ECreatureState.IDLE) return;
        if (MoveDirection == EMoveDirection.NONE) return;

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
        State = ECreatureState.MOVE;
    }

    private void UpdatePosition()
    {
        if (State != ECreatureState.MOVE) return;

        Vector3 destPos = Managers.Map.CurrentGrid.CellToWorld(cellPos) + new Vector3(0.5f, 0.5f, 0);

        if ((destPos - transform.position).sqrMagnitude < (moveSpeed * Time.fixedDeltaTime) * (moveSpeed * Time.fixedDeltaTime))
        {
            transform.position = destPos;
            State = ECreatureState.IDLE;
        }
        else
        {
            switch (MoveDirection)
            {
                case EMoveDirection.UP:
                    transform.position += moveSpeed * Time.fixedDeltaTime * Vector3.up;
                    break;

                case EMoveDirection.DOWN:
                    transform.position += moveSpeed * Time.fixedDeltaTime * Vector3.down;
                    break;

                case EMoveDirection.LEFT:
                    transform.position += moveSpeed * Time.fixedDeltaTime * Vector3.left;
                    break;

                case EMoveDirection.RIGHT:
                    transform.position += moveSpeed * Time.fixedDeltaTime * Vector3.right;
                    break;
            }
        }
    }

    #endregion Methods
}
