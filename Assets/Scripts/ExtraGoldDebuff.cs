using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraGoldDebuff : Debuff
{
    const string DEBUFF_NAME = "Gold";

    internal override void Start()
    {
        info = DebuffController.instance.GetInfo(DEBUFF_NAME);
        base.Start();
    }

    internal override void ApplyDebuff()
    {
        base.ApplyDebuff();
        myEnemy.moneyOnKill += (int)info.effectAmount;
        myEnemy.debuffIcons.AddNewIcon(info.icon);
    }

    internal override void RemoveDebuff()
    {
        base.RemoveDebuff();
        myEnemy.moneyOnKill -= (int)info.effectAmount;
        myEnemy.debuffIcons.RemoveIcon(info.icon);
    }
}
