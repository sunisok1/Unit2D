using System.Collections.Generic;
using UnityEngine;

public class GridVisual : MonoBehaviour
{
    private static readonly Dictionary<string, Color> GridMaterialList = new()
    {
        ["White"] = new Color(1, 1, 1, 1),
        ["Blue"] = new Color(0.2f, 0.7f, 0.9f, 1),
        ["Red"] = new Color(1, 0, 0, 1),
        ["RedSoft"] = new Color(0.8f, 0.2f, 0.2f, 1),
        ["Yellow"] = new Color(0.9f, 0.9f, 0.2f, 1)
    };

    private void HideAllGridPosition()
    {
        for (int x = 0; x < GridSystem.width; x++)
        {
            for (int y = 0; y < GridSystem.height; y++)
            {
                GridSystem.gridArray[x, y].Hide();
            }
        }
    }

    private void ShowGridPositionList(List<Vector2Int> gridPositonList, Color color)
    {
        foreach (Vector2Int gridPositon in gridPositonList)
        {
            GridSystem.gridArray[gridPositon.x, gridPositon.y].Show(color);
        }
    }
}
