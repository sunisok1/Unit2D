using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEditor.AnimatedValues;
using UnityEngine;

public abstract class Card
{
    public Unit owner;

    public string Name { get; protected set; }

    public int targetNum;


    public event EventHandler<bool> OnSelected;
    public event Action<bool> OnInteractable;
    public event Action OnGoToDiscardPile;

    private bool interactable;
    public bool Interactable
    {
        get => interactable;
        set
        {
            if (interactable == value) return;
            interactable = value;
            OnInteractable?.Invoke(value);
        }
    }


    private bool selected;

    public bool Selected
    {
        get => selected;
        set
        {
            if (!interactable) return;
            if (selected == value) return;
            selected = value;
            OnSelected?.Invoke(this, value);
        }
    }

    public virtual void Use(IEnumerable<Unit> targets)
    {
        OnGoToDiscardPile?.Invoke();
    }


    public Func<Unit, bool> targetFilter;
}

public class Sha : Card
{
    public Sha()
    {
        Name = "sha";
        targetFilter = (unit) =>
        {
            return unit != owner;
        };
        targetNum = 1;
    }

    public override void Use(IEnumerable<Unit> targets)
    {
        base.Use(targets);
        foreach (var item in targets)
        {
            Debug.Log(item.name);
        }
    }
}
public class Shan : Card
{
    public Shan()
    {
        Name = "shan";
    }
}