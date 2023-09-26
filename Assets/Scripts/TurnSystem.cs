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
                inActionPlayer.Leave();
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
        inTurnPlayer.Phase = Phase.begin;

        //step 2:
        inTurnPlayer.Phase = Phase.draw;
        inTurnPlayer.Draw(6);

        //step 3:
        inTurnPlayer.Phase = Phase.use;
        Coroutine coroutine = null;
        while (true)
        {
            yield return null;
            coroutine ??= inTurnPlayer.StartCoroutine(inTurnPlayer.ChooseToUse());
            if (inTurnPlayer.Phase == Phase.end)
            {
                inTurnPlayer.StopCoroutine(coroutine);
                inTurnPlayer.EndTurn();
                break;
            }
        }
        //step 4:
        inTurnPlayer.Phase = Phase.end;
        yield return null;
        NextTurn();
    }

    public static void EndUse()
    {
        if (inActionPlayer.Phase == Phase.use)
            inActionPlayer.Phase = Phase.end;
    }

    public static void NextTurn()
    {
        TurnNumber++;
        StartTurn(inActionPlayer.next);
    }
}
