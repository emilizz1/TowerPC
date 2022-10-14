using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Research/CastleHealths")]
[Serializable]
public class ResearchCastle : Research
{
    public int maxHealthMultiplayer;
    public int increaseRegen;

    public override void Researched()
    {
        base.Researched();
        if (maxHealthMultiplayer > 0)
        {
            PlayerLife.instance.MaxHealthMultiplied(maxHealthMultiplayer);
        }

        if (increaseRegen > 0)
        {
            PlayerLife.instance.regen += increaseRegen;
        }
    }
}
