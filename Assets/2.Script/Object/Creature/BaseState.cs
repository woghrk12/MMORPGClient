using Google.Protobuf.Protocol;
using UnityEngine;

public interface IBaseState<out T> where T : Creature
{
    public ECreatureState StateID { get; }

    public void OnEnter();
    public void OnUpdate();
    public void OnExit();
}

public abstract class BaseState<T> : IBaseState<T> where T : Creature
{
    #region Variables

    protected Animator cachedAnimator = null;
    protected Transform cachedTransform = null;

    protected T controller = null;

    #endregion Variables

    #region Properties

    public abstract ECreatureState StateID { get; }

    #endregion Properties

    #region Constructor

    public BaseState(T controller)
    {
        this.controller = controller;

        cachedAnimator = controller.CachedAnimator;
        cachedTransform = controller.CachedTransform;
    }

    #endregion Constructor

    #region Methods

    public virtual void OnEnter() { }

    public virtual void OnUpdate() { }

    public virtual void OnExit() { }

    #endregion Methods
}
