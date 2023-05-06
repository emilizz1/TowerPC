using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardHolderManager : MonoSingleton<CardHolderManager>
{
    public CardHolder allCards;
    public Card curseCard;
    public Card engineCard;


    public Card GetUpgradedCard(Card cardToUpgrade)
    {
        foreach (Card card in CardHolderManager.instance.allCards.cardsCollection[0].cards)
        {
            if (card.cardName == cardToUpgrade.cardName)
            {
                if (card.cardLevel == cardToUpgrade.cardLevel + 1)
                {
                    return Instantiate(card);
                }
            }
        }
        return null;
    }

    public Card GetCardByName(string name)
    {
        foreach (Card card in allCards.cardsCollection[0].cards)
        {
            if(card.cardName + card.cardLevel == name)
            {
                return Instantiate( card);
            }
        }
        return null;
    }
}
