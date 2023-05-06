using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Castle Upgrade Action Card")]
[Serializable]
public class CastleUpgradeAction : ActionCard
{
    [SerializeField] TowerStats addStats;

    public override void PlayAction()
    {
        FindObjectOfType<CastleTower>().statsMultiplayers.CombineStats(addStats);

        TurnController.actionsPlayed++;
        if (GlobalConditionHolder.firstAction && TurnController.actionsPlayed == 1)
        {
            PlayAction();
        }
    }
}
