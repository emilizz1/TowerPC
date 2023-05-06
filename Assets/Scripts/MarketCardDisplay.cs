using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class MarketCardDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] CardDisplay cardDisplay;
    [SerializeField] TextMeshProUGUI priceText;
    [SerializeField] float spotCostMultiplayer;
    [SerializeField] GameObject xIcon;
    [SerializeField] List<float> cardPriceMultiplayerByTier;

    int price;
    Card myCard;
    Card myUpgradedCard;
    bool deckCard;
    bool bought;
    
    public void DisplayMarketCard(Card card, bool noPrice)
    {
        bought = false;
        myCard = Instantiate(card);
        cardDisplay.gameObject.SetActive(true);
        cardDisplay.transform.SetParent(transform);
        cardDisplay.transform.rotation = Quaternion.identity;
        cardDisplay.transform.localPosition = new Vector3(0f, 120f, 0f);
        cardDisplay.DisplayCard(card);
        price = noPrice ? 0 : Mathf.CeilToInt(cardPriceMultiplayerByTier[card.cardTier] * (MarketCardManager.instance.basePrice + TurnController.currentTurn) *
            spotCostMultiplayer * CostController.GetMarketBuyingCostMultiplayer(card.cardType));
        priceText.text = price.ToString();
        priceText.transform.parent.gameObject.SetActive(true);

        myUpgradedCard = null;
        xIcon.SetActive(false);

        CheckPriceColor();
    }

    public  void DisplayForgeCard(Card card)
    {
        bought = false;
        myCard = card;
        cardDisplay.gameObject.SetActive(true);
        cardDisplay.transform.SetParent(transform);
        cardDisplay.transform.rotation = Quaternion.identity;
        deckCard = Deck.instance.deckCards.Contains(card);
        cardDisplay.transform.position = deckCard ? Deck.instance.deckTransform.position :  Discard.instance.discardTransform.position;
        LeanTween.moveLocal(cardDisplay.gameObject, new Vector3(0f, 120f, 0f), 0.5f);
        cardDisplay.DisplayCard(card);
        price = Mathf.CeilToInt(cardPriceMultiplayerByTier[card.cardTier] * (MarketCardManager.instance.basePrice + TurnController.currentTurn) *
            spotCostMultiplayer * CostController.GetForgeBuyingCostMultiplayer(card.cardType));
        priceText.text = price.ToString();
        priceText.transform.parent.gameObject.SetActive(true);

        myUpgradedCard = CardHolderManager.instance.GetUpgradedCard(myCard);
        xIcon.SetActive(false);


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
        cardDisplay.transform.rotation = Quaternion.identity;
        deckCard = Deck.instance.deckCards.Contains(card);
        cardDisplay.transform.position = deckCard ? Deck.instance.deckTransform.position : Discard.instance.discardTransform.position;
        LeanTween.moveLocal(cardDisplay.gameObject, new Vector3(0f, 120f, 0f), 0.5f);
        cardDisplay.DisplayCard(card);
        price = Mathf.CeilToInt(cardPriceMultiplayerByTier[card.cardTier] * (MarketCardManager.instance.basePrice + TurnController.currentTurn) *
            spotCostMultiplayer * CostController.GetGraveyardBuyingCostMultiplayer(card.cardType));
        priceText.text = price.ToString();
        priceText.transform.parent.gameObject.SetActive(true);

        myUpgradedCard = null;
        xIcon.SetActive(false);

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

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(myUpgradedCard != null)
        {
            cardDisplay.DisplayCard(myUpgradedCard);
        }
        else if (MarketWindow.instance.graveyard)
        {
            xIcon.SetActive(true);

        }

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (myUpgradedCard != null)
        {
            cardDisplay.DisplayCard( myCard);
        }
        else if (MarketWindow.instance.graveyard)
        {
            xIcon.SetActive(false);

        }
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
                cardDisplay.DisplayCard(myUpgradedCard);
                Deck.instance.AddCard(myUpgradedCard);
                LeanTween.move(cardDisplay.gameObject, Deck.instance.deckTransform, 0.5f).setOnComplete(ResetCard);
            }
            else if (MarketWindow.instance.graveyard)
            {
                cardDisplay.DestroyCard();
            }
            priceText.transform.parent.gameObject.SetActive(false);
            MarketWindow.instance.CheckIfAllCardsUpgraded();
        }
    }

    public void ResetCard()
    {
        cardDisplay.gameObject.SetActive(false);
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
            LeanTween.move(cardDisplay.gameObject, deckCard ? Deck.instance.deckTransform : Discard.instance.discardTransform, 0.5f).setOnComplete(ResetCard);
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
            LeanTween.move(cardDisplay.gameObject, deckCard ? Deck.instance.deckTransform : Discard.instance.discardTransform, 0.5f).setOnComplete(ResetCard);
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
