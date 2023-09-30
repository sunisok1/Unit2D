using System.Collections.Generic;
using UnityEngine;

public static class Deck
{
    private static readonly List<Card> cardList = new();

    static Deck()
    {
        cardList.Clear();
        #region 【杀】共44张 14红30黑
        #region 普通【杀】共30张 9红21黑
        cardList.Add(new Sha(6, Suit.diamond));
        cardList.Add(new Sha(7, Suit.diamond));
        cardList.Add(new Sha(8, Suit.diamond));
        cardList.Add(new Sha(9, Suit.diamond));
        cardList.Add(new Sha(10, Suit.diamond));
        cardList.Add(new Sha(13, Suit.diamond));
        cardList.Add(new Sha(2, Suit.club));
        cardList.Add(new Sha(3, Suit.club));
        cardList.Add(new Sha(4, Suit.club));
        cardList.Add(new Sha(5, Suit.club));
        cardList.Add(new Sha(6, Suit.club));
        cardList.Add(new Sha(7, Suit.club));
        cardList.Add(new Sha(8, Suit.club));
        cardList.Add(new Sha(8, Suit.club));
        cardList.Add(new Sha(9, Suit.club));
        cardList.Add(new Sha(9, Suit.club));
        cardList.Add(new Sha(10, Suit.club));
        cardList.Add(new Sha(10, Suit.club));
        cardList.Add(new Sha(11, Suit.club));
        cardList.Add(new Sha(11, Suit.club));
        cardList.Add(new Sha(10, Suit.heart));
        cardList.Add(new Sha(10, Suit.heart));
        cardList.Add(new Sha(11, Suit.heart));
        cardList.Add(new Sha(7, Suit.spade));
        cardList.Add(new Sha(8, Suit.spade));
        cardList.Add(new Sha(8, Suit.spade));
        cardList.Add(new Sha(9, Suit.spade));
        cardList.Add(new Sha(9, Suit.spade));
        cardList.Add(new Sha(10, Suit.spade));
        cardList.Add(new Sha(10, Suit.spade));
        #endregion
        #region 【雷杀】共9张 全黑
        cardList.Add(new LeiSha(5, Suit.club));
        cardList.Add(new LeiSha(6, Suit.club));
        cardList.Add(new LeiSha(7, Suit.club));
        cardList.Add(new LeiSha(8, Suit.club));
        cardList.Add(new LeiSha(4, Suit.spade));
        cardList.Add(new LeiSha(5, Suit.spade));
        cardList.Add(new LeiSha(6, Suit.spade));
        cardList.Add(new LeiSha(7, Suit.spade));
        cardList.Add(new LeiSha(8, Suit.spade));
        #endregion
        #region 【火杀】共5张 全红
        cardList.Add(new HuoSha(4, Suit.diamond));
        cardList.Add(new HuoSha(5, Suit.diamond));
        cardList.Add(new HuoSha(4, Suit.heart));
        cardList.Add(new HuoSha(7, Suit.heart));
        cardList.Add(new HuoSha(10, Suit.heart));
        #endregion
        #endregion
        #region 【闪】共24张 全红
        cardList.Add(new Shan(2, Suit.diamond));
        cardList.Add(new Shan(2, Suit.diamond));
        cardList.Add(new Shan(3, Suit.diamond));
        cardList.Add(new Shan(4, Suit.diamond));
        cardList.Add(new Shan(5, Suit.diamond));
        cardList.Add(new Shan(6, Suit.diamond));
        cardList.Add(new Shan(6, Suit.diamond));
        cardList.Add(new Shan(7, Suit.diamond));
        cardList.Add(new Shan(7, Suit.diamond));
        cardList.Add(new Shan(8, Suit.diamond));
        cardList.Add(new Shan(8, Suit.diamond));
        cardList.Add(new Shan(9, Suit.diamond));
        cardList.Add(new Shan(10, Suit.diamond));
        cardList.Add(new Shan(10, Suit.diamond));
        cardList.Add(new Shan(11, Suit.diamond));
        cardList.Add(new Shan(11, Suit.diamond));
        cardList.Add(new Shan(11, Suit.diamond));
        cardList.Add(new Shan(2, Suit.heart));
        cardList.Add(new Shan(2, Suit.heart));
        cardList.Add(new Shan(8, Suit.heart));
        cardList.Add(new Shan(9, Suit.heart));
        cardList.Add(new Shan(11, Suit.heart));
        cardList.Add(new Shan(12, Suit.heart));
        cardList.Add(new Shan(13, Suit.heart));
        #endregion
        #region 【桃】共12张 全红
        cardList.Add(new Tao(2, Suit.diamond));
        cardList.Add(new Tao(3, Suit.diamond));
        cardList.Add(new Tao(12, Suit.diamond));
        cardList.Add(new Tao(3, Suit.heart));
        cardList.Add(new Tao(4, Suit.heart));
        cardList.Add(new Tao(5, Suit.heart));
        cardList.Add(new Tao(6, Suit.heart));
        cardList.Add(new Tao(6, Suit.heart));
        cardList.Add(new Tao(7, Suit.heart));
        cardList.Add(new Tao(8, Suit.heart));
        cardList.Add(new Tao(9, Suit.heart));
        cardList.Add(new Tao(12, Suit.heart));
        #endregion
        #region 【酒】共5张 1红4黑
        cardList.Add(new Jiu(9, Suit.diamond));
        cardList.Add(new Jiu(3, Suit.club));
        cardList.Add(new Jiu(9, Suit.club));
        cardList.Add(new Jiu(3, Suit.spade));
        cardList.Add(new Jiu(9, Suit.spade));
        #endregion
    }

    public static Card GetOne()
    {
        int random = Random.Range(0, cardList.Count);
        Card card = cardList[random];
        cardList.Remove(card);
        return card;
    }
}
