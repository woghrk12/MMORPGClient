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
    protected EMoveDirection lastMoveDirection = EMoveDirection.RIGHT;

    protected ECreatureState state = ECreatureState.IDLE;
    protected bool isActing = false;

    #endregion Variables

    #region Properties

    public Vector3Int CellPos { set; get; } = Vector3Int.zero;

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

    public Vector3Int GetFrontCellPos()
    {
        Vector3Int cellPos = CellPos;

        switch (lastMoveDirection)
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

    private void UpdateAnimation()
    {
        animator.SetBool(AnimatorKey.Creature.IS_MOVE_HASH, state == ECreatureState.MOVE);

        if (state == ECreatureState.ATTACK)
        {
            animator.SetTrigger(AnimatorKey.Creature.DO_ATTACK_HASH);
        }
    }

    private void UpdateMoveState()
    {
        if (isActing == true) return;
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
        lastMoveDirection = moveDirection;

        if (Managers.Map.CheckCanMove(cellPos) == true && ReferenceEquals(Managers.Obj.Find(cellPos), null) == true)
        {
            CellPos = cellPos;
            isActing = true;
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
            isActing = false;

            if (moveDirection == EMoveDirection.NONE)
            {
                State = ECreatureState.IDLE;
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
