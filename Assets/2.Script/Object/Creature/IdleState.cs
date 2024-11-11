using Google.Protobuf.Protocol;

namespace CreatureState
{
    public class IdleState : BaseState<Creature>
    {
        #region Properties

        public sealed override ECreatureState StateID => ECreatureState.Idle;

        #endregion Properties
    }
}