using System;
using System.Collections.Generic;

public abstract class Card
{
    public readonly int point;

    public readonly Suit suit;

    public readonly CardColor color;

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

    public virtual void Use(IEnumerable<Unit> targets) { }

    public virtual void Respond() { }

    public Func<Unit, bool> targetFilter;

    protected Card(int point, Suit suit)
    {
        this.point = point;
        this.suit = suit;
        color = suit switch
        {
            Suit.spade => CardColor.balck,
            Suit.club => CardColor.balck,
            Suit.heart => CardColor.red,
            Suit.diamond => CardColor.red,
            _ => throw new NotImplementedException(),
        };
        Reset();
    }

    public abstract void Reset();
}

public class Sha : Card
{
    public int damage = 1;
    public Sha(int point, Suit suit) : base(point, suit)
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
        foreach (var unit in targets)
        {
            unit.BeTargeted(this);
        }
    }
    public override void Reset()
    {
        damage = 1;
        targetNum = (1, 1);
    }
}

public class LeiSha : Sha
{
    public LeiSha(int point, Suit suit) : base(point, suit)
    {
    }
}
public class HuoSha : Sha
{
    public HuoSha(int point, Suit suit) : base(point, suit)
    {
    }
}

public class Shan : Card
{
    public Shan(int point, Suit suit) : base(point, suit)
    {
        name = "shan";
    }

    public override void Reset()
    {
    }
}

public class Tao : Card
{
    public Tao(int point, Suit suit) : base(point, suit)
    {
        name = "tao";
    }
    public override void Use(IEnumerable<Unit> targets)
    {
        owner.Recover();
    }

    public override void Reset()
    {
        targetNum = (0, 0);
    }
}

public class Jiu : Card
{
    public Jiu(int point, Suit suit) : base(point, suit)
    {
        name = "jiu";
        targetNum = (0, 0);
    }
    public override void Use(IEnumerable<Unit> targets)
    {
        base.Use(targets);
        owner.Drunk += 1;
    }

    public override void Reset()
    {
        targetNum = (0, 0);
    }
}