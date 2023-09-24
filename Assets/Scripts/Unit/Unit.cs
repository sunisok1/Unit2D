using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.CanvasScaler;

public class Unit : MonoBehaviour
{
    #region Fileds

    public bool isEnemy;

    private int hp = 4;

    private int shaNum = 1;

    private int jiuNum = 1;

    private Vector2Int gridPositon;


    /// <summary>
    /// 手牌
    /// </summary>
    private readonly List<Card> hand = new();

    [NonSerialized]
    //下家
    public Unit next;
    //选择的牌
    private Card SelectedCard;
    //选择的单位
    private readonly List<Unit> targets = new();
    public Phase Phase { get; set; }

    #endregion

    #region Events
    // 加入手牌时调用
    public event Action<Card> OnAddCard;
    //离开手牌时调用
    public event Action<Card> OnRemoveCard;
    // 更改被选择状态时调用
    public event Action<bool> OnChosen;
    //更改可选择性时调用
    public event Action<bool> OnInteractable;
    //可确定时调用
    public event Action<bool> OnCanConfirm;
    //可取消时调用
    public event Action<bool> OnCanCancel;
    //使用时调用
    public event Action<Card> OnUseOrRespondCard;
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
    #endregion
    private void Awake()
    {
        gridPositon = GridSystem.GetGridPositon(transform.position);
        GridSystem.AddUnitAtGridPosition(gridPositon.x, gridPositon.y, this);

        OnAddCard += Unit_OnAddCard;
        OnRemoveCard += Unit_OnRemoveCard;

        OnChosen += Unit_OnChosen;
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
            TurnSystem.OperatePlayer.targets.Add(this);
        }
        else
        {
            TurnSystem.OperatePlayer.targets.Remove(this);
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

            switch (card.Name)
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

    public void ResetCards()
    {
        foreach (Card card in hand)
        {
            card.Selected = false;
        }
    }

    public IEnumerable<Card> GetCards(char type)
    {
        if (type == 'h')
        {
            return hand;
        }
        return null;
    }
    #endregion

    #region GameLogics

    public void Draw(int amount = 1)
    {
        for (int i = 0; i < amount; i++)
        {
            Card card = Deck.GetOne();
            hand.Add(card);
            OnAddCard(card);
        }
    }

    public void Use(Card card)
    {
        Debug.Log("使用 " + card.Name);
        OnUseOrRespondCard?.Invoke(card);
        card.Use(targets);
        if (card.Name == "sha")
            shaNum--;
        else if (card.Name == "jiu")
            jiuNum--;
        SelectedCard = null;
    }

    public void Respond(Card card)
    {
        Debug.Log("打出 " + card.Name);
        OnUseOrRespondCard?.Invoke(card);
    }

    public void Damage(int amount = 1)
    {
        hp -= amount;
        Debug.Log(name + "curhp = " + hp);
    }

    #endregion

    #region Operations
    private void StopSelectingTarget()
    {
        while (targets.Count > 0)
        {
            targets[0].Chosen = false;
        }
        UnitManager.UpdateUnitInteractable((unit) => false);
    }

    /// 选择一张牌使用
    /// </summary>
    /// <returns></returns>
    public IEnumerator ChooseToUse()
    {
        bool ready_pre = true;
        while (true)
        {
            bool ready = SelectedCard != null;
            //与上一帧相比发生变化时再进行，避免每一帧都进行无意义的调用
            if (ready != ready_pre)
            {
                ready_pre = ready;
                Coroutine coroutine = null;
                if (ready)
                {
                    //如果就绪，将除选择牌以外的牌可选择性改成false
                    UpdateCardInteractable((card) => card == SelectedCard);
                    coroutine = StartCoroutine(ChooseUseTarget(SelectedCard.targetFilter, SelectedCard.targetNum, callback));
                    void callback()
                    {
                        Use(SelectedCard);
                    }
                }
                else
                {
                    if (coroutine != null)
                    {
                        StopSelectingTarget();
                        StopCoroutine(coroutine);
                    }
                    CanConfirm = false;
                    UpdateCardInteractable();
                }
            }
            yield return null;
        }
    }

    public IEnumerator ChooseUseTarget(Func<Unit, bool> filter, int num, Action callback)
    {
        CanConfirm = false;
        UnitManager.UpdateUnitInteractable(filter);
        int count_pre = 0;
        while (true)
        {
            yield return null;
            if (count_pre != targets.Count)
            {
                count_pre = targets.Count;
                UnitManager.UpdateUnitInteractable((unit) => targets.Contains(unit));
                CanConfirm = targets.Count == num;
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
    #endregion
}


