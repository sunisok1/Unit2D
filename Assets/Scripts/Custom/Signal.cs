using System;

public class Signal
{
    private event Action<bool> OnReady;
    private bool ready = false;

    public bool Ready
    {
        get => ready;
        set
        {
            ready = value;
            OnReady?.Invoke(value);
        }
    }

    public Signal(params Action<bool>[] actions)
    {
        foreach (Action<bool> action in actions)
        {
            OnReady += action;
        }
    }
}