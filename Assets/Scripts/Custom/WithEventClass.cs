using System;
using System.Collections.Generic;
using UnityEngine.UIElements;

public class ListWithEvent<T> : List<T>
{
    public event Action<T> OnAdd;
    public event Action<T> OnRemove;
    public new void Add(T item)
    {
        base.Add(item);
        OnAdd?.Invoke(item);
    }
    public new void Remove(T item)
    {
        base.Remove(item);
        OnRemove?.Invoke(item);
    }
}

public class StackWithEvent<T> : Stack<T>
{
    public event Action<T> OnPush;
    public event Action OnPop;

    public new void Push(T item)
    {
        base.Push(item);
        OnPush?.Invoke(item);
    }

    public new T Pop()
    {
        OnPop?.Invoke();
        return base.Pop();
    }
}