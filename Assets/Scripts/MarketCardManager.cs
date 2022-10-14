using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarketCardManager : MonoSingleton<MarketCardManager>
{
    [SerializeField] List<MarketCardDisplay> marketCardDisplays;
    public int basePrice;
    [SerializeField] CardHolder marketCards;

    List<Card> marketDeck;
    internal List<Card> marketDiscard;

    void Start()
    {
        marketDeck = new List<Card>();
        marketDiscard = new List<Card>();

        CreateStartingMarket();
    }

    void CreateStartingMarket()
    {
        foreach(Card card in marketCards.cards)
        {
            marketDeck.Add(card);
        }

        foreach (Card card in CharacterSelector.firstCharacter.marketCards.cards)
        {
            marketDeck.Add(card);
        }

        foreach (Card card in CharacterSelector.secondCharacter.marketCards.cards)
        {
            marketDeck.Add(card);
        }

    }

    public void DisplayNewMarket()
    {

        foreach(MarketCardDisplay display in marketCardDisplays)
        {
            if(marketDeck.Count == 0)
            {
                marketDeck.AddRange(marketDiscard);
                marketDiscard = new List<Card>();
            }
            Card cardToDisplay = marketDeck[Random.Range(0, marketDeck.Count)];
            display.DisplayCard(cardToDisplay);
            marketDeck.Remove(cardToDisplay);
        }
    }

    public void CloseMarket()
    {
        foreach (MarketCardDisplay display in marketCardDisplays)
        {
            display.DiscardMarketCard();
        }
    }
}
