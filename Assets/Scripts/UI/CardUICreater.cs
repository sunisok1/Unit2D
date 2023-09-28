using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public static class CardUICreater
    {
        private static readonly Dictionary<Card, GameObject> CardObjects = new();

        private static readonly GameObject UICardPrefab;

        private static readonly Transform HandCardSlot;

        static CardUICreater()
        {
            UICardPrefab = Resources.Load<GameObject>("Prefabs/Card");
            if (UICardPrefab == null)
            {
                Debug.LogError("Prefabs/Card missing");
            }

            HandCardSlot = GameObject.Find("UI/PlayerUI/CardPanel").transform;
            if (HandCardSlot == null)
            {
                Debug.LogError("Cann't find UI/PlayerUI/CardPanel");
            }
            else
            {
                ClearAll();
            }
        }

        public static void Init()
        {
            TurnSystem.OnCurrentUnitEnter += TurnSystem_OnCurrentUnitEnter;
            TurnSystem.OnCurrentUnitLeave += TurnSystem_OnCurrentUnitLeave;
        }

        private static void TurnSystem_OnCurrentUnitLeave(object obj)
        {
            Unit unit = obj as Unit;
            unit.hand.OnAdd -= Unit_OnAddCard;
            unit.hand.OnRemove -= Unit_OnRemoveCard;
        }
        private static void TurnSystem_OnCurrentUnitEnter(object obj)
        {
            Unit unit = obj as Unit;
            unit.hand.OnAdd += Unit_OnAddCard;
            unit.hand.OnRemove += Unit_OnRemoveCard;
            ClearAll();
            CreateCard(unit.hand);
        }

        private static void Unit_OnRemoveCard(Card card)
        {
            Object.Destroy(CardObjects[card]);
            CardObjects.Remove(card);
        }

        private static void Unit_OnAddCard(Card card)
        {
            CreateCard(card);
        }


        private static void CreateCard(Card card)
        {
            GameObject gameObject = Object.Instantiate(UICardPrefab, HandCardSlot);
            CardUI ui = gameObject.GetComponent<CardUI>();
            ui.SetCard(card);
            CardObjects.Add(card, gameObject);
        }

        private static void CreateCard(IEnumerable<Card> cards)
        {
            foreach (Card card in cards)
            {
                CreateCard(card);
            }
        }

        private static void ClearAll()
        {
            CardObjects.Clear();
            for (int i = 0; i < HandCardSlot.transform.childCount; i++)
            {
                Transform transform = HandCardSlot.transform.GetChild(i);
                Object.Destroy(transform.gameObject);
            }
        }
    }
}
