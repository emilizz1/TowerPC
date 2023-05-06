using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using I2.Loc;
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
    [TextArea(10, 10)]
    [SerializeField] LocalizedString alternativeDescription;

    //TODO think of moving these to different classes
    public virtual void PlayAction()
    {
        if (addMoney > 0)
        {
            if (GlobalConditionHolder.additionalCoinReward)
            {
                Money.instance.AddCurrency(Mathf.RoundToInt(addMoney * 1.4f), false);
                SoundsController.instance.PlayOneShot("Money");
                AchievementManager.GoldGotFromCoins(Mathf.RoundToInt(addMoney * 1.4f));
                return;
            }
            Money.instance.AddCurrency(addMoney, false);
            SoundsController.instance.PlayOneShot("Money");
            AchievementManager.GoldGotFromCoins(addMoney);
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


        TurnController.actionsPlayed++;
        if (GlobalConditionHolder.firstAction && TurnController.actionsPlayed == 1)
        {
            PlayAction();
        }
    }

    public virtual bool CanItBePlayed()
    {
        return true;
    }

    public override string GetDescription()
    {

        if (addMoney > 0)
        {
            if (GlobalConditionHolder.additionalCoinReward)
            {
                return alternativeDescription;
            }
        }
                return base.GetDescription();
    }
}
