namespace Creature
{
    public class AttackState<T> : CreatureState where T : CreatureController
    {
        #region Variables

        protected T controller = null;

        #endregion Variables

        #region Properties

        public sealed override ECreatureState StateID => ECreatureState.ATTACK;

        #endregion Properties

        #region Unity Events

        protected override void Awake()
        {
            base.Awake();

            controller = GetComponent<T>();
        }

        #endregion Unity Events
    }
}