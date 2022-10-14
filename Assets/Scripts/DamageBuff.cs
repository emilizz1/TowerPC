using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageBuff : Buff
{
    const string BUFF_NAME = "Damage";

    internal override void Start()
    {
        info = BuffController.instance.GetInfo(BUFF_NAME);
        base.Start();
    }

    internal override void ApplyBuff()
    {
        base.ApplyBuff();
        myTower.statsMultiplayers.damage[0] += info.effectAmount;
        myTower.statsMultiplayers.damage[1] += info.effectAmount;
        myTower.statsMultiplayers.damage[2] += info.effectAmount;
    }

    internal override void RemoveBuff()
    {
        base.RemoveBuff();
        myTower.statsMultiplayers.damage[0] -= info.effectAmount;
        myTower.statsMultiplayers.damage[1] -= info.effectAmount;
        myTower.statsMultiplayers.damage[2] -= info.effectAmount;
        stacks--;

        if (stacks == 0)
        {
            Destroy(this);
        }
    }
}
