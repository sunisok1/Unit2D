using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Unit))]
    public class UnitUI : MonoBehaviour, IPointerClickHandler
    {
        Unit unit;
        [SerializeField] private Image image;
        [SerializeField] private GameObject glowObject;
        [SerializeField] private Animator glowAnimator;
        Material material;

        private static Color disableColor = Color.gray;
        private static Color enableColor = new(0.3f, 0.7f, 0.8f);
        private static Color chosenColor = new(0.9f, 0.2f, 0.2f);
        private static Color drunkColor = new(1, 0.6f, 0.6f);

        public Material Material
        {
            get
            {
                if (material == null)
                {
                    material = new(image.material);
                    image.material = material;
                }
                return material;
            }

            set
            {
                material = value;
            }
        }

        private void Awake()
        {
            unit = GetComponent<Unit>();
            unit.OnInteractable += Unit_OnInteractable;
            unit.OnChosen += Unit_OnChosen; ;
            unit.OnSetSkin += Unit_OnSetSkin;
            unit.OnDrunk += Unit_OnDrunk;
            unit.OnDead += Unit_OnDead;
        }

        private void Unit_OnDead(object sender, System.EventArgs e)
        {
            SetGray(true);
            StartCoroutine(transparent(3f));
            IEnumerator transparent(float time)
            {
                float timer = time;
                Color color = Color.white;
                while (timer >= 0)
                {
                    yield return null;
                    timer -= Time.deltaTime;
                    color.a = timer / time;
                    SetColor(color);
                }
            }
        }

        private void Unit_OnDrunk(int drunk)
        {
            if (drunk == 0)
            {
                SetColor(Color.white);
            }
            else
            {
                SetColor(drunkColor);
            }
        }

        private void Unit_OnSetSkin(Sprite sprite)
        {
            image.sprite = sprite;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!unit.Interactable) return;
            unit.Chosen = !unit.Chosen;
        }

        private void Unit_OnInteractable(bool val)
        {
            glowObject.SetActive(val);
            if (val)
            {
                glowAnimator.SetTrigger("blue");
            }
        }

        private void Unit_OnChosen(object sender, System.EventArgs e)
        {
            bool value = (sender as Unit).Chosen;
            if (value)
            {
                glowAnimator.SetTrigger("red");
            }
            else
            {
                glowAnimator.SetTrigger("blue");
            }
        }

        public void SetColor(Color color)
        {
            Material.SetColor("_Color", color);
        }

        public void SetGray(bool val)
        {
            Material.SetFloat("_Gray", val ? 1 : 0);
        }
    }
}