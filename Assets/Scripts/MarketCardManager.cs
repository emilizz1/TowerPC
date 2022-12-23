using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class MarketCardManager : MonoSingleton<MarketCardManager>
{
    [SerializeField] List<MarketCardDisplay> marketCardDisplays;
    public int basePrice;
    public CardHolder marketCards;
    
    internal List<MarketDeck> marketDecks;
    bool noPrice;

    void Start()
    {
        marketDecks = new List<MarketDeck>();
        for (int i = 0; i < 5; i++)
        {
            marketDecks.Add(new MarketDeck());
            marketDecks[i].CreateNewDeck();
        }

        for (int i = 0; i < marketDecks.Count; i++)
        {
            CreateStartingMarket(i);
        }
    }

    void CreateStartingMarket(int deckIndex)
    {
        marketDecks[deckIndex].CreateNewDeck();

        foreach (Card card in marketCards.cards)
        {
            if (card.cardType == CardType.action && card.cardLevel < 2 && deckIndex == 0)
            {
                marketDecks[0].AddCard(card);
            }
            if (card.cardType == CardType.spell && card.cardLevel < 2 && deckIndex == 1)
            {
                marketDecks[1].AddCard(card);
            }
            if (card.cardType == CardType.tower && card.cardLevel < 2 && deckIndex == 2)
            {
                marketDecks[2].AddCard(card);
            }
            if (card.cardLevel == 2 && deckIndex == 3)
            {
                marketDecks[3].AddCard(card);
            }
            if (card.cardLevel == 3 && deckIndex == 4)
            {
                marketDecks[4].AddCard(card);
            }
        }

        foreach (Card card in CharacterSelector.firstCharacter.marketCards.cards)
        {
            if(card.cardType == CardType.action && card.cardLevel < 2 && deckIndex == 0)
            {
                marketDecks[0].AddCard(card);
            }
            if (card.cardType == CardType.spell && card.cardLevel < 2 && deckIndex == 1)
            {
                marketDecks[1].AddCard(card);
            }
            if (card.cardType == CardType.tower && card.cardLevel < 2 && deckIndex == 2)
            {
                marketDecks[2].AddCard(card);
            }
            if(card.cardLevel == 2 && deckIndex == 3)
            {
                marketDecks[3].AddCard(card);
            }
            if(card.cardLevel == 3 && deckIndex == 4)
            {
                marketDecks[4].AddCard(card);
            }
        }

        foreach (Card card in CharacterSelector.secondCharacter.marketCards.cards)
        {
            if (card.cardType == CardType.action && card.cardLevel < 2 && deckIndex == 0)
            {
                marketDecks[0].AddCard(card);
            }
            if (card.cardType == CardType.spell && card.cardLevel < 2 && deckIndex == 1)
            {
                marketDecks[1].AddCard(card);
            }
            if (card.cardType == CardType.tower && card.cardLevel < 2 && deckIndex == 2)
            {
                marketDecks[2].AddCard(card);
            }
            if (card.cardLevel == 2 && deckIndex == 3)
            {
                marketDecks[3].AddCard(card);
            }
            if (card.cardLevel == 3 && deckIndex == 4)
            {
                marketDecks[4].AddCard(card);
            }
        }
    }

    public void DisplayNewMarket()
    {
        for (int i = 0; i < marketCardDisplays.Count; i++)
        {
            if (marketDecks[i].deck.Count == 0)
            {
                if(marketDecks[i].discard.Count == 0)
                {
                    CreateStartingMarket(i);
                }
                marketDecks[i].ShuffleDeck();
            }
            Card cardToDisplay = marketDecks[i].deck[UnityEngine.Random.Range(0, marketDecks[i].deck.Count)];
            marketCardDisplays[i].DisplayCard(cardToDisplay, noPrice);
            marketDecks[i].RemoveCard(cardToDisplay);
        }
    }

    public void CloseMarket()
    {
        for (int i = 0; i < marketCardDisplays.Count; i++)
        {
            marketCardDisplays[i].DiscardMarketCard(i);

        }
    }

    public void RemoveLevel0Cards()
    {
        foreach (MarketDeck deck in marketDecks)
        {
            List<Card> cardsToRemove = new List<Card>();
            foreach(Card card in deck.deck)
            {
                if(card.cardLevel == 0)
                {
                    cardsToRemove.Add(card);
                }
            }
            foreach(Card card in cardsToRemove)
            {
                deck.RemoveCard(card);
            }

            cardsToRemove = new List<Card>();
            foreach (Card card in deck.discard)
            {
                if (card.cardLevel == 0)
                {
                    cardsToRemove.Add(card);
                }
            }
            foreach (Card card in cardsToRemove)
            {
                deck.RemoveCardFromDiscard(card);
            }
        }
    }

    public void NoMoreCardBuying()
    {
        MarketDeck newMarketDeck = new MarketDeck();
        newMarketDeck.CreateNewDeck();

        foreach (MarketDeck deck in marketDecks)
        {
            foreach(Card card in deck.deck)
            {
                newMarketDeck.AddCard(card);
            }
        }

        marketDecks = new List<MarketDeck>();
        marketDecks.Add(newMarketDeck);
        marketDecks.Add(newMarketDeck);

        marketCardDisplays[4].gameObject.SetActive(false);
        marketCardDisplays[3].gameObject.SetActive(false);
        marketCardDisplays[2].gameObject.SetActive(false);
        marketCardDisplays.RemoveAt(4);
        marketCardDisplays.RemoveAt(3);
        marketCardDisplays.RemoveAt(2);

        marketCardDisplays[0].transform.localPosition = new Vector3(-150f, 0f, 0f);
        marketCardDisplays[1].transform.localPosition = new Vector3(150f, 0f, 0f);

        noPrice = true;
    }
}

[Serializable]
public class MarketDeck
{
    public List<Card> deck;
    public List<Card> discard;

    public void CreateNewDeck()
    {
        deck = new List<Card>();
        discard = new List<Card>();
    }

    public void AddCard(Card card)
    {
        deck.Add(card);
    }

    public void DiscardCard(Card card)
    {
        discard.Add(card);
    }

    public void RemoveCard(Card card)
    {
        deck.Remove(card);
    }

    public void RemoveCardFromDiscard(Card card)
    {
        discard.Remove(card);
    }

    public void ShuffleDeck()
    {
        deck = discard;
        discard = new List<Card>();
    }
}
