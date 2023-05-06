using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyBullet : Bullet
{
    internal override void TargetReached()
    {
        if (SecondTowerAbilityManager.instance.SecondSpecialUnlocked(TowerType.Energy) == 2)
        {
            if (target != null)
            {
                if (target.GetComponent<PoisonDebuff>())
                {
                    target.GetComponent<PoisonDebuff>().AddStack();
                }
                else
                {
                    target.gameObject.AddComponent<PoisonDebuff>();
                }
            }
        }
        base.TargetReached();
    }
}
