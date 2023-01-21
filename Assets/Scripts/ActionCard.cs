using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

[CreateAssetMenu(menuName = "Action Card")]
[Serializable]
public class ActionCard : Card
{
    [SerializeField] int addMoney;
    [SerializeField] int addMana;
    [SerializeField] int addHealth;
    [SerializeField] int drawCards;
    [SerializeField] int advanceResearch;
    [SerializeField] int discount;

    //TODO think of moving these to different classes
    public virtual void PlayAction()
    {
        if (addMoney > 0)
        {
            Money.instance.AddCurrency(addMoney, false);
            SoundsController.instance.PlayOneShot("Money");
        }
        if (addMana > 0)
        {
            Mana.instance.AddCurrency(addMana, false);
            SoundsController.instance.PlayOneShot("Mana");
        }
        if (addHealth > 0)
        {
            PlayerLife.instance.ChangeHealthAmount(addHealth);
        }
        if(drawCards > 0)
        {
            Hand.instance.DrawNewCards(drawCards);
        }
        if (advanceResearch > 0)
        {
            for (int i = 0; i < advanceResearch; i++)
            {
                ResearchWindow.instance.AdvanceResearch();
                ResearchWindow.instance.IfNoResearchSelectedOpen();
            }
        }
        if(discount > 0)
        {
            CostController.currentTurnDiscount += discount;
            HandCardSlotController.instance.UpdateCards();
        }
    }

    public virtual bool CanItBePlayed()
    {
        return true;
    }
}
