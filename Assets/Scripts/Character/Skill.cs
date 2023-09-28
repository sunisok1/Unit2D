using System;
using System.Collections;
using System.Collections.Generic;

public abstract class Skill
{
    public string name;
    public Action<UnitEventArgs> content = null;
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
    public Func<UnitEventArgs, Unit, bool> check;
    public bool frequent = false;
    public bool forced = false;
    public bool nopop = false;
    public bool direct = false;
    public Func<UnitEventArgs, Unit, bool> filter;

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

public class UnitEventArgs : System.EventArgs
{
    public Unit player;
    public List<Card> cards = new();
    public Unit target;
}