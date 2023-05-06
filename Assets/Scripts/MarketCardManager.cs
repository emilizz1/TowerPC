using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class MarketCardManager : MonoSingleton<MarketCardManager>
{
    [SerializeField] List<MarketCardDisplay> marketCardDisplays;
    public int basePrice;
    public CardHolder marketCards;

    [SerializeField] int baseMarketSize;
    public int baseForgeSize;
    public int baseGraveyardSize;
    
    internal List<MarketDeck> marketDecks;
    bool noPrice;

    List<Card> baseMarketCards;
    List<Card> firstMarketCards;
    List<Card> secondMarketCards;
    List<Card> playerCards = new List<Card>();

    void Start()
    {
        marketDecks = new List<MarketDeck>();
        for (int i = 0; i < 5; i++)
        {
            marketDecks.Add(new MarketDeck());
            marketDecks[i].CreateNewDeck();
        }

        baseMarketCards = new List<Card>();
        foreach(Card card in GetMarketCards(ProgressManager.GetLevel("Base"), marketCards))
        {
            baseMarketCards.Add(Instantiate(card));
        }
        firstMarketCards = new List<Card>();

        foreach (Card card in GetMarketCards(ProgressManager.GetLevel(CharacterSelector.firstCharacter.characterName), CharacterSelector.firstCharacter.marketCards))
        {
            firstMarketCards.Add(Instantiate(card));
        }
        secondMarketCards = new List<Card>();
        foreach (Card card in GetMarketCards(ProgressManager.GetLevel(CharacterSelector.secondCharacter.characterName), CharacterSelector.secondCharacter.marketCards))
        {
            secondMarketCards.Add(Instantiate(card));
        }

        for (int i = 0; i < marketDecks.Count; i++)
        {
            CreateStartingMarket(i);
        }
    }


    List<Card> GetMarketCards(int level, CardHolder cardHolder)
    {
        List<Card> finalList = new List<Card>();
        foreach(CardHolder.CardHolderCollection collection in cardHolder.cardsCollection)
        {
            if(collection.levelFrom <= level)
            {
                finalList = collection.cards;
            } 
        }

        return finalList;
    }

    void CreateStartingMarket(int deckIndex)
    {
        marketDecks[deckIndex].CreateNewDeck();

        foreach (Card card in baseMarketCards)
        {
            Card cardToAdd = Instantiate( card);
            if (card.cardType == CardType.Action && card.cardLevel < 2 && deckIndex == 0)
            {
                marketDecks[0].AddCard(cardToAdd);
            }
            if (card.cardType == CardType.Spell && card.cardLevel < 2 && deckIndex == 1)
            {
                marketDecks[1].AddCard(cardToAdd);
            }
            if (card.cardType == CardType.Tower && card.cardLevel < 2 && deckIndex == 2)
            {
                marketDecks[2].AddCard(cardToAdd);
            }
            if (card.cardLevel == 2 && deckIndex == 3)
            {
                marketDecks[3].AddCard(cardToAdd);
            }
            if (card.cardLevel == 3 && deckIndex == 4)
            {
                marketDecks[4].AddCard(cardToAdd);
            }
        }

        foreach (Card card in firstMarketCards)
        {
            Card cardToAdd = Instantiate(card);
            if (card.cardType == CardType.Action && card.cardLevel < 2 && deckIndex == 0)
            {
                marketDecks[0].AddCard(cardToAdd);
            }
            if (card.cardType == CardType.Spell && card.cardLevel < 2 && deckIndex == 1)
            {
                marketDecks[1].AddCard(cardToAdd);
            }
            if (card.cardType == CardType.Tower && card.cardLevel < 2 && deckIndex == 2)
            {
                marketDecks[2].AddCard(cardToAdd);
            }
            if(card.cardLevel == 2 && deckIndex == 3)
            {
                marketDecks[3].AddCard(cardToAdd);
            }
            if(card.cardLevel == 3 && deckIndex == 4)
            {
                marketDecks[4].AddCard(cardToAdd);
            }
        }

        foreach (Card card in secondMarketCards)
        {
            Card cardToAdd = Instantiate(card);
            if (card.cardType == CardType.Action && card.cardLevel < 2 && deckIndex == 0)
            {
                marketDecks[0].AddCard(cardToAdd);
            }
            if (card.cardType == CardType.Spell && card.cardLevel < 2 && deckIndex == 1)
            {
                marketDecks[1].AddCard(cardToAdd);
            }
            if (card.cardType == CardType.Tower && card.cardLevel < 2 && deckIndex == 2)
            {
                marketDecks[2].AddCard(cardToAdd);
            }
            if (card.cardLevel == 2 && deckIndex == 3)
            {
                marketDecks[3].AddCard(cardToAdd);
            }
            if (card.cardLevel == 3 && deckIndex == 4)
            {
                marketDecks[4].AddCard(cardToAdd);
            }
        }
    }

    public void DisplayNewMarket()
    {
        for (int i = 0; i < marketCardDisplays.Count; i++)
        {
            if (i < baseMarketSize)
            {
                marketCardDisplays[i].gameObject.SetActive(true);
                if (marketDecks[i].deck.Count == 0)
                {
                    if (marketDecks[i].discard.Count == 0)
                    {
                        CreateStartingMarket(i);
                    }
                    marketDecks[i].ShuffleDeck();
                }
                Card cardToDisplay = marketDecks[i].deck[UnityEngine.Random.Range(0, marketDecks[i].deck.Count)];
                marketCardDisplays[i].DisplayMarketCard(cardToDisplay, noPrice);
                marketDecks[i].RemoveCard(cardToDisplay);
            }
            else
            {
                marketCardDisplays[i].gameObject.SetActive(false);
            }
        }
    }

    public void DisplayNewForge()
    {
        playerCards = GatherAllPlayerCards();
        for (int i = 0; i < marketCardDisplays.Count; i++)
        {
            if (i < baseForgeSize)
            {
                marketCardDisplays[i].gameObject.SetActive(true);

                Card cardToDisplay = playerCards[UnityEngine.Random.Range(0, playerCards.Count)];
                playerCards.Remove(cardToDisplay);

                marketCardDisplays[i].DisplayForgeCard(cardToDisplay);
            }
            else
            {
                marketCardDisplays[i].gameObject.SetActive(false);
            }
        }
    }

    public void DisplayNewGraveyard()
    {
        playerCards = GatherAllPlayerCards();
        for (int i = 0; i < marketCardDisplays.Count; i++)
        {
            if (i < baseGraveyardSize)
            {
                marketCardDisplays[i].gameObject.SetActive(true);

                Card cardToDisplay = playerCards[UnityEngine.Random.Range(0, playerCards.Count)];
                playerCards.Remove(cardToDisplay);

                marketCardDisplays[i].DisplayGraveyardCard(cardToDisplay);
            }
            else
            {
                marketCardDisplays[i].gameObject.SetActive(false);
            }
        }
    }

    List<Card> GatherAllPlayerCards()
    {
        List<Card> playerCards = new List<Card>();

        foreach(Card card in Deck.instance.deckCards)
        {
            if (card.cardLevel < 3)
            {
                playerCards.Add(card);
            }
        }

        foreach (Card card in Discard.instance.discardCards)
        {
            if (card.cardLevel < 3)
            {
                playerCards.Add(card);
            }
        }

        return playerCards;
    }

    public void CloseMarket()
    {
        for (int i = 0; i < marketCardDisplays.Count; i++)
        {
            if (marketCardDisplays[i].gameObject.activeSelf) {
                if (MarketWindow.instance.market)
                {
                    if (i < baseMarketSize)
                    {
                        marketCardDisplays[i].DiscardMarketCard(i);
                    }
                }
                else if (MarketWindow.instance.forge)
                {
                    if (i < baseForgeSize)
                    {
                        marketCardDisplays[i].DiscardForgeCard();
                    }
                }
                else
                {
                    if (i < baseForgeSize)
                    {
                        marketCardDisplays[i].DiscardGraveyardCard();
                    }
                }
            }
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

        baseMarketSize = 2;

        noPrice = true;
    }

    public void RecheckColors()
    {
        for (int i = 0; i < marketCardDisplays.Count; i++)
        {
            if (MarketWindow.instance.market)
            {
                if (i < baseMarketSize)
                {
                    marketCardDisplays[i].CheckPriceColor();
                }
            }
        }
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
