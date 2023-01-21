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
    
    public void DisplayCard(Card card, bool noPrice)
    {
        myCard = card;
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
            MarketCardManager.instance.RecheckColors();
            SoundsController.instance.PlayOneShot("Buy");
            Discard.instance.DiscardCardFromHand(cardDisplay);
            priceText.transform.parent.gameObject.SetActive(false);
        }
    }

    public void DiscardMarketCard(int index)
    {
        MarketCardManager.instance.marketDecks[index].DiscardCard(myCard);
    }
}
