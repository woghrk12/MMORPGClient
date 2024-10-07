using Google.Protobuf.Protocol;
using System.Collections.Generic;

public class RemoteObject : MMORPG.Object
{
    #region Variables

    private Dictionary<EObjectState, RemoteObjectState.State> stateDictionary = new();
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

    public void SetState(EObjectState value)
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

    public EObjectState GetState()
    {
        return ReferenceEquals(curState, null) == false ? curState.StateID : EObjectState.Idle;
    }

    public void PerformAttack(int attackID)
    {
        if (stateDictionary.TryGetValue(EObjectState.Attack, out RemoteObjectState.State state) == false) return;

        RemoteObjectState.AttackState attackState = state as RemoteObjectState.AttackState;
        attackState.SetAttackType(attackID);

        SetState(EObjectState.Attack);
    }

    #endregion Methods
}
