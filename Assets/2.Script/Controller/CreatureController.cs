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
    SKILL,
    DEAD,
}

public class CreatureController : MonoBehaviour
{
    #region Variables

    protected Animator animator = null;

    [SerializeField] protected float moveSpeed = 0f;

    protected EMoveDirection moveDirection = EMoveDirection.NONE;

    protected ECreatureState state = ECreatureState.IDLE;

    #endregion Variables

    #region Properties

    public Vector3Int CellPos { protected set; get; } = Vector3Int.zero;

    public ECreatureState State
    {
        protected set
        {
            if (state == value) return;

            state = value;

            UpdateAnimation();
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
        Vector3 pos = Managers.Map.CurrentGrid.CellToWorld(CellPos) + new Vector3(0.5f, 0.5f, 0);
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

    private void UpdateAnimation()
    {
        if (state == ECreatureState.IDLE)
        {
            animator.Play("Idle");
        }
        else if (state == ECreatureState.MOVE)
        {
            animator.Play("Move");
        }
        else if (state == ECreatureState.ATTACK)
        {
            animator.Play("Attack");
        }
        else if (state == ECreatureState.SKILL)
        {
            animator.Play("Skill");
        }
        else
        {
            animator.Play("Die");
        }
    }

    private void UpdateMoveState()
    {
        if (State != ECreatureState.IDLE) return;
        if (moveDirection == EMoveDirection.NONE) return;

        Vector3Int cellPos = CellPos;

        switch (moveDirection)
        {
            case EMoveDirection.UP:
                cellPos += Vector3Int.up;
                break;

            case EMoveDirection.DOWN:
                cellPos += Vector3Int.down;
                break;

            case EMoveDirection.LEFT:
                cellPos += Vector3Int.left;
                transform.localScale = new Vector3(-1f, 1f, 1f);
                break;

            case EMoveDirection.RIGHT:
                cellPos += Vector3Int.right;
                transform.localScale = new Vector3(1f, 1f, 1f);
                break;
        }

        State = ECreatureState.MOVE;

        if (Managers.Map.CheckCanMove(cellPos) == true && ReferenceEquals(Managers.Obj.Find(cellPos), null) == true)
        {
            CellPos = cellPos;
        }
    }

    private void UpdatePosition()
    {
        if (State != ECreatureState.MOVE) return;

        Vector3 destPos = Managers.Map.CurrentGrid.CellToWorld(CellPos) + new Vector3(0.5f, 0.5f, 0);
        Vector3 moveVector = destPos - transform.position;

        if (moveVector.sqrMagnitude < (moveSpeed * Time.fixedDeltaTime) * (moveSpeed * Time.fixedDeltaTime))
        {
            transform.position = destPos;
            state = ECreatureState.IDLE;

            if (moveDirection == EMoveDirection.NONE)
            {
                UpdateAnimation();
            }
        }
        else
        {
            transform.position += moveSpeed * Time.fixedDeltaTime * moveVector.normalized;
            State = ECreatureState.MOVE;
        }
    }

    #endregion Methods
}
