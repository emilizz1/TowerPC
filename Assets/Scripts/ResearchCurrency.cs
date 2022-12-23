using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Research/Currency")]
[Serializable]

public class ResearchCurrency : Research
{
    [SerializeField] int manaRegenPerSecond;
    [SerializeField] int maxMana;
    [SerializeField] int incomeIncreased;
    [SerializeField] int addMoney;

    public override void Researched()
    {
        base.Researched();
        if (manaRegenPerSecond > 0)
        {
            Mana.instance.AddRegen(manaRegenPerSecond);
        }
        else  if(incomeIncreased > 0)
        {
            Mana.instance.IncreaseMaxMana(maxMana);
        }
        else if (incomeIncreased > 0)
        {
            Money.instance.AddToPassiveIncome(incomeIncreased);
        }
        else if (addMoney > 0)
        {
            Money.instance.AddCurrency(addMoney, false);
        }
    }
}
