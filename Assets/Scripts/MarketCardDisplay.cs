using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MarketCardDisplay : MonoBehaviour
{
    [SerializeField] CardDisplay cardDisplay;
    [SerializeField] TextMeshProUGUI priceText;
    [SerializeField] float spotCostMultiplayer;

    int price;
    Card myCard;
    bool deckCard;
    bool bought;
    
    public void DisplayMarketCard(Card card, bool noPrice)
    {
        bought = false;
        myCard = Instantiate(card);
        cardDisplay.gameObject.SetActive(true);
        cardDisplay.transform.SetParent(transform);
        cardDisplay.transform.localPosition = new Vector3(0f, 120f, 0f);
        cardDisplay.DisplayCard(card);
        price = noPrice ? 0 : Mathf.CeilToInt(card.buyCostMultiplayer * (MarketCardManager.instance.basePrice + TurnController.currentTurn) *
            spotCostMultiplayer * CostController.GetBuyingCostMultiplayer(card.cardType));
        priceText.text = price.ToString();
        priceText.transform.parent.gameObject.SetActive(true);

        CheckPriceColor();
    }

    public  void DisplayForgeCard(Card card)
    {
        bought = false;
        myCard = card;
        cardDisplay.gameObject.SetActive(true);
        cardDisplay.transform.SetParent(transform);
        deckCard = Deck.instance.deckCards.Contains(card);
        cardDisplay.transform.position = deckCard ? Deck.instance.deckTransform.position :  Discard.instance.discardTransform.position;
        LeanTween.move(cardDisplay.gameObject, new Vector3(0f, 120f, 0f), 0.25f);
        LeanTween.rotate(cardDisplay.gameObject, Vector3.zero, 0.25f);
        cardDisplay.DisplayCard(card);
        price = 0;
        priceText.text = price.ToString();
        priceText.transform.parent.gameObject.SetActive(true);


        if (deckCard)
        {
            Deck.instance.deckCards.Remove(card);
        }
        else
        {
            Discard.instance.discardCards.Remove(card);
        }

        CheckPriceColor();

    }

    public void DisplayGraveyardCard(Card card)
    {
        bought = false;
        myCard = card;
        cardDisplay.gameObject.SetActive(true);
        cardDisplay.transform.SetParent(transform);
        deckCard = Deck.instance.deckCards.Contains(card);
        cardDisplay.transform.position = deckCard ? Deck.instance.deckTransform.position : Discard.instance.discardTransform.position;
        LeanTween.move(cardDisplay.gameObject, new Vector3(0f, 120f, 0f), 0.25f);
        LeanTween.rotate(cardDisplay.gameObject, Vector3.zero, 0.25f);
        cardDisplay.DisplayCard(card);
        price = 0;
        priceText.text = price.ToString();
        priceText.transform.parent.gameObject.SetActive(true);


        if (deckCard)
        {
            Deck.instance.deckCards.Remove(card);
        }
        else
        {
            Discard.instance.discardCards.Remove(card);
        }

        CheckPriceColor();
    }

    public void CheckPriceColor()
    {
        if (price > Money.instance.currentAmount)
        {
            priceText.color = Color.gray;
        }
        else
        {
            priceText.color = Color.white;
        }
    }

    public void BuyCard()
    {
        if (Money.instance.TryPaying(price))
        {
            bought = true;
            MarketCardManager.instance.RecheckColors();
            if (MarketWindow.instance.market)
            {
                SoundsController.instance.PlayOneShot("Buy");
                Discard.instance.DiscardCardFromHand(cardDisplay);
            }
            else if (MarketWindow.instance.forge)
            {
                SoundsController.instance.PlayOneShot("Buy");
                Deck.instance.AddCard(GetUpgradedCard(myCard));
                LeanTween.move(cardDisplay.gameObject, Deck.instance.deckTransform, 0.25f);
            }
            else if (MarketWindow.instance.graveyard)
            {
                cardDisplay.DestroyCard();
            }
            priceText.transform.parent.gameObject.SetActive(false);
        }
    }

    Card GetUpgradedCard(Card cardToUpgrade)
    {
        foreach(Card card in MarketWindow.instance.allCards.cardsCollection[0].cards)
        {
            if(card.cardName == cardToUpgrade.cardName)
            {
                if(card.cardLevel == cardToUpgrade.cardLevel + 1)
                {
                    return Instantiate(card);
                }
            }
        }
        return null;
    }

    public void DiscardMarketCard(int index)
    {
        if (!bought)
        {
            MarketCardManager.instance.marketDecks[index].DiscardCard(myCard);
        }
    }

    public void DiscardForgeCard()
    {
        if (!bought)
        {
            LeanTween.move(cardDisplay.gameObject, deckCard ? Deck.instance.deckTransform : Discard.instance.discardTransform, 0.25f);
            if (deckCard)
            {
                Deck.instance.AddCard(myCard);
            }
            else
            {
                Discard.instance.AddCard(myCard);
            }
        } 
    }

    public void DiscardGraveyardCard()
    {
        if (!bought)
        {
            LeanTween.move(cardDisplay.gameObject, deckCard ? Deck.instance.deckTransform : Discard.instance.discardTransform, 0.25f);
            if (deckCard)
            {
                Deck.instance.AddCard(myCard);
            }
            else
            {
                Discard.instance.AddCard(myCard);
            }
        }
    }
}
