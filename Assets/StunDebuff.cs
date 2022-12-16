using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunDebuff : Debuff
{
    const string DEBUFF_NAME = "Stun";

    internal override void Start()
    {
        info = DebuffController.instance.GetInfo(DEBUFF_NAME);
        base.Start();
    }

    internal override void ApplyDebuff()
    {
        base.ApplyDebuff();
        myEnemy.movement.additionalSpeed -= myEnemy.movement.movementSpeed;
        myEnemy.debuffIcons.AddNewIcon(info.icon);
    }

    internal override void RemoveDebuff()
    {
        base.RemoveDebuff();
        myEnemy.movement.additionalSpeed += myEnemy.movement.movementSpeed;
        myEnemy.debuffIcons.RemoveIcon(info.icon);
    }

}
