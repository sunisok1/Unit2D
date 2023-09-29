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

    /// 手牌
    public readonly ListWithEvent<Card> hand = new();

    private Character character;

    //下家
    [NonSerialized] public Unit next;

    public Phase Phase { get; set; }

    public Dictionary<string, object> storage = new();

    private Sprite skin;

    public List<Skill> skillList = new();

    public bool isZhu = false;
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
    //受伤时调用
    public event Action<int> OnBeDamaged;
    //添加技能时调用
    public event Action<Skill> OnAddSkill;
    //设置皮肤时调用
    public event Action<Sprite> OnSetSkin;
    #region SkillEvents
    private readonly UnitEventArgs unitEventArgs = new();

    private event Action<UnitEventArgs> OnPhaseUseBegin;

    private event Action<UnitEventArgs> OnDamageEnd;
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
                unitEventArgs.Reset();
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

        unitEventArgs.player = this;
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
            OnChosen?.Invoke(this, TurnSystem.InActionPlayer.unitEventArgs);
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

    public UnitState State;


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
            unitEventArgs.card = card;
            unitEventArgs.cards.Add(card);
        }
        else
        {
            unitEventArgs.card = null;
            unitEventArgs.cards.Remove(card);
        }
    }

    private void Unit_OnChosen(object sender, EventArgs e)
    {
        Unit unit = sender as Unit;
        UnitEventArgs unitEventArgs = e as UnitEventArgs;
        if (unit.chosen)
        {
            unitEventArgs.targets.Add(unit);
        }
        else
        {
            unitEventArgs.targets.Remove(unit);
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
            filter = (card) => unitEventArgs.cards.Contains(card);
        }
        foreach (var card in hand)
        {
            card.interactable = filter(card);
        }
        bool DefalutFilter(Card card)
        {

            switch (card.name)
            {
                case "sha":
                    if (shaNum >= 1)
                    {
                        return true;
                    }
                    return false;
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
        foreach (Unit unit in unitEventArgs.targets)
        {
            Debug.Log($"{Lib.Translate(name)}对{Lib.Translate(unit.name)}使用了{card.name}");
        }
        hand.Remove(card);
        OnUseOrRespondCard?.Invoke(this, card);
        card.Use(unitEventArgs.targets);
        if (card.name == "sha")
            shaNum--;
        else if (card.name == "jiu")
            jiuNum--;
        unitEventArgs.card = null;
        unitEventArgs.cards.Remove(card);
    }
    //打出
    public void Respond(Card card)
    {
        Debug.Log($"{Lib.Translate(name)}打出了{Lib.Translate(card.name)}");
        hand.Remove(card);
        OnUseOrRespondCard?.Invoke(this, card);
        card.Respond();
        unitEventArgs.card = null;
        unitEventArgs.cards.Remove(card);
    }
    //成为目标
    public void BeTargeted(Card card)
    {
        Debug.Log($"{Lib.Translate(name)}成为{Lib.Translate(card.name)}的目标");
        switch (card)
        {
            case Sha sha:
                StartCoroutine(ChooseToRespond((card) => card.name == "shan", () => Damage(1)));
                break;
            default:
                break;
        }
    }
    //受到伤害
    public void Damage(int amount = 1)
    {
        Debug.Log($"{Lib.Translate(name)}受到{amount}点伤害，当前生命值为{hp}");
        hp -= amount;
        OnBeDamaged?.Invoke(amount);
        OnDamageEnd?.Invoke(unitEventArgs);
    }
    //设置武将
    public void SetCharactor(Character character)
    {
        this.character = character;
        name = character.name;
        sex = character.sex;
        hp = maxHp = character.maxHp;
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
        Debug.Log($"停止了选择卡牌协程{coroutine}");
    }
    private void StopChoosingTarget(ref Coroutine coroutine)
    {
        StopCoroutine(coroutine);
        UnitManager.UpdateUnitInteractable(unitEventArgs, (u) => false);
        coroutine = null;
        Debug.Log($"停止了选择目标协程{coroutine}");
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
                    Respond(unitEventArgs.card);
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
        bool ready_pre = true;
        while (true)
        {
            yield return null;
            bool ready = unitEventArgs.cards.Count.Between(range);
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
        bool ready_pre = true;
        while (true)
        {
            yield return null;
            bool ready = unitEventArgs.targets.Count.Between(range);
            if (ready != ready_pre)
            {
                signal.Ready = ready_pre = ready;
                UnitManager.UpdateUnitInteractable(unitEventArgs, filter, ready);
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

        OnPhaseUseBegin?.Invoke(unitEventArgs);

        Coroutine cardCoroutine = null;
        Coroutine targetCoroutine = null;
        Signal cardSignal = new((val) => CanCancel = val);
        Signal targetSignal = new((val) => CanConfirm = val);
        unitEventArgs.Reset();

        Card card = null;
        bool @using = true;

        cardCoroutine = StartCoroutine(ChooseCards(null, (1, 1), cardSignal));
        while (@using)
        {
            yield return new WaitUntil(() => TurnSystem.InActionPlayer == this);
            switch (cardSignal.Ready)
            {
                case true when targetCoroutine == null:
                    card = unitEventArgs.cards[0];
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
                    unitEventArgs.Reset();
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


