using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;

public abstract class Card
{
    public int point;

    public Suit suit;

    public CardColor color;

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

    public virtual void Use(Args args)
    {
    }

    public virtual void Respond()
    {
    }

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
    }
}

public class Sha : Card
{
    public Sha(int point, Suit suit) : base(point, suit)
    {
        name = "sha";
        targetFilter = (unit) =>
        {
            return unit != owner;
        };
        targetNum = (1, 1);
    }

    public override void Use(Args args)
    {
        base.Use(args);
        foreach (var unit in args.targets)
        {
            unit.BeTargeted(args, this);
        }
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
}

public class Tao : Card
{
    public Tao(int point, Suit suit) : base(point, suit)
    {
        name = "tao";
        targetNum = (0, 0);
    }
    public override void Use(Args args)
    {
        base.Use(args);

        args.target ??= args.player;
        args.target.Recover();
    }
}

public class Jiu : Card
{
    public Jiu(int point, Suit suit) : base(point, suit)
    {
        name = "jiu";
        targetNum = (0, 0);
    }
    public override void Use(Args args)
    {
        base.Use(args);
        args.player.Drunk += 1;
    }
}