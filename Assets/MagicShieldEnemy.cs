using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicShieldEnemy : Enemy
{
    public override void DealDamage(List<float> damages, Color damageColor, int additionalGoldOnKill = 0)
    {
        for (int i = 0; i < 3; i++)
        {
            damages[i] = Mathf.Min(damages[i], 1f);
        }
        base.DealDamage(damages, damageColor, additionalGoldOnKill);
    }
}
