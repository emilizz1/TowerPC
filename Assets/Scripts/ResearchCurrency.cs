using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Research/Currency")]
[Serializable]

public class ResearchCurrency : Research
{
    [SerializeField] int manaRegenPerSecond;

    public override void Researched()
    {
        base.Researched();
        Mana.instance.AddRegen( manaRegenPerSecond);
    }
}
