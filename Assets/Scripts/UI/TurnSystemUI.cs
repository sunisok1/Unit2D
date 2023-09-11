using Newtonsoft.Json.Linq;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public static class TurnSystemUI
{
    private static Unit Player => TurnSystem.OperatePlayer;

    private static readonly Button EndTurnButton;
    private static readonly Button ConfirmButton;
    private static readonly Button CancelButton;
    private static readonly TextMeshProUGUI TurnNumberText;

    static TurnSystemUI()
    {
        string path;
        path = "UI/TurnSystemUI/OperationButtons/EndTurnButton";
        if (!GameObject.Find(path).TryGetComponent(out EndTurnButton))
        {
            Debug.LogError($"Cann't find {path} !");
        }
        path = "UI/TurnSystemUI/OperationButtons/ConfirmButton";
        if (!GameObject.Find(path).TryGetComponent(out ConfirmButton))
        {
            Debug.LogError($"Cann't find {path} !");
        }
        path = "UI/TurnSystemUI/OperationButtons/CancelButton";
        if (!GameObject.Find(path).TryGetComponent(out CancelButton))
        {
            Debug.LogError($"Cann't find {path} !");
        }
        path = "UI/TurnSystemUI/TurnNumberText";
        if (!GameObject.Find(path).TryGetComponent(out TurnNumberText))
        {
            Debug.LogError($"Cann't find {path} !");
        }
    }

    public static void Init()
    {
        EndTurnButton.onClick.AddListener(() =>
        {
            TurnSystem.EndUse();
        });

        ConfirmButton.onClick.AddListener(() => Player.Confirm());
        CancelButton.onClick.AddListener(() => Player.Cancel());

        TurnSystem.OnCurrentUnitEnter += TurnSystem_OnCurrentUnitEnter;
        TurnSystem.OnCurrentUnitLeave += TurnSystem_OnCurrentUnitLeave;

        UpdateTurnText();
        EndTurnButton.interactable = true;
    }
    #region Events
    private static void TurnSystem_OnCurrentUnitEnter(Unit unit)
    {
        UpdateTurnText();
        UpdateEndTurnButtonVisibility(unit);
        //在当前角色能确定或取消时更新Button可交互性
        unit.OnCanConfirm += Unit_OnCanConfirm;
        unit.OnCanCancel += Unit_OnCanCancel;
    }
    private static void TurnSystem_OnCurrentUnitLeave(Unit unit)
    {
        //取消绑定更新Button
        unit.OnCanConfirm -= Unit_OnCanConfirm;
        unit.OnCanCancel -= Unit_OnCanCancel;
    }
    private static void Unit_OnCanConfirm(bool value)
    {
        ConfirmButton.interactable = value;
    }
    private static void Unit_OnCanCancel(bool value)
    {
        CancelButton.interactable = value;
    }
    #endregion

    private static void UpdateTurnText()
    {
        TurnNumberText.text = $"TURN {TurnSystem.TurnNumber}";
    }

    private static void UpdateEndTurnButtonVisibility(Unit unit)
    {
        EndTurnButton.interactable = !unit.isEnemy;
    }

}
