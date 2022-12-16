using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBullet : Bullet
{
    internal override void TargetReached()
    {
        base.TargetReached();
        Enemy enemy = target.GetComponent<Enemy>();
        if (enemy.currentHealth[2] == 0 && enemy.currentHealth[1] > 0)
        {
            target.AddComponent<StunDebuff>();
        }
    }
}
