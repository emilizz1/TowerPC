using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Research/ChargesMultiplayer")]
[Serializable]
public class ResearchChargesMultiplayer : Research
{
    [SerializeField] CardType type;
    [SerializeField] int amount;
    [SerializeField] ChargesUses usedFor;

    [Serializable]
    public enum ChargesUses
    {
        all,
        deck
    }

    public override void Researched()
    {
        base.Researched();

        if (usedFor == ChargesUses.all)
        {
            ChargesController.AddNewChargesAddition(type, amount);
        }

        if (usedFor == ChargesUses.deck)
        {
            foreach (Card card in Deck.instance.deckCards)
            {
                card.maxUses += amount;
            }
            foreach (Card card in Discard.instance.discardCards)
            {
                card.maxUses += amount;
            }
            foreach (Card card in Hand.instance.handCards)
            {
                card.maxUses += amount;
            }
        }
    }

}
