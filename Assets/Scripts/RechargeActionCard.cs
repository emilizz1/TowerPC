using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Recharge Action Card")]
[Serializable]
public class RechargeActionCard : ActionCard
{
    [SerializeField] int rechargeAmount;

    public override void PlayAction()
    {
        
        CardDisplay selectedCard = HandCardSlotController.instance.GetRandomDisplay(HandCardSlotController.instance.GetDisplayByCard(this));
        selectedCard.FlashCard();
        selectedCard.displayedCard.maxUses += rechargeAmount;
        selectedCard.DisplayCard(selectedCard.displayedCard);
        SoundsController.instance.PlayOneShot("Blessing");
    }

    public override bool CanItBePlayed()
    {
        return Hand.instance.handCards.Count >1;
    }
}
