using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBuff : Buff
{
    const string BUFF_NAME = "Speed";

    internal override void Start()
    {
        info = BuffController.instance.GetInfo(BUFF_NAME);
        base.Start();
    }

    internal override void ApplyBuff()
    {
        base.ApplyBuff();
        myTower.statsMultiplayers.fireRate += info.effectAmount;
    }

    internal override void RemoveBuff()
    {
        base.RemoveBuff();
        myTower.statsMultiplayers.fireRate -= info.effectAmount;
        stacks--;

        if(stacks == 0)
        {
            Destroy(this);
        }

    }
}
