using System.Collections.Generic;
using UnityEngine;

public class SkillButtons : MonoBehaviour
{
    [SerializeField] private Vector2 cellSize;
    [SerializeField] private Vector2 spacing;
    [SerializeField] private RectTransform skillButtonPrefab;
    private readonly List<RectTransform> buttons = new();

    private void Awake()
    {

    }

    public void AddButton()
    {
        buttons.Add(Instantiate(skillButtonPrefab, transform));
        int count = buttons.Count;
        if (count % 2 == 1)
        {
            RectTransform button = buttons[^1];
            button.anchoredPosition = new(0, (count / 2 + 1) * (spacing.y + cellSize.y) - cellSize.y);
            button.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, cellSize.x * 2 + spacing.x);
            button.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, cellSize.y);
        }
        else
        {
            RectTransform Lrect = buttons[^2];
            RectTransform Rrect = buttons[^1];
            float x = (cellSize.x + spacing.x) / 2;
            float y = (count / 2) * (spacing.y + cellSize.y) - cellSize.y;
            Lrect.anchoredPosition = new(-x, y);
            Rrect.anchoredPosition = new(x, y);
            Lrect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, cellSize.x);
            Lrect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, cellSize.y);
            Rrect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, cellSize.x);
            Rrect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, cellSize.y);
        }
    }
}
