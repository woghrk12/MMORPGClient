using Google.Protobuf.Protocol;
using System.Collections.Generic;

public class RemoteCreature : Creature
{
    #region Variables

    private Dictionary<ECreatureState, RemoteCreatureState.State> stateDictionary = new();
    private RemoteCreatureState.State curState = null;

    #endregion Variables

    #region Unity Events

    private void Awake()
    {
        RemoteCreatureState.State[] states = GetComponents<RemoteCreatureState.State>();

        foreach (RemoteCreatureState.State state in states)
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

        if (stateDictionary.TryGetValue(value, out RemoteCreatureState.State state) == false) return;

        curState = state;
        curState.OnEnter();
    }

    public ECreatureState GetState()
    {
        return ReferenceEquals(curState, null) == false ? curState.StateID : ECreatureState.Idle;
    }

    #endregion Methods
}
