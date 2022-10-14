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

    //TODO think of moving these to different classes
    public virtual void PlayAction()
    {
        if (addMoney > 0)
        {
            Money.instance.AddCurrency(addMoney, false);
        }
        if (addMana > 0)
        {
            Mana.instance.AddCurrency(addMana, false);
        }
        if (addHealth > 0)
        {
            PlayerLife.instance.ChangeHealthAmount(addHealth);
        }
        if(drawCards > 0)
        {
            //TODO think about changing to coroutine on Hand
            for (int i = 0; i < drawCards; i++)
            {
                Hand.instance.DrawCard();
            }
        }
    }
}
