using UnityEngine;

public static class GridSystem
{
    public static readonly int width;
    public static readonly int height;
    public static readonly float cellSize;
    public static readonly GridSingle[,] gridArray;

    private static readonly GameObject gridDebugObjectPrefab;
    private static readonly Transform DebugObjectParent;
    public static readonly Transform UnitsParent;

    static GridSystem()
    {
        width = 20;
        height = 20;
        cellSize = 1;
        gridArray = new GridSingle[width, height];

        gridDebugObjectPrefab = Resources.Load<GameObject>("Prefabs/GridSingle");
        if (gridDebugObjectPrefab == null)
        {
            Debug.LogError("Prefabs/GridSingle prefab missing!");
        }

        DebugObjectParent = GameObject.Find("GridSystem/GridDebugObjectParent").transform;
        if (DebugObjectParent == null)
        {
            Debug.LogError("Cann't find DebugObjectParent in scence!");
        }

        UnitsParent = GameObject.Find("GridSystem/UnitsParent").transform;
        if (UnitsParent == null)
        {
            Debug.LogError("Cann't find UnitsParent in scence!");
        }


        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GameObject grid = Object.Instantiate(gridDebugObjectPrefab, GetWorldPosition(x, y), Quaternion.identity, DebugObjectParent);
                gridArray[x, y] = grid.GetComponent<GridSingle>();
            }
        }
    }

    public static void AddUnitAtGridPosition(int x, int y, Unit unit)
    {
        gridArray[x, y].Unit = unit;
    }

    public static void RemoveUnitAtGridPosition(int x, int y)
    {
        gridArray[x, y].Unit = null;
    }

    public static Vector2Int GetGridPositon(Vector3 worldPosition)
    {
        return new((int)Mathf.Round(worldPosition.x / cellSize), (int)Mathf.Round(worldPosition.y / cellSize));
    }

    public static bool IsValidGridPosition(int x, int y)
    {
        return x >= 0 && y >= 0 && x < width && y < height;
    }

    public static bool HasAnyUnitOnGridPosition(int x, int y)
    {
        return gridArray[x, y].HasUnit;
    }

    public static Vector3 GetWorldPosition(int x, int y)
    {
        return new(x * cellSize, y * cellSize, 0);
    }
}
