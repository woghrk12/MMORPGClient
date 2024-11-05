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

public class LocalCharacter : MMORPG.Object
{
    #region Variables

    private Dictionary<ECreatureState, LocalPlayerState.State> stateDictionary = new();
    private LocalPlayerState.State curState = null;

    private Camera mainCamera = null;

    #endregion Variables

    #region Unity Events

    protected override void Awake()
    {
        base.Awake();

        LocalPlayerState.State[] states = GetComponents<LocalPlayerState.State>();

        foreach (LocalPlayerState.State state in states)
        {
            stateDictionary.Add(state.StateID, state);
        }

        mainCamera = Camera.main;
    }

    private void Update()
    {
        curState?.OnUpdate(GetInput());
    }

    private void LateUpdate()
    {
        mainCamera.transform.position = new Vector3(transform.position.x, transform.position.y, -10f);
    }

    #endregion Unity Events

    #region Methods

    public void SetState(ECreatureState value, EPlayerInput input = EPlayerInput.NONE)
    {
        if (ReferenceEquals(curState, null) == false)
        {
            if (curState.StateID == value) return;

            curState.OnExit(input);
        }

        if (stateDictionary.TryGetValue(value, out LocalPlayerState.State state) == false) return;

        curState = state;
        curState.OnEnter(input);
    }

    public ECreatureState GetState()
    {
        return ReferenceEquals(curState, null) == false ? curState.StateID : ECreatureState.Idle;
    }

    public void PerformAttack(int attackID)
    {
        if (stateDictionary.TryGetValue(ECreatureState.Attack, out LocalPlayerState.State state) == false) return;

        LocalPlayerState.AttackState attackState = state as LocalPlayerState.AttackState;
        attackState.SetAttackType(attackID);

        SetState(ECreatureState.Attack, EPlayerInput.NONE);
    }

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

    #region Events

    public override void OnDead(MMORPG.Object attacker)
    {
        base.OnDead(attacker);

        SetState(ECreatureState.Dead);

        Managers.Resource.Instantiate("UI/DeadUI");
    }

    public override void OnRevive(Vector3Int revivePos)
    {
        base.OnRevive(revivePos);

        SetState(ECreatureState.Idle);
    }

    #endregion Events

    #endregion Methods
}
