using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearTower : Tower
{
    [SerializeField] List<float> firerateIncreasesPerShot;

    float timesHit;

    GameObject prevTarget;

    internal override float GetTimeToNextShot()
    {
        float extraFireRate = 1f;
        for (int i = 0; i < timesHit; i++)
        {
            extraFireRate *=  firerateIncreasesPerShot[currentLevel];
        }
        return base.GetTimeToNextShot() * extraFireRate * (SecondTowerAbilityManager.instance.spearTowersPlaced *0.02f);
    }

    internal override void Shoot()
    {
        base.Shoot();
        if(currentTarget == prevTarget)
        {
            timesHit++;
        }
        else
        {
            prevTarget = currentTarget;
            timesHit = 0;
        }
    }

    public override void Activate()
    {
        base.Activate();
        if (SecondTowerAbilityManager.instance.SecondSpecialUnlocked(towerType) == 1)
        {
                TowerPlacer.castleTower.statsMultiplayers.fireRate += 0.04f;
        }
        else if (SecondTowerAbilityManager.instance.SecondSpecialUnlocked(towerType) == 2)
        {
            SecondTowerAbilityManager.instance.spearTowersPlaced++;
        }
    }

}
