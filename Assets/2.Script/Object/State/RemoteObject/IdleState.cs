using Google.Protobuf.Protocol;

namespace RemoteObjectState
{
    public class IdleState : State
    {
        #region Properties

        public sealed override ECreatureState StateID => ECreatureState.Idle;

        #endregion Properties
    }
}

