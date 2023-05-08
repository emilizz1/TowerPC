using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using I2.Loc;
using System;

[CreateAssetMenu(menuName = "Tower Card")]
[Serializable]
public class TowerCard : Card
{
    public GameObject towerPrefab;
    public TowerType towerType;
    public LocalizedString firstAbilityText;
    public LocalizedString secondAbilityText;
    public string firstAbility;
    public string secondAbility;

    public override string GetDescription()
    {
        Tower myTower = towerPrefab.GetComponent<Tower>();
        myTower.PrepareTower(null);
        List<float> damage = myTower.GetDamageMultiplied();
        float range = myTower.towerStats[myTower.currentLevel].range * myTower.statsMultiplayers.range;
        string statsText = "<sprite=4> " + damage[0] + " <sprite=3> " + damage[1] + " <sprite=2> " + damage[2] +
            " <sprite =1> " + myTower.GetTimeToNextShot() + " / s < sprite = 0 > " + range + "/n";

        if(SecondTowerAbilityManager.instance.SecondSpecialUnlocked(myTower.towerType) == 1)
        {
            return statsText + firstAbilityText;
        }
        else if(SecondTowerAbilityManager.instance.SecondSpecialUnlocked(myTower.towerType) == 2)
        {
            return statsText + secondAbilityText;
        }
        return statsText + base.GetDescription();
    }
}
