using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DartBullet : Bullet
{
    internal override void TargetReached()
    {
        base.TargetReached();
        if(target != null)
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
}
