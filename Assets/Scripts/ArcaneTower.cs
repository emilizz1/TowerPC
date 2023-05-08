using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcaneTower : Tower
{
    bool secondAbilityApplied;

    internal override List<float> GetDamageMultiplied()
    {
        if (SecondTowerAbilityManager.instance.SecondSpecialUnlocked(towerType) == 2)
        {
            if (!secondAbilityApplied)
            {
                secondAbilityApplied = true;
                TowerStats newStats = new TowerStats();
                newStats.damage[0] = 1;
                newStats.damage[1] = 1;
                newStats.damage[2] = 1;
                newStats.fireRate = -0.2f;
                statsMultiplayers.CombineStats(newStats);
            }
        }
        List<float> normal = base.GetDamageMultiplied();

        if(currentTarget != null) 
        {
            if (currentTarget.GetComponent<Debuff>())
            {
                for (int i = 0; i < normal.Count; i++)
                {
                    if (SecondTowerAbilityManager.instance.SecondSpecialUnlocked(towerType) == 1)
                    {
                        normal[i] *= 3;
                    }
                    else
                    {
                        normal[i] *= 2;
                    }
                }
            }
        }
        return normal;
    }

    internal override float GetTimeToNextShot()
    {
        if (SecondTowerAbilityManager.instance.SecondSpecialUnlocked(towerType) == 2)
        {
            if (!secondAbilityApplied)
            {
                secondAbilityApplied = true;
                TowerStats newStats = new TowerStats();
                newStats.damage[0] = 1;
                newStats.damage[1] = 1;
                newStats.damage[2] = 1;
                newStats.fireRate = -0.2f;
                statsMultiplayers.CombineStats(newStats);
            }
        }
        return base.GetTimeToNextShot();
    }
}
