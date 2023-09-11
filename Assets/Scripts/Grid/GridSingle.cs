using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GridSingle : MonoBehaviour
{
    [SerializeField] private TextMeshPro textMeshPro;
    [SerializeField] private SpriteRenderer gridVisual;
    public Unit Unit { get; set; }
    private void Awake()
    {
        int x = (int)transform.position.x;
        int y = (int)transform.position.y;
        textMeshPro.text = $"({x},{y})";
    }

    public void Show(Color color)
    {
        gridVisual.enabled = true;
        gridVisual.color = color;
    }

    public void Hide()
    {
        gridVisual.enabled = false;
    }
    public bool HasUnit => Unit != null;
}
