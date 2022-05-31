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
    
    public void DisplayCard(Card card)
    {
        cardDisplay.transform.parent = transform;
        cardDisplay.transform.position = new Vector3(0f, 120f, 0f);
        cardDisplay.DisplayCard(card);
        float truePrice = card.buyCostMultiplayer * MarketCardManager.instance.basePrice * spotCostMultiplayer;
        price = Mathf.FloorToInt( truePrice - (truePrice % 10));
        priceText.text = price.ToString();
    }

    public void BuyCard()
    {
        if (Money.instance.TryPaying(price))
        {
            Discard.instance.DiscardCardFromHand(cardDisplay);
        }
    }

    public void DiscardMarketCard()
    {
        if (gameObject.activeSelf)
        {
            MarketCardManager.instance.marketDiscard.Add(cardDisplay.displayedCard);
        }
    }
}
