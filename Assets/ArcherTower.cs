using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherTower : Tower
{
    public override void Activate()
    {
        base.Activate();
        if(SecondTowerAbilityManager.instance.SecondSpecialUnlocked(towerType) == 1)
        {
            for (int i = 0; i < 3; i++)
            {
                TowerPlacer.castleTower.statsMultiplayers.damage[i] += 0.05f;
            }
        }
        else if(SecondTowerAbilityManager.instance.SecondSpecialUnlocked(towerType) == 2)
        {
            TowerStats newStats = new TowerStats();

                newStats.range += 0.02f;
            
            PasiveTowerStatsController.AddAdditionalPasiveStats(DamageTypes.All, newStats);
        }
    }
}
