using System.Collections.Generic;
using UnityEngine;
using System;

public static class UnitManager
{
    public static Unit First = null;
    public static Unit Last = null;
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

    public static Unit SpawnUnit(int x, int y, string name, bool isEnemy)
    {
        Vector3 position = GridSystem.GetWorldPosition(x, y);
        GameObject prefab = isEnemy ? UnitEnemyPrefab : UnitPrefab;
        GameObject gameObject = UnityEngine.Object.Instantiate(prefab, position, Quaternion.identity, GridSystem.UnitsParent);
        gameObject.name = name;
        Unit unit = gameObject.GetComponent<Unit>();
        if (First == null)
        {
            First = unit;
            Last = unit;
        }

        Last.next = unit;
        unit.pre = Last;
        unit.next = First;
        First.pre = unit;
        Last = unit;

        UnitList.Add(unit);
        (unit.isEnemy ? EnemyUnitList : FriendlyUnitList).Add(unit);
        unit.Chosen = false;
        return unit;
    }

    public static void UpdateUnitInteractable(Args args, Func<Unit, bool> filter = null, bool ready = false)
    {
        filter ??= (unit) => false;
        if (ready)
        {
            filter = (unit) => args.targets.Contains(unit);
        }
        foreach (Unit unit in UnitList)
        {
            unit.Interactable = filter(unit);
        }
    }
}
