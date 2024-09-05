using Google.Protobuf.Protocol;

namespace LocalPlayerState
{
    public class IdleState : State
    {
        #region Properties

        public sealed override ECreatureState StateID => ECreatureState.Idle;

        #endregion Properties
    }
}