namespace CreatureState
{
    public abstract class BaseState<T> : State where T : Creature
    {
        #region Variables

        protected T controller = null;

        #endregion Variables

        #region Unity Events

        protected override void Awake()
        {
            base.Awake();

            controller = GetComponent<T>();
        }

        #endregion Unity Events
    }
}
