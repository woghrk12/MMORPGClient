using System;
using System.Collections.Generic;

public class PriorityQueue<T> where T : IComparable<T>
{
    #region Variables

    private List<T> heap = new();

    #endregion Variables

    #region Properties

    public int Count => heap.Count;

    #endregion Properties

    #region Methods

    public void Push(T data)
    {
        heap.Add(data);

        int index = heap.Count - 1;
        while (index > 0)
        {
            int parent = (index - 1) / 2;

            if (heap[index].CompareTo(heap[parent]) < 0) break;

            T temp = heap[index];
            heap[index] = heap[parent];
            heap[parent] = temp;

            index = parent;
        }
    }

    public T Pop()
    {
        T result = heap[0];

        int lastIndex = heap.Count - 1;
        heap[0] = heap[lastIndex];
        heap.RemoveAt(lastIndex);
        lastIndex--;

        int index = 0;
        while (true)
        {
            int left = 2 * index + 1;
            int right = 2 * index + 2;

            int child = index;
            if (left <= lastIndex && heap[child].CompareTo(heap[left]) < 0)
            {
                child = left;
            }
            if (right <= lastIndex && heap[child].CompareTo(heap[right]) < 0)
            {
                child = right;
            }

            if (child == index) break;

            T temp = heap[index];
            heap[index] = heap[child];
            heap[child] = temp;

            index = child;
        }

        return result;
    }

    public T Top() => heap[0];

    #endregion Methods
}
