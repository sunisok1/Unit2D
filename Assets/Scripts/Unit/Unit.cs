using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class Unit : MonoBehaviour
{
    #region Fileds

    public bool isEnemy;

    public Sex sex;

    private int maxHp = 4;

    private int hp = 4;

    private int shaNum = 1;

    private int jiuNum = 1;

    private Vector2Int gridPositon;

    public UnitState State;
    /// 手牌
    public readonly ListWithEvent<Card> hand = new();

    private Character character;
    //上家
    public Unit pre;
    //下家
    public Unit next;

    public Phase Phase { get; set; }

    public Dictionary<string, object> storage = new();

    private Sprite skin;

    public List<Skill> skillList = new();

    public bool isZhu = false;

    private int drunk = 0;
    #endregion

    #region Events
    // 更改被选择状态时调用
    public event EventHandler OnChosen;
    //更改可选择性时调用
    public event Action<bool> OnInteractable;
    //可确定时调用
    public event Action<bool> OnCanConfirm;
    //可取消时调用
    public event Action<bool> OnCanCancel;
    //使用时调用
    public event EventHandler<Card> OnUseOrRespondCard;
    //血量改变时调用
    public event Action<int> OnHpChange;
    //血上限改变时调用
    public event Action<int> OnMaxHpChange;
    //受伤时调用
    public event Action<int> OnDamage;
    //回血时调用
    public event Action OnRecover;
    //死亡时调用
    public event EventHandler OnDead;
    //添加技能时调用
    public event Action<Skill> OnAddSkill;
    //设置皮肤时调用
    public event Action<Sprite> OnSetSkin;
    //喝酒时调用
    public event Action<int> OnDrunk;
    #region SkillEvents
    private readonly Args args = new();

    private event Action<Args> OnPhaseUseBegin;

    private event Action<Args> OnDamageEnd;
    #endregion
    #endregion

    #region UserApi
    public void Confirm()
    {
        State = UnitState.Confirmed;
    }
    public void Cancel()
    {
        State = UnitState.Canceled;
    }

    public void EndUse()
    {
        State = UnitState.EndUse;
        foreach (Card card in hand)
        {
            card.interactable = false;
        }
    }

    public void UseSkill(Skill skill)
    {
        switch (skill)
        {
            case ActiveSkill activeSkill:
                args.Reset();
                StartCoroutine(SkillProcess(activeSkill));
                break;
            default:
                break;
        }
    }
    #endregion
    private void Awake()
    {
        gridPositon = GridSystem.GetGridPositon(transform.position);
        GridSystem.AddUnitAtGridPosition(gridPositon.x, gridPositon.y, this);

        hand.OnAdd += Unit_OnAddCard;
        hand.OnRemove += Unit_OnRemoveCard;

        OnChosen += Unit_OnChosen;

        args.player = this;
    }



    #region Attribute

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

    private bool chosen;
    public bool Chosen
    {
        get => chosen;
        set
        {
            if (chosen == value) return;
            chosen = value;
            OnChosen?.Invoke(this, TurnSystem.InActionPlayer.args);
        }
    }

    private bool canConfirm;
    public bool CanConfirm
    {
        get => canConfirm;
        set
        {
            OnCanConfirm?.Invoke(value);
            canConfirm = value;
            State = UnitState.Waiting;
        }
    }
    private bool canCancel;

    public bool CanCancel
    {
        get => canCancel;
        private set
        {
            OnCanCancel?.Invoke(value);
            canCancel = value;
        }
    }

    public Sprite Skin
    {
        get => skin;
        set
        {
            OnSetSkin?.Invoke(value);
            skin = value;
        }
    }

    public int MaxHp
    {
        get => maxHp;
        set
        {
            maxHp = value;
            OnMaxHpChange?.Invoke(value);
        }
    }
    public int Hp
    {
        get => hp;
        set
        {
            hp = value;
            OnHpChange?.Invoke(value);
        }
    }



    #endregion

    #region EventsFunc

    private void Unit_OnAddCard(Card card)
    {
        card.owner = this;
        card.OnSelected += Card_OnSelected;
    }

    private void Unit_OnRemoveCard(Card card)
    {
        card.OnSelected -= Card_OnSelected;
    }

    private void Card_OnSelected(object obj, bool value)
    {
        Card card = obj as Card;
        if (value)
        {
            args.card = card;
            args.cards.Add(card);
        }
        else
        {
            args.card = null;
            args.cards.Remove(card);
        }
    }

    private void Unit_OnChosen(object sender, EventArgs e)
    {
        Unit unit = sender as Unit;
        Args args = e as Args;
        if (unit.chosen)
        {
            args.targets.Add(unit);
        }
        else
        {
            args.targets.Remove(unit);
        }
    }

    #endregion

    #region Method

    /// <summary>
    /// 更新手牌可交互性
    /// </summary>
    /// <param name="filter"></param>
    public void UpdateCardInteractable(Func<Card, bool> filter = null, bool ready = false)
    {
        filter ??= DefalutFilter;
        if (ready)
        {
            filter = (card) => args.cards.Contains(card);
        }
        foreach (var card in hand)
        {
            card.interactable = filter(card);
        }
        bool DefalutFilter(Card card)
        {

            switch (card)
            {
                case Sha:
                    return shaNum >= 1;
                case Tao:
                    return Injured;
                case Jiu:
                    return jiuNum >= 1;
                default:
                    return false;
            }
        }

    }
    #endregion

    #region GameLogics
    //开启回合
    public void StartTurn()
    {
        StartCoroutine(TurnProcess());
    }
    //是否受伤
    public bool Injured => hp < MaxHp;

    public int Drunk
    {
        get => drunk;
        set
        {
            drunk = value;
            OnDrunk?.Invoke(value);
        }
    }

    //摸牌
    public void Draw(int amount = 1)
    {
        for (int i = 0; i < amount; i++)
        {
            Card card = Deck.GetOne();
            hand.Add(card);
        }
    }
    //使用
    public void Use(Card card)
    {
        foreach (Unit unit in args.targets)
        {
            Debug.Log($"{Lib.Translate(name)}对{Lib.Translate(unit.name)}使用了{card.name}");
        }
        hand.Remove(card);
        OnUseOrRespondCard?.Invoke(this, card);
        card.Use(args);
        if (card.name == "sha")
            shaNum--;
        else if (card.name == "jiu")
            jiuNum--;
        args.card = null;
        args.cards.Remove(card);
    }
    //打出
    public void Respond(Card card)
    {
        Debug.Log($"{Lib.Translate(name)}打出了{Lib.Translate(card.name)}");
        hand.Remove(card);
        OnUseOrRespondCard?.Invoke(this, card);
        card.Respond();
        args.card = null;
        args.cards.Remove(card);
    }
    //成为目标
    public void BeTargeted(Args args, Card card)
    {
        Debug.Log($"{Lib.Translate(name)}成为{Lib.Translate(card.name)}的目标");
        switch (card)
        {
            case Sha sha:
                StartCoroutine(ChooseToRespond((card) => card.name == "shan", () => args.player.DamageSource(this, 1 + args.player.Drunk)));
                break;
            default:
                break;
        }
    }
    //造成伤害
    public void DamageSource(Unit target, int amount = 1)
    {
        target.Damage(amount);
    }
    //受到伤害
    public void Damage(int amount = 1)
    {
        Hp -= amount;
        Debug.Log($"{Lib.Translate(name)}受到{amount}点伤害，当前生命值为{Hp}");
        OnDamage?.Invoke(amount);

        if (Hp <= 0)
        {
            Die();
        }

        OnDamageEnd?.Invoke(args);
    }
    public void Recover(int amount = 1)
    {
        if (hp == maxHp) { return; }
        if (hp + amount <= MaxHp)
        {
            Hp += amount;
        }
        else
        {
            Hp = MaxHp;
        }
        OnRecover?.Invoke();
    }
    //死亡
    public void Die()
    {
        next.pre = pre;
        pre.next = next;
        OnDead?.Invoke(this, null);
        Destroy(gameObject, 5);
    }
    //设置武将
    public void SetCharactor(Character character)
    {
        this.character = character;
        name = character.name;
        sex = character.sex;
        Hp = MaxHp = character.maxHp;
        foreach (Skill skill in character.skills)
        {
            AddSkill(skill);
        }
        Skin = Resources.Load<Sprite>($"Image/skin/{name}/00");

    }
    //添加技能
    public void AddSkill(Skill skill)
    {
        if (skill.type == SkillType.zhu && isZhu == false) return;
        OnAddSkill?.Invoke(skill);
        skillList.Add(skill);
        if (skill.CompanionSkills != null)
        {
            foreach (Skill companionSkill in skill.CompanionSkills)
            {
                AddSkill(companionSkill);
            }
        }
        switch (skill)
        {
            case TriggerSkill triggerSkill:
                switch (triggerSkill.trigger)
                {
                    case (Triggerer.player, Timing.phaseUseBegin):
                        OnPhaseUseBegin += skill.content;
                        break;
                    case (Triggerer.player, Timing.damageEnd):
                        OnDamageEnd += skill.content;
                        break;
                    default:
                        throw new NotImplementedException("没有对应技能触发事件");
                }
                break;
            case ActiveSkill activeSkill:
                switch (activeSkill.enable)
                {
                    case Timing.phaseUse:
                        break;
                    default:
                        throw new NotImplementedException("没有对应技能开启时机");
                }
                break;
            case ViewAsSkill viewAsSkill:
                break;
            default:
                throw new NotImplementedException("技能类型异常");
        }
        //Debug.Log($"{Lib.Translate(name)}添加了技能{Lib.Translate(skill.name)}");
    }
    //交给目标角色手牌
    public void Give(IEnumerable<Card> cards, Unit target)
    {
        foreach (Card card in cards)
        {
            hand.Remove(card);
        }
        target.Gain(cards);
    }
    //获得手牌
    public void Gain(IEnumerable<Card> cards)
    {
        hand.AddRange(cards);
    }


    #endregion

    #region Operations
    private void StopChoosingCard(ref Coroutine coroutine)
    {
        StopCoroutine(coroutine);
        UpdateCardInteractable((c) => false);
        coroutine = null;
        //Debug.Log($"停止了选择卡牌协程{coroutine}");
    }
    private void StopChoosingTarget(ref Coroutine coroutine)
    {
        StopCoroutine(coroutine);
        UnitManager.UpdateUnitInteractable(args, (u) => false);
        coroutine = null;
        //Debug.Log($"停止了选择目标协程{coroutine}");
    }

    private Unit CheckCurrentUnit(Unit unit = null)
    {
        if (unit == null)
        {
            unit = this;
        }
        if (TurnSystem.InActionPlayer != unit)
        {
            Unit pre = TurnSystem.InActionPlayer;
            TurnSystem.InActionPlayer = unit;
            return pre;
        }
        return null;
    }

    public IEnumerator ChooseToRespond(Func<Card, bool> filter, Action fail = null)
    {
        Unit pre = CheckCurrentUnit();
        Coroutine cardCoroutine = null;
        Signal signal = new((val) => CanConfirm = val);
        CanCancel = true;
        while (true)
        {
            yield return null;
            cardCoroutine ??= StartCoroutine(ChooseCards(filter, (1, 1), signal));

            switch (State)
            {
                case UnitState.Waiting:
                    break;
                case UnitState.Confirmed:
                    Respond(args.card);
                    goto default;
                case UnitState.Canceled:
                    fail?.Invoke();
                    goto default;
                default:
                    CheckCurrentUnit(pre);
                    State = UnitState.Waiting;
                    StopChoosingCard(ref cardCoroutine);
                    yield break;
            }
        }
    }


    public IEnumerator ChooseCards(Func<Card, bool> filter, (int, int) range, Signal signal)
    {
        CheckCurrentUnit();
        bool ready_pre = false;
        UpdateCardInteractable(filter);
        while (true)
        {
            yield return null;
            bool ready = args.cards.Count.Between(range);
            if (ready != ready_pre)
            {
                signal.Ready = ready_pre = ready;
                UpdateCardInteractable(filter, ready);
            }
        }
    }

    public IEnumerator ChooseTargets(Func<Unit, bool> filter, (int, int) range, Signal signal)
    {
        CheckCurrentUnit();
        bool ready_pre = false;
        UnitManager.UpdateUnitInteractable(args, filter);
        while (true)
        {
            yield return null;
            bool ready = args.targets.Count.Between(range);
            if (ready != ready_pre)
            {
                signal.Ready = ready_pre = ready;
                UnitManager.UpdateUnitInteractable(args, filter, ready);
            }
        }
    }

    private IEnumerator TurnProcess()
    {
        TurnSystem.SetInTurnPlayer(this);
        //step 0:
        shaNum = 1;
        jiuNum = 1;
        //step 1:
        Phase = Phase.begin;

        //step 2:
        Phase = Phase.draw;
        Draw(2);

        //step 3:
        Phase = Phase.use;

        OnPhaseUseBegin?.Invoke(args);

        Coroutine cardCoroutine = null;
        Coroutine targetCoroutine = null;
        Signal cardSignal = new((val) => CanCancel = val);
        Signal targetSignal = new((val) => CanConfirm = val);
        args.Reset();

        Card card = null;
        bool @using = true;

        cardCoroutine = StartCoroutine(ChooseCards(null, (1, 1), cardSignal));
        while (@using)
        {
            yield return new WaitUntil(() => TurnSystem.InActionPlayer == this);
            switch (cardSignal.Ready)
            {
                case true when targetCoroutine == null:
                    card = args.cards[0];
                    targetCoroutine = StartCoroutine(ChooseTargets(card.targetFilter, card.targetNum, targetSignal));
                    break;
                case false when targetCoroutine != null:
                    StopChoosingTarget(ref targetCoroutine);
                    break;
            }
            switch (State)
            {
                case UnitState.Waiting:
                    break;
                case UnitState.Confirmed:
                    Use(card);
                    CanConfirm = false;
                    goto default;
                case UnitState.Canceled:
                    goto default;
                case UnitState.EndUse:
                    @using = false;
                    StopChoosingCard(ref cardCoroutine);
                    goto default;
                default:
                    State = UnitState.Waiting;
                    args.Reset();
                    cardSignal.Ready = false;
                    if (targetCoroutine != null)
                    {
                        StopChoosingTarget(ref targetCoroutine); ;
                        targetSignal.Ready = false;
                    }
                    break;
            }
        }
        //step 4:
        Phase = Phase.end;

        yield return null;
        TurnSystem.NextTurn();
    }
    public IEnumerator SkillProcess(ActiveSkill skill)
    {
        //while (true)
        //{
        yield return null;
        //    coroutine ??= StartCoroutine(ChooseCard(skill.filterCard, skill.selectCard, signal));
        //}
    }
    #endregion
}


