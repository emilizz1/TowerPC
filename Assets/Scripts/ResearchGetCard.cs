using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Research/GetCard")]
[Serializable]

public class ResearchGetCard : Research
{
    public Card cardToGet;

    public override void Researched()
    {
        base.Researched();
        CardDisplay cardDisplay = HandCardSlotController.instance.GetDisplay(Hand.instance.handCards.Count);
        cardDisplay.DisplayCard(cardToGet);
        cardDisplay.transform.position = ResearchButton.instance.transform.position;
        Hand.instance.handCards.Add(cardToGet);
        HandCardSlotController.instance.RearrangeCardSlots();
    }
}
