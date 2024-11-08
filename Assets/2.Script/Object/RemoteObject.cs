using Google.Protobuf.Protocol;
using MMORPG;
using UnityEngine;
using System.Collections.Generic;

public class RemoteObject : MMORPG.Object
{
    #region Variables

    private Dictionary<ECreatureState, RemoteObjectState.State> stateDictionary = new();
    private RemoteObjectState.State curState = null;

    #endregion Variables

    #region Unity Events

    protected override void Awake()
    {
        base.Awake();

        RemoteObjectState.State[] states = GetComponents<RemoteObjectState.State>();

        foreach (RemoteObjectState.State state in states)
        {
            stateDictionary.Add(state.StateID, state);
        }
    }

    private void Update()
    {
        curState?.OnUpdate();
    }

    #endregion Unity Events

    #region Methods

    public void SetState(ECreatureState value)
    {
        if (ReferenceEquals(curState, null) == false)
        {
            if (curState.StateID == value) return;

            curState.OnExit();
        }

        if (stateDictionary.TryGetValue(value, out RemoteObjectState.State state) == false) return;

        curState = state;
        curState.OnEnter();
    }

    public ECreatureState GetState()
    {
        return ReferenceEquals(curState, null) == false ? curState.StateID : ECreatureState.Idle;
    }

    public void PerformAttack(int attackID)
    {
        if (stateDictionary.TryGetValue(ECreatureState.Attack, out RemoteObjectState.State state) == false) return;

        RemoteObjectState.AttackState attackState = state as RemoteObjectState.AttackState;
        attackState.SetAttackType(attackID);

        SetState(ECreatureState.Attack);
    }

    #region Events

    public override void OnDead(MMORPG.Object attacker)
    {
        base.OnDead(attacker);

        SetState(ECreatureState.Dead);
    }

    public override void OnRevive(Vector3Int revivePos)
    {
        base.OnRevive(revivePos);

        SetState(ECreatureState.Idle);
    }

    #endregion Events

    #endregion Methods
}
