using System;
using System.Collections.Generic;

public abstract class Card
{
    public Unit owner;

    public string name;

    public (int, int) targetNum;


    public event EventHandler<bool> OnSelected;

    public bool interactable;

    private bool chosen;

    public bool Chosen
    {
        get => chosen;
        set
        {
            if (chosen == value) return;
            chosen = value;
            OnSelected?.Invoke(this, value);
        }
    }

    public virtual void Use(IEnumerable<Unit> targets)
    {
    }

    public virtual void Respond()
    {
    }

    public Func<Unit, bool> targetFilter;
}

public class Sha : Card
{
    public Sha()
    {
        name = "sha";
        targetFilter = (unit) =>
        {
            return unit != owner;
        };
        targetNum = (1, 1);
    }

    public override void Use(IEnumerable<Unit> targets)
    {
        base.Use(targets);
        foreach (var unit in targets)
        {
            unit.BeTargeted(this);
        }
    }
}
public class Shan : Card
{
    public Shan()
    {
        name = "shan";
    }
}