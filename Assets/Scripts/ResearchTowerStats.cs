using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Research/TowerStats")]
[Serializable]

public class ResearchTowerStats : Research
{
    public List<Tower.TowerTypes> affectedTypes;
    public Tower.TowerStats statMultiplayers;

    public override void Researched()
    {
        base.Researched();
        foreach (Tower.TowerTypes type in affectedTypes)
        {
            PasiveTowerStatsController.AddAdditionalPasiveStats(type, statMultiplayers);
        }
    }
}
