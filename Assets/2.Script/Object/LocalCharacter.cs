using Google.Protobuf.Protocol;
using System.Collections.Generic;
using UnityEngine;

public enum EPlayerInput
{
    NONE = 0,
    UP = 1 << 0,
    DOWN = 1 << 1,
    LEFT = 1 << 2,
    RIGHT = 1 << 3,
    ATTACK = 1 << 4,
    SKILL = 1 << 5,
}

public class LocalCharacter : Character
{
    #region Variables

    private Transform cachedTransform = null;
    private Camera mainCamera = null;

    private Dictionary<ECreatureState, LocalCharacterState> stateDictionary = new();
    private LocalCharacterState curState = null;

    #endregion Variables

    #region Properties

    public EPlayerInput PlayerInput { protected set; get; } = EPlayerInput.NONE;


    public override ECreatureState CurState
    {
        set
        {
            if (stateDictionary.ContainsKey(value) == false) return;

            if (ReferenceEquals(curState, null) == false)
            {
                if (curState.StateID == value) return;

                curState.OnExit();
            }

            curState = stateDictionary[value];
            curState.OnEnter();
        }
        get => ReferenceEquals(curState, null) == false ? curState.StateID : ECreatureState.Idle;
    }

    #endregion Properties

    #region Unity Events

    protected override void Awake()
    {
        base.Awake();

        stateDictionary.Add(ECreatureState.Idle, gameObject.AddComponent<IdleState>());
        stateDictionary.Add(ECreatureState.Move, gameObject.AddComponent<MoveState>());
        stateDictionary.Add(ECreatureState.Attack, gameObject.AddComponent<AttackState>());
        stateDictionary.Add(ECreatureState.Dead, gameObject.AddComponent<DeadState>());

        cachedTransform = GetComponent<Transform>();
        mainCamera = Camera.main;

        CreatureDead += () => Managers.Resource.Instantiate("UI/DeadUI");
    }

    protected override void Update()
    {
        PlayerInput = GetInput();

        mainCamera.transform.position = new Vector3(cachedTransform.position.x, cachedTransform.position.y, -10f);

        base.Update();

        curState?.OnUpdate();
    }

    #endregion Unity Events

    #region Methods

    private EPlayerInput GetInput()
    {
        EPlayerInput input = EPlayerInput.NONE;

        if (Input.GetKey(KeyCode.W))
        {
            input |= EPlayerInput.UP;
        }
        if (Input.GetKey(KeyCode.S))
        {
            input |= EPlayerInput.DOWN;
        }
        if (Input.GetKey(KeyCode.A))
        {
            input |= EPlayerInput.LEFT;
        }
        if (Input.GetKey(KeyCode.D))
        {
            input |= EPlayerInput.RIGHT;
        }
        if (Input.GetKey(KeyCode.Space))
        {
            input |= EPlayerInput.ATTACK;
        }
        if (Input.GetKey(KeyCode.R))
        {
            input |= EPlayerInput.SKILL;
        }

        return input;
    }

    #endregion Methods
}
