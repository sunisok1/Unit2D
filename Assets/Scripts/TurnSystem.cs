using System;
using System.Collections;
using UnityEngine;

public static class TurnSystem
{
    /// <summary>
    /// 当前单位进入时调用
    /// </summary>
    public static event Action<Unit> OnCurrentUnitEnter;
    /// <summary>
    /// 当前单位离开时调用
    /// </summary>
    public static event Action<Unit> OnCurrentUnitLeave;

    private static Unit inActionPlayer;
    private static Unit inTurnPlayer;

    public static int TurnNumber { get; private set; } = 1;

    public static Unit InActionPlayer
    {
        get => inActionPlayer;
        set
        {
            if (inActionPlayer == value) return;
            if (inActionPlayer != null)
            {
                OnCurrentUnitLeave(inActionPlayer);
            }
            inActionPlayer = value;
            OnCurrentUnitEnter(inActionPlayer);
        }
    }
    public static Unit InTurnPlayer
    {
        get => inTurnPlayer;
        private set
        {
            inTurnPlayer = value;
            InActionPlayer = value;
        }
    }

    public static void SetInTurnPlayer(Unit inTurnPlayer)
    {
        InTurnPlayer = inTurnPlayer;
    }


    public static void StartGame()
    {
        if (UnitManager.FriendlyUnitList.Count == 0)
        {
            Debug.LogWarning("UnitList is Empty.Can not start game");
        }
        UnitManager.First.StartTurn();
    }
    public static void NextTurn()
    {
        TurnNumber++;
        inActionPlayer.next.StartTurn();
    }
}
