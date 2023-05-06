using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DartTower : Tower
{
    public override void PrepareTower(Spot spot)
    {
        base.PrepareTower(spot);
        if(SecondTowerAbilityManager.instance.SecondSpecialUnlocked(towerType) == 1)
        {
            foreach(TerrainBonus terrain in spot.terrainBonus)
            {
                if(terrain.type == TerrainBonus.TerrainType.forest)
                {
                    TowerStats doubleDamage = new TowerStats();
                    doubleDamage.damage[0] += towerStats[currentLevel].damage[0];
                    doubleDamage.damage[1] += towerStats[currentLevel].damage[1];
                    doubleDamage.damage[2] += towerStats[currentLevel].damage[2];
                    statsMultiplayers.CombineStats(doubleDamage);
                }
            }
        }
    }

    internal override List<float> GetDamageMultiplied()
    {
        List<float> baseDamage = base.GetDamageMultiplied();
        if (SecondTowerAbilityManager.instance.SecondSpecialUnlocked(towerType) == 2)
        {
            if (currentTarget.GetComponent<PoisonDebuff>())
            {
                baseDamage[0] *= 1.5f;
                baseDamage[1] *= 1.5f;
                baseDamage[2] *= 1.5f;
            }
        }
        return baseDamage;
    }
}
