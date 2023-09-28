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

    private int shaNum = 2;

    private int jiuNum = 1;

    private Vector2Int gridPositon;


    /// <summary>
    /// 手牌
    /// </summary>
    public readonly ListWithEvent<Card> hand = new();

    private Character character;

    //下家
    [NonSerialized] public Unit next;
    //选择的牌
    private Card SelectedCard;
    //选择的单位
    private readonly List<Unit> targets = new();
    public Phase Phase { get; set; }

    public Dictionary<string, object> storage = new();

    private Sprite skin;
    #endregion

    #region Events
    // 更改被选择状态时调用
    public event Action<bool> OnChosen;
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
    private event Action<UnitEventArgs> OnPhaseUseBegin;
    private event Action<UnitEventArgs> OnDamageEnd;
    private UnitEventArgs evnetArgs = new();
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
        SelectedCard = null;
        StopSelectingTarget();
        foreach (Card card in hand)
        {
            card.interactable = false;
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
        evnetArgs.player = this;
    }

    public void Leave()
    {
        StopSelectingTarget();
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
            OnChosen?.Invoke(value);
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
            SelectedCard = card;
        }
        else
        {
            SelectedCard = null;
        }
    }

    private void Unit_OnChosen(bool value)
    {
        if (value)
        {
            TurnSystem.InActionPlayer.targets.Add(this);
        }
        else
        {
            TurnSystem.InActionPlayer.targets.Remove(this);
        }
    }

    #endregion

    #region Method

    /// <summary>
    /// 更新手牌可交互性
    /// </summary>
    /// <param name="filter"></param>
    public void UpdateCardInteractable(Func<Card, bool> filter = null)
    {
        filter ??= DefalutFilter;
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
        StartCoroutine(startTurn());
        IEnumerator startTurn()
        {
            TurnSystem.SetInTurnPlayer(this);
            //step 0:

            //step 1:
            Phase = Phase.begin;

            //step 2:
            Phase = Phase.draw;
            Draw(2);

            //step 3:
            Phase = Phase.use;
            OnPhaseUseBegin?.Invoke(evnetArgs);
            Coroutine coroutine = null;
            while (true)
            {
                yield return null;
                coroutine ??= StartCoroutine(ChooseToUse());
                if (State == UnitState.EndUse)
                {
                    State = UnitState.Waiting;
                    StopCoroutine(coroutine);
                    break;
                }
            }
            //step 4:
            Phase = Phase.end;

            yield return null;
            TurnSystem.NextTurn();
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
        foreach (Unit unit in targets)
        {
            Debug.Log($"{Lib.Translate(name)}对{Lib.Translate(unit.name)}使用了{card.name}");
        }
        hand.Remove(card);
        OnUseOrRespondCard?.Invoke(this, card);
        card.Use(targets);
        if (card.name == "sha")
            shaNum--;
        else if (card.name == "jiu")
            jiuNum--;
        SelectedCard = null;
    }
    //打出
    public void Respond(Card card)
    {
        Debug.Log($"{Lib.Translate(name)}打出了{Lib.Translate(card.name)}");
        hand.Remove(card);
        OnUseOrRespondCard?.Invoke(this, card);
        card.Respond();
        SelectedCard = null;
    }
    //成为目标
    public void BeTargeted(Card card)
    {
        Debug.Log($"{Lib.Translate(name)}成为{Lib.Translate(card.name)}的目标");
        switch (card)
        {
            case Sha sha:
                StartCoroutine(ChooseToRespond((card) => card.name == "shan", () => Damage(sha.damage)));
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
        OnDamageEnd?.Invoke(evnetArgs);
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
        OnAddSkill?.Invoke(skill);
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
            default:
                throw new NotImplementedException("技能类型异常");
        }
        Debug.Log($"{Lib.Translate(name)}添加了技能{skill.name}");
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
    private Unit CheckCurrentUnit()
    {
        if (TurnSystem.InActionPlayer != this && isEnemy == false)
        {
            Unit pre = TurnSystem.InActionPlayer;
            TurnSystem.InActionPlayer = this;
            return pre;
        }
        return null;
    }

    private void StopSelectingTarget()
    {
        try
        {
            while (targets.Count > 0)
            {
                targets[0].Chosen = false;
            }
        }
        catch (Exception e)
        {
            throw e;
        }
        UnitManager.UpdateUnitInteractable((unit) => false);
    }

    public IEnumerator ChooseToUse()
    {
        yield return new WaitForEndOfFrame();
        CheckCurrentUnit();
        CanCancel = false;
        bool ready_pre = true;
        Coroutine coroutine = null;
        while (true)
        {
            yield return null;
            bool ready = SelectedCard != null;
            //与上一帧相比发生变化时再进行，避免每一帧都进行无意义的调用
            if (ready != ready_pre)
            {
                ready_pre = ready;
                if (ready)
                {
                    //如果就绪，将除选择牌以外的牌可选择性改成false
                    UpdateCardInteractable((card) => card == SelectedCard);
                    coroutine = StartCoroutine(ChooseUseTarget(SelectedCard.targetFilter, SelectedCard.targetNum, callback));
                    void callback()
                    {
                        Use(SelectedCard);
                    }
                    CanCancel = true;
                }
                else
                {
                    if (coroutine != null)
                    {
                        StopSelectingTarget();
                        StopCoroutine(coroutine);
                    }
                    UpdateCardInteractable();
                    CanConfirm = false;
                    CanCancel = false;
                }
            }
            if (State == UnitState.Canceled)
            {
                if (coroutine != null)
                {
                    StopSelectingTarget();
                    StopCoroutine(coroutine);
                }
                SelectedCard.Selected = false;
                SelectedCard = null;
                CanCancel = false;
                UpdateCardInteractable();
            }
        }
    }

    public IEnumerator ChooseUseTarget(Func<Unit, bool> filter, int num, Action callback)
    {
        yield return new WaitForEndOfFrame();
        CheckCurrentUnit();
        CanConfirm = false;
        UnitManager.UpdateUnitInteractable(filter);
        int count_pre = 0;
        while (true)
        {
            yield return null;
            if (count_pre != targets.Count)
            {
                count_pre = targets.Count;
                bool ready = targets.Count == num;
                if (ready)
                {
                    UnitManager.UpdateUnitInteractable((unit) => targets.Contains(unit));
                }
                else
                {
                    UnitManager.UpdateUnitInteractable(filter);
                }
                CanConfirm = ready;
            }
            switch (State)
            {
                case UnitState.Waiting:
                    break;
                case UnitState.Confirmed:
                    callback();
                    CanConfirm = false;
                    UpdateCardInteractable();
                    StopSelectingTarget();
                    yield break;
                case UnitState.Canceled:
                    CanConfirm = false;
                    StopSelectingTarget();
                    yield break;
                default:
                    break;
            }
        }
    }

    public IEnumerator ChooseToRespond(Func<Card, bool> filter, Action fail = null)
    {
        yield return new WaitForEndOfFrame();
        Unit pre = CheckCurrentUnit();
        CanCancel = true;
        bool ready_pre = true;
        while (true)
        {
            yield return null;
            bool ready = SelectedCard != null && filter(SelectedCard);
            if (ready != ready_pre)
            {
                ready_pre = ready;
                if (ready)
                {
                    UpdateCardInteractable((card) => card == SelectedCard);
                    CanConfirm = true;
                }
                else
                {
                    CanConfirm = false;
                    UpdateCardInteractable(filter);
                }
            }
            switch (State)
            {
                case UnitState.Waiting:
                    break;
                case UnitState.Confirmed:
                    Respond(SelectedCard);
                    CanConfirm = false;
                    UpdateCardInteractable();
                    TurnSystem.InActionPlayer = pre;
                    yield break;
                case UnitState.Canceled:
                    CanConfirm = false;
                    fail?.Invoke();
                    TurnSystem.InActionPlayer = pre;
                    yield break;
                default:
                    break;
            }
        }
    }

    public IEnumerator ChooseWhetherToActivateSkill()
    {
        yield break;
    }
    #endregion
}


