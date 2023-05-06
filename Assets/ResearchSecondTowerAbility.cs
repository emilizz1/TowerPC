using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Research/SecondTowerAbility")]
[Serializable]
public class ResearchSecondTowerAbility : Research
{
    [SerializeField] TowerType towerType;
    [SerializeField] int outcome;

    public override void Researched()
    {
        base.Researched();

        SecondTowerAbilityManager.instance.ResearchFinished(towerType, outcome);
    }
}
