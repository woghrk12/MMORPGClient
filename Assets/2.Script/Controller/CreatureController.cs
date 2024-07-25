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

    protected ECreatureState state = ECreatureState.IDLE;
    protected EMoveDirection moveDirection = EMoveDirection.NONE;

    #endregion Variables

    #region Properties

    public Vector3Int CellPos { set; get; } = Vector3Int.zero;

    public virtual ECreatureState State
    {
        protected set
        {
            if (state == value) return;

            state = value;

            UpdateAnimation();
        }
        get => state;
    }

    public EMoveDirection MoveDirection 
    {
        protected set
        {
            if (moveDirection == value) return;

            moveDirection = value;

            if (moveDirection == EMoveDirection.NONE) return;

            if (moveDirection == EMoveDirection.LEFT || moveDirection == EMoveDirection.RIGHT)
            {
                transform.localScale = new Vector3(moveDirection == EMoveDirection.LEFT ? -1f : 1f, 1f, 1f);
            }

            LastMoveDirection = moveDirection;
        }
        get => moveDirection; 
    }
    public EMoveDirection LastMoveDirection { protected set; get; } = EMoveDirection.RIGHT;

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
        switch (State)
        {
            case ECreatureState.IDLE:
                UpdateIdleState();
                break;

            case ECreatureState.MOVE:
                UpdateMoveState();
                break;

            case ECreatureState.ATTACK:
                UpdateAttackState();
                break;

            case ECreatureState.SKILL:
                UpdateSkillState();
                break;

            case ECreatureState.DEAD:
                UpdateDeadState();
                break;
        }
    }

    #endregion Unity Events

    #region Methods

    #region States

    protected virtual void UpdateIdleState() { }

    protected virtual void UpdateMoveState() 
    {
        Vector3 destPos = Managers.Map.CurrentGrid.CellToWorld(CellPos) + new Vector3(0.5f, 0.5f, 0);
        Vector3 moveVector = destPos - transform.position;

        if (moveVector.sqrMagnitude < (moveSpeed * Time.deltaTime) * (moveSpeed * Time.deltaTime))
        {
            transform.position = destPos;
            MoveToNextPos();
        }
        else
        {
            transform.position += moveSpeed * Time.deltaTime * moveVector.normalized;
        }
    }

    protected virtual void UpdateAttackState() { }

    protected virtual void UpdateSkillState() { }

    protected virtual void UpdateDeadState() { }

    #endregion States

    public Vector3Int GetFrontCellPos()
    {
        Vector3Int cellPos = CellPos;

        switch (LastMoveDirection)
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

        return cellPos;
    }

    protected virtual void UpdateAnimation()
    {
        animator.SetBool(AnimatorKey.Creature.IS_MOVE_HASH, state == ECreatureState.MOVE);

        if (state == ECreatureState.ATTACK)
        {
            animator.SetTrigger(AnimatorKey.Creature.DO_ATTACK_HASH);
        }
    }

    protected virtual void MoveToNextPos()
    {
        if (MoveDirection == EMoveDirection.NONE)
        {
            State = ECreatureState.IDLE;
            return;
        }

        Vector3Int cellPos = CellPos;

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
                transform.localScale = new Vector3(-1f, 1f, 1f);
                break;

            case EMoveDirection.RIGHT:
                cellPos += Vector3Int.right;
                transform.localScale = new Vector3(1f, 1f, 1f);
                break;
        }

        LastMoveDirection = MoveDirection;

        if (Managers.Map.CheckCanMove(cellPos) == true && ReferenceEquals(Managers.Obj.Find(cellPos), null) == true)
        {
            CellPos = cellPos;
        }
    }

    #region Events

    public virtual void OnDamaged() { }

    #endregion Events

    #endregion Methods
}
