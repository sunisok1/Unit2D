using Newtonsoft.Json.Linq;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardUI : MonoBehaviour, IPointerClickHandler
{
    private Card card;

    public void SetCard(Card card)
    {
        this.card = card;
    }

    [SerializeField] private TextMeshProUGUI CardNameText;
    [SerializeField] private Image Image;

    public static string ImagePath = "Image/";

    [SerializeField] private Image[] TargetImages;
    [SerializeField] private TextMeshProUGUI[] TargetTexts;
    [Header("Colors")]
    [SerializeField] private Color NormalColor;
    [SerializeField] private Color TextNormalColor;
    [SerializeField] private Color DisabledColor;
    [SerializeField] private Color TextDisableColor;

    public IEnumerator Start()
    {
        yield return new WaitWhile(() => card == null);

        CardNameText.text = Lib.Translate[card.Name];
        Image.sprite = Resources.Load<Sprite>(ImagePath + card.Name);
        card.OnInteractable += Card_OnInteractable;
        card.OnSelected += Card_OnSelected;
        card.OnGoToDiscardPile += Card_OnGoToDiscardPile;
    }



    public void OnDestroy()
    {
        card.OnInteractable -= Card_OnInteractable;
        card.OnSelected -= Card_OnSelected;
        card.OnGoToDiscardPile -= Card_OnGoToDiscardPile;
    }
    /// <summary>
    /// 点击事件
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        //反转selected状态，属性内部会检查是否interactable
        card.Selected = !card.Selected;
    }

    private void Card_OnInteractable(bool value)
    {
        //更新颜色
        SetColor(value);
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
