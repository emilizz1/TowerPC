using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmoredEnemy : Enemy
{
    [SerializeField] float damageToIgnore;

    public override void DealDamage(List<float> damages, Color damageColor, int additionalGoldOnKill = 0)
    {

        for (int i = 0; i < 3; i++)
        {
            damages[i] = Mathf.Max(damages[i] - damageToIgnore, 0f);
        }

        base.DealDamage(damages, damageColor, additionalGoldOnKill);
    }
}
