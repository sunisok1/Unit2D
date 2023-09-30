using System;
using System.Collections;
using System.Collections.Generic;
using UI;

public abstract class Skill
{
    public SkillType type;
    public Unit owner;
    public string name;
    public Action<Args> content = null;
    public Skill[] CompanionSkills;
    public bool silent = false;
}
/// <summary>
/// 触发技
/// </summary>
public class TriggerSkill : Skill
{
    public (Triggerer, Timing) trigger;
    public float priority;
    public Func<Args, Unit, bool> check;
    public bool frequent = false;
    public bool forced = false;
    public bool nopop = false;
    public bool direct = false;
    public Func<Args, Unit, bool> filter;

}
/// <summary>
/// 主动技
/// </summary>
public class ActiveSkill : Skill
{
    public Timing enable;
    public int usable = 1;
    public int round = 1;
    public Func<Card, bool> filterCard = null;
    public string position = "";
    public (int, int) selectCard;
    public Func<Card, bool> check = null;
    public bool discard = true;
    public string prepare = "";
    public Func<Card, Unit, Unit, bool> filterTarget = null;
    public int selectTarget;
    public bool multitarget = true;
    public string[] targetprompt;
}
/// <summary>
/// 视为技
/// </summary>
public class ViewAsSkill : Skill
{

}

public class Args : EventArgs
{
    public Unit player;
    public readonly List<Card> cards = new();
    public readonly List<Unit> targets = new();
    public Card card;
    public Unit target;
    public void Reset()
    {
        List<Card> cardList = new(cards);
        foreach (Card card in cardList)
        {
            card.Chosen = false;
        }
        List<Unit> targetList = new(targets);
        foreach (Unit target in targetList)
        {
            target.Chosen = false;
        }
        cards.Clear();
        targets.Clear();
        card = null;
        target = null;
    }
}