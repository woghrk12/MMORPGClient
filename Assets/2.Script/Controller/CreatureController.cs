using Google.Protobuf.Protocol;
using System.Collections.Generic;
using UnityEngine;

public class CreatureController : MonoBehaviour
{
    #region Variables

    private Dictionary<ECreatureState, CreatureState> stateDictionary = new();
    private CreatureState curState = null;

    protected Animator animator = null;

    private EMoveDirection facingDirection = EMoveDirection.None;

    #endregion Variables

    #region Properties

    public int ID { set; get; } = -1;

    public string Name { set; get; } = string.Empty;

    public Vector3Int CellPos { set; get; } = Vector3Int.zero;

    public ECreatureState CurState
    {
        set
        {
            if (ReferenceEquals(curState, null) == false && curState.StateID == value) return;
            if (stateDictionary.TryGetValue(value, out CreatureState state) == false) return;

            curState?.OnExit();
            curState = state;
            curState.OnStart();
        }
        get => ReferenceEquals(curState, null) == false ? curState.StateID : ECreatureState.Idle;
    }

    public EMoveDirection FacingDirection
    {
        set
        {
            if (facingDirection == value) return;

            facingDirection = value;

            if (facingDirection == EMoveDirection.Left)
            {
                transform.localScale = new Vector3(-1f, 1f, 1f);
            }
            if (facingDirection == EMoveDirection.Right)
            {
                transform.localScale = new Vector3(1f, 1f, 1f);
            }
        }
        get => facingDirection;
    }

    public int MoveSpeed { set; get; } = -1;

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

    #region Events

    public virtual void OnDamaged() { }

    #endregion Events

    #endregion Methods
}
