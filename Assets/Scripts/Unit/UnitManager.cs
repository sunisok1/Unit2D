using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Experimental.GlobalIllumination;

public static class UnitManager
{
    public static Unit First { get; private set; } = null;
    public static Unit Last { get; private set; } = null;
    public static List<Unit> UnitList { get; private set; } = new();
    public static List<Unit> FriendlyUnitList { get; private set; } = new();
    public static List<Unit> EnemyUnitList { get; private set; } = new();

    private static readonly GameObject UnitPrefab;
    private static readonly GameObject UnitEnemyPrefab;

    static UnitManager()
    {
        UnitPrefab = Resources.Load<GameObject>("Prefabs/Unit");
        if (UnitPrefab == null)
        {
            Debug.LogError("Prefabs/Unit prefab missing!");
        }
        UnitEnemyPrefab = Resources.Load<GameObject>("Prefabs/UnitEnemy");
        if (UnitEnemyPrefab == null)
        {
            Debug.LogError("Prefabs/UnitEnemy prefab missing!");
        }
    }

    public static GameObject SpawnUnit(int x, int y, string name, bool isEnemy = false)
    {
        Vector3 position = GridSystem.GetWorldPosition(x, y);
        GameObject prefab = isEnemy ? UnitEnemyPrefab : UnitPrefab;
        GameObject gameObject = UnityEngine.Object.Instantiate(prefab, position, Quaternion.identity, GridSystem.UnitsParent);
        gameObject.name = name;
        Unit unit = gameObject.GetComponent<Unit>();
        OnAnyUnitSpawned(unit);
        unit.Chosen = false;
        return gameObject;
    }

    public static void ReSetChosen()
    {
        foreach (Unit unit in UnitList)
        {
            unit.Chosen = false;
        }
    }

    private static void OnAnyUnitSpawned(Unit unit)
    {
        if (First == null)
        {
            First = unit;
            Last = unit;
        }

        Last.next = unit;
        unit.next = First;
        Last = unit;

        UnitList.Add(unit);
        (unit.isEnemy ? EnemyUnitList : FriendlyUnitList).Add(unit);
    }

    private static void OnAnyUnitDead(Unit unit)
    {
        throw new System.NotImplementedException();
    }

    internal static void UpdateUnitInteractable(Func<Unit, bool> filter = null)
    {
        filter ??= (unit) => false;
        foreach (Unit unit in UnitList)
        {
            unit.Interactable = filter(unit);
        }
    }
}
