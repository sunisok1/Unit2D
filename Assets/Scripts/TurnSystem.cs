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

    private static Unit operatePlayer;
    private static Unit player;

    public static int TurnNumber { get; private set; } = 1;

    public static Unit OperatePlayer
    {
        get => operatePlayer;
        set
        {
            if (operatePlayer == value) return;
            if (operatePlayer != null)
            {
                OnCurrentUnitLeave(operatePlayer);
                operatePlayer.Leave();
            }
            operatePlayer = value;
            OnCurrentUnitEnter(operatePlayer);
        }
    }
    public static Unit InTurnPlayer
    {
        get => player;
        private set
        {
            player = value;
            OperatePlayer = value;
        }
    }


    public static void StartGame()
    {
        if (UnitManager.FriendlyUnitList.Count == 0)
        {
            Debug.LogWarning("UnitList is Empty.Can not start game");
        }
        UnitManager.First.StartCoroutine(StartTurn(UnitManager.First));
    }

    private static IEnumerator StartTurn(Unit unit)
    {
        InTurnPlayer = unit;
        //step 0:

        //step 1:
        player.Phase = Phase.begin;

        //step 2:
        player.Phase = Phase.draw;
        player.Draw(6);

        //step 3:
        player.Phase = Phase.use;
        while (player.Phase == Phase.use)
        {
            yield return player.ChooseToUse();
        }

        //step 4:
        player.Phase = Phase.end;
        yield return null;
        NextTurn();
    }

    public static void EndUse()
    {
        if (operatePlayer.Phase == Phase.use)
            operatePlayer.Phase = Phase.end;
    }

    public static void NextTurn()
    {
        TurnNumber++;
        StartTurn(operatePlayer.next);
    }
}
