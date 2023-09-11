using System.Collections.Generic;
using UnityEngine;

public static class Deck
{
    private static readonly List<Card> cardList = new();

    static Deck()
    {
        cardList.Clear();
        for (int i = 0; i < 100; i++)
        {
            cardList.Add(new Sha());
        }
        for (int i = 0; i < 100; i++)
        {
            cardList.Add(new Shan());
        }
    }

    public static Card GetOne()
    {
        int random = Random.Range(0, cardList.Count);
        Card card = cardList[random];
        cardList.Remove(card);
        return card;
    }
}
