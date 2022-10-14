using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowDebuff : Debuff
{
    const string DEBUFF_NAME = "Slow";

    internal override void Start()
    {
        info = DebuffController.instance.GetInfo(DEBUFF_NAME);
        base.Start();
    }

    internal override void ApplyDebuff()
    {
        base.ApplyDebuff();
        myEnemy.movement.additionalSpeed -= info.effectAmount;
        myEnemy.debuffIcons.AddNewIcon(info.icon);
    }

    internal override void RemoveDebuff()
    {
        base.RemoveDebuff();
        myEnemy.movement.additionalSpeed += info.effectAmount;
        myEnemy.debuffIcons.RemoveIcon(info.icon);
    }
}
