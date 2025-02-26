using System;

public class TaskQueue
{
    #region Variables

    private PriorityQueue<TaskBase> priorityQueue = new();

    #endregion Variables

    #region Methods

    public void Push(Action action, int afterTick)
    {
        Task task = new Task(action, Environment.TickCount + afterTick);

        Push(task);
    }

    public void Push<P1>(Action<P1> action, P1 p1, int afterTick)
    {
        Task<P1> task = new Task<P1>(action, p1, Environment.TickCount + afterTick);

        Push(task);
    }

    public void Push<P1, P2>(Action<P1, P2> action, P1 p1, P2 p2, int afterTick)
    {
        Task<P1, P2> task = new Task<P1, P2>(action, p1, p2, Environment.TickCount + afterTick);

        Push(task);
    }

    public void Push<P1, P2, P3>(Action<P1, P2, P3> action, P1 p1, P2 p2, P3 p3, int afterTick)
    {
        Task<P1, P2, P3> task = new Task<P1, P2, P3>(action, p1, p2, p3, Environment.TickCount + afterTick);

        Push(task);
    }

    public void Push<P1, P2, P3, P4>(Action<P1, P2, P3, P4> action, P1 p1, P2 p2, P3 p3, P4 p4, int afterTick)
    {
        Task<P1, P2, P3, P4> task = new Task<P1, P2, P3, P4>(action, p1, p2, p3, p4, Environment.TickCount + afterTick);

        Push(task);
    }

    public void Flush()
    {
        while (priorityQueue.Count > 0)
        {
            TaskBase task = priorityQueue.Top();
            long now = Environment.TickCount;

            if (task.ExecTick > now) break;

            priorityQueue.Pop();

            task?.Execute();
        }
    }

    private void Push(TaskBase task)
    {
        priorityQueue.Push(task);
    }

    #endregion Methods
}
