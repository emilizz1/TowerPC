using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Recharge Action Card")]
[Serializable]
public class UpgradeActionCard : ActionCard
{
    public override void PlayAction()
    {
        
        CardDisplay selectedCard = HandCardSlotController.instance.GetRandomDisplayWithUpgradableCard(HandCardSlotController.instance.GetDisplayByCard(this));

        Hand.instance.handCards.Remove(selectedCard.displayedCard);
        Card newCard = MarketWindow.instance.GetUpgradedCard(selectedCard.displayedCard);
        selectedCard.DisplayCard(newCard);
        Hand.instance.handCards.Add(newCard);

        selectedCard.FlashCard();
        SoundsController.instance.PlayOneShot("Blessing");
    }

    public override bool CanItBePlayed()
    {
        int possibleCards = cardLevel == 3 ? -1 : -2; 
        if (Hand.instance.handCards.Count > 1) 
        {
            foreach (Card card in Hand.instance.handCards)
            {
                if (card.cardLevel  < 3)
                {
                    possibleCards++;
                }
        } 
        }
        return possibleCards >= 0;
    }
}
