using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Research/TowerStats")]
[Serializable]
public class ResearchTowerStats : Research
{
    public List<DamageTypes> affectedTypes;
    public TowerStats statMultiplayers;

    public override void Researched()
    {
        base.Researched();
        foreach (DamageTypes type in affectedTypes)
        {
            PasiveTowerStatsController.AddAdditionalPasiveStats(type, statMultiplayers);
        }
    }
}
