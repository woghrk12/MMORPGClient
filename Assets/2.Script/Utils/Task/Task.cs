using System;

public abstract class TaskBase : IComparable<TaskBase>
{
    #region Properties

    public int ExecTick { protected set; get; }

    #endregion Properties

    #region Methods

    public abstract void Execute();

    public int CompareTo(TaskBase other)
    {
        long diff = other.ExecTick - ExecTick;

        return diff != 0 ? (diff > 0 ? 1 : -1) : 0;
    }

    #endregion Methods
}

public class Task : TaskBase
{
    #region Variables

    private Action action = null;

    #endregion Variables

    #region Constructor

    public Task(Action action, int execTick = 0)
    {
        this.action = action;

        ExecTick = execTick;
    }

    #endregion Constructor

    #region Methods

    public override void Execute()
    {
        action?.Invoke();
    }

    #endregion Methods
}

public class Task<P1> : TaskBase
{
    #region Variables

    private Action<P1> action = null;
    private P1 p1 = default;

    #endregion Variables

    #region Constructor

    public Task(Action<P1> action, P1 p1, int execTick = 0)
    {
        this.action = action;
        this.p1 = p1;

        ExecTick = execTick;
    }

    #endregion Constructor

    #region Methods

    public override void Execute()
    {
        action?.Invoke(p1);
    }

    #endregion Methods
}

public class Task<P1, P2> : TaskBase
{
    #region Variables

    private Action<P1, P2> action = null;
    private P1 p1 = default;
    private P2 p2 = default;

    #endregion Variables

    #region Constructor

    public Task(Action<P1, P2> action, P1 p1, P2 p2, int execTick = 0)
    {
        this.action = action;
        this.p1 = p1;
        this.p2 = p2;

        ExecTick = execTick;
    }

    #endregion Constructor

    #region Methods

    public override void Execute()
    {
        action?.Invoke(p1, p2);
    }

    #endregion Methods
}

public class Task<P1, P2, P3> : TaskBase
{
    #region Variables

    private Action<P1, P2, P3> action = null;
    private P1 p1 = default;
    private P2 p2 = default;
    private P3 p3 = default;

    #endregion Variables

    #region Constructor

    public Task(Action<P1, P2, P3> action, P1 p1, P2 p2, P3 p3, int execTick = 0)
    {
        this.action = action;
        this.p1 = p1;
        this.p2 = p2;
        this.p3 = p3;

        ExecTick = execTick;
    }

    #endregion Constructor

    #region Methods

    public override void Execute()
    {
        action?.Invoke(p1, p2, p3);
    }

    #endregion Methods
}

public class Task<P1, P2, P3, P4> : TaskBase
{
    #region Variables

    private Action<P1, P2, P3, P4> action = null;
    private P1 p1 = default;
    private P2 p2 = default;
    private P3 p3 = default;
    private P4 p4 = default;

    #endregion Variables

    #region Constructor

    public Task(Action<P1, P2, P3, P4> action, P1 p1, P2 p2, P3 p3, P4 p4, int execTick = 0)
    {
        this.action = action;
        this.p1 = p1;
        this.p2 = p2;
        this.p3 = p3;
        this.p4 = p4;

        ExecTick = execTick;
    }

    #endregion Constructor

    #region Methods

    public override void Execute()
    {
        action?.Invoke(p1, p2, p3, p4);
    }

    #endregion Methods
}