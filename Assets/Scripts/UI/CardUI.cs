using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class CardUI : MonoBehaviour, IPointerClickHandler
    {
        private Card card;

        public void SetCard(Card card)
        {
            this.card = card;
        }

        [SerializeField] private TextMeshProUGUI CardNameText;
        [SerializeField] private Image Image;

        public static string ImagePath = "Image/card/";

        [SerializeField] private Image[] TargetImages;
        [SerializeField] private TextMeshProUGUI[] TargetTexts;
        [Header("Colors")]
        [SerializeField] private Color NormalColor;
        [SerializeField] private Color TextNormalColor;
        [SerializeField] private Color DisabledColor;
        [SerializeField] private Color TextDisableColor;

        [SerializeField] private bool interactable;

        public IEnumerator Start()
        {
            yield return new WaitForEndOfFrame();

            CardNameText.text = card.chName;
            Image.sprite = Resources.Load<Sprite>(ImagePath + card.name);
            SetColor(card.interactable);
            card.OnSelected += Card_OnSelected;
            card.OnGoToDiscardPile += Card_OnGoToDiscardPile;
        }

        private void Update()
        {
            if (interactable != card.interactable)
            {
                interactable = card.interactable;
                SetColor(interactable);
            }
        }


        public void OnDestroy()
        {
            card.OnSelected -= Card_OnSelected;
            card.OnGoToDiscardPile -= Card_OnGoToDiscardPile;
        }
        /// <summary>
        /// 点击事件
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerClick(PointerEventData eventData)
        {
            if (card.interactable)
            {
                card.Selected = !card.Selected;
            }
        }

        private void Card_OnSelected(object obj, bool value)
        {
            //选中就上移40，反之下移40
            transform.Translate(new Vector3(0, value ? 40 : -40, 0));
        }
        private void Card_OnGoToDiscardPile()
        {
            Destroy(gameObject);
        }
        /// <summary>
        /// 设置颜色
        /// </summary>
        /// <param name="color"></param>
        public void SetColor(bool normal)
        {
            Color imageColor = normal ? NormalColor : DisabledColor;
            Color textColor = normal ? TextNormalColor : TextDisableColor;

            foreach (Image image in TargetImages)
            {
                image.color = imageColor;
            }
            foreach (TextMeshProUGUI text in TargetTexts)
            {
                text.color = textColor;
            }
        }
    }
}
