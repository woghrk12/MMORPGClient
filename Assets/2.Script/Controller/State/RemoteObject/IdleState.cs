using Google.Protobuf.Protocol;

namespace RemoteObjectState
{
    public class IdleState : State
    {
        #region Properties

        public sealed override EObjectState StateID => EObjectState.Idle;

        #endregion Properties
    }
}

