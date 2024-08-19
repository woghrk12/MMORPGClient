using Google.Protobuf.Protocol;
using System.Collections.Generic;
using UnityEngine;

public enum ECreatureState
{ 
    NONE = -1,
    IDLE = 0,
    MOVE,
    ATTACK,
    SKILL,
    DEAD,
}

public class CreatureController : MonoBehaviour
{
    #region Variables

    private Dictionary<ECreatureState, CreatureState> stateDictionary = new();
    private CreatureState curState = null;

    protected Animator animator = null;

    protected EMoveDirection moveDirection = EMoveDirection.None;

    #endregion Variables

    #region Properties

    public int ID { set; get; } = -1;

    public Vector3Int CellPos { set; get; } = Vector3Int.zero;

    public EMoveDirection MoveDirection
    {
        set
        {
            if (moveDirection == value) return;

            moveDirection = value;

            if (curState.StateID == ECreatureState.MOVE && moveDirection == EMoveDirection.None)
            {
                SetState(ECreatureState.IDLE);
                return;
            }

            if (curState.StateID == ECreatureState.IDLE && moveDirection != EMoveDirection.None)
            {
                SetState(ECreatureState.MOVE);
            }

            if (moveDirection == EMoveDirection.Left)
            {
                transform.localScale = new Vector3(-1f, 1f, 1f);
            }
            if (moveDirection == EMoveDirection.Right)
            {
                transform.localScale = new Vector3(1f, 1f, 1f);
            }

            LastMoveDirection = moveDirection;
        }
        get => moveDirection;
    }

    public EMoveDirection LastMoveDirection { set; get; } = EMoveDirection.Right;

    #endregion Properties

    #region Unity Events

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();

        CreatureState[] states = GetComponents<CreatureState>();
        
        foreach (CreatureState state in states)
        {
            stateDictionary.Add(state.StateID, state);
        }
    }

    protected virtual void Start()
    {
        Vector3 pos = Managers.Map.CurrentGrid.CellToWorld(CellPos) + new Vector3(0.5f, 0.5f, 0);
        transform.position = pos;

        SetState(ECreatureState.IDLE);
    }

    protected virtual void Update()
    {
        curState?.OnUpdate();
    }

    protected virtual void FixedUpdate()
    {
        curState?.OnFixedUpdate();
    }

    protected virtual void LateUpdate()
    {
        curState?.OnLateUpdate();
    }

    #endregion Unity Events

    #region Methods

    public void SetState(ECreatureState stateID)
    {
        if (stateDictionary.TryGetValue(stateID, out CreatureState state) == false) return;

        curState?.OnExit();
        curState = state;
        curState.OnStart();
    }

    public Vector3Int GetFrontCellPos()
    {
        Vector3Int cellPos = CellPos;

        switch (LastMoveDirection)
        {
            case EMoveDirection.Up:
                cellPos += Vector3Int.up;
                break;

            case EMoveDirection.Down:
                cellPos += Vector3Int.down;
                break;

            case EMoveDirection.Left:
                cellPos += Vector3Int.left;
                break;

            case EMoveDirection.Right:
                cellPos += Vector3Int.right;
                break;
        }

        return cellPos;
    }

    #region Events

    public virtual void OnDamaged() { }

    #endregion Events

    #endregion Methods
}
