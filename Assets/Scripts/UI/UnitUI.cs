using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Unit))]
    public class UnitUI : MonoBehaviour, IPointerClickHandler
    {
        Unit unit;
        [SerializeField]
        private Image chosenVisual;

        private static Color disableColor = Color.gray;
        private static Color enableColor = new(0.3f, 0.7f, 0.8f);
        private static Color chosenColor = new(0.9f, 0.2f, 0.2f);
        private void Start()
        {
            unit = GetComponent<Unit>();
            unit.OnInteractable += Unit_OnInteractable;
            unit.OnChosen += Unit_OnChosen;
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            if (!unit.Interactable) return;
            unit.Chosen = !unit.Chosen;
        }

        private void Unit_OnInteractable(bool obj)
        {
            chosenVisual.color = obj ? enableColor : disableColor;
        }

        private void Unit_OnChosen(bool obj)
        {
            chosenVisual.color = obj ? chosenColor : enableColor;
        }
    }
}