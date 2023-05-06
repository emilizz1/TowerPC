using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalBullet : Bullet
{
    internal override void TargetReached()
    {
        if(SecondTowerAbilityManager.instance.SecondSpecialUnlocked(TowerType.Crystal) == 2) {
            if (target != null)
            {
                if (target.GetComponent<SlowDebuff>())
                {
                    target.GetComponent<SlowDebuff>().AddStack();
                }
                else
                {
                    target.gameObject.AddComponent<SlowDebuff>();
                }
            }
        }
        base.TargetReached();
    }
}
