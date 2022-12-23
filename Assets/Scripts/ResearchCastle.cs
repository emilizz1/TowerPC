using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Research/CastleHealths")]
[Serializable]
public class ResearchCastle : Research
{
    public int maxHealthAddition;
    public int increaseRegen;
    public int enemyDamageStoppedCount;

    public override void Researched()
    {
        base.Researched();
        if (maxHealthAddition > 0)
        {
            PlayerLife.instance.MaxHealthAddition(maxHealthAddition);
        }

        if (increaseRegen > 0)
        {
            PlayerLife.instance.regen += increaseRegen;
        }

        if (enemyDamageStoppedCount > 0)
        {
            PlayerLife.instance.IncreaseIgnorAmount ( enemyDamageStoppedCount);
        }
    }
}
