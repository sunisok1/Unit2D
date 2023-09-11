using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ChosenVisual : MonoBehaviour
{
    private Image image;

    private void Start()
    {
        image = GetComponent<Image>();

    }

    public void CanBeChosen()
    {
        image.color = Color.white;
    }

    public void CanNotBeChosen()
    {
        image.color = Color.gray;
    }

    public void HasBeenChosen()
    {
        image.color = Color.red;
    }
}
