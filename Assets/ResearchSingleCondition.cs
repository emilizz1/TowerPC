using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Research/SingleCondition")]
[Serializable]
public class ResearchSingleCondition : Research
{
    [SerializeField] SingleResearch singleResearch;

    public override void Researched()
    {
        base.Researched();
        switch (singleResearch)
        {
            case (SingleResearch.secondTower):
                TowerPlacer.SecondCastleTowerUnlocked();
                return;
            case (SingleResearch.doubleBuffs):
                BuffController.instance.DoubleBuffPowerUnlocked();
                GlobalConditionHolder.doubleImrpovementSpells = true;
                return;
            case (SingleResearch.interest):
                GlobalConditionHolder.interest = true;
                return;
            case (SingleResearch.resetNewHandCost):
                Deck.instance.ResetNewHandCost();
                return;
            case (SingleResearch.firstDamageDoubled):
                GlobalConditionHolder.firstHitDoubleDamage = true;
                return;
            case (SingleResearch.moreDefenseAndGold):
                GlobalConditionHolder.increasedHealth = true;
                return;
            case (SingleResearch.noCardBuying):
                MarketCardManager.instance.NoMoreCardBuying();
                return;
            case (SingleResearch.noLevel0Cards):
                MarketCardManager.instance.RemoveLevel0Cards();
                return;
            case (SingleResearch.additionalForgeChoice):
                MarketCardManager.instance.baseForgeSize++;
                return;
            case (SingleResearch.additionalGraveyardChoice):
                MarketCardManager.instance.baseGraveyardSize++;
                return;
            case (SingleResearch.additionalExperience):
                PasiveTowerStatsController.extraExperience ++;
                return;
            case (SingleResearch.additionalDuration):
                GlobalConditionHolder.additionalDuration = true;
                return;
            case (SingleResearch.debuffAdditionalTime):
                DebuffController.instance.AddAdditionalTime();
                return;
            case (SingleResearch.additionalCoinReward):
                GlobalConditionHolder.additionalCoinReward = true;
                return;
            case (SingleResearch.firstAction):
                GlobalConditionHolder.firstAction = true;
                return;
            case (SingleResearch.coinsFromResearch):
                GlobalConditionHolder.coinsFromResearch = true;
                return;
            case (SingleResearch.poisonKills):
                GlobalConditionHolder.poisonKills = true;
                return;
            case (SingleResearch.doublePoisonDamage):
                GlobalConditionHolder.doublePoisonDamage = true;
                return;
            case (SingleResearch.terrainBonusesIncreased):
                GlobalConditionHolder.terrainBonusesIncreased = true;
                TerrainPlacer.instance.DoubleBonuses();
                return;
            case (SingleResearch.doubleTerrain):
                TerrainPlacer.instance.spawnChance += TerrainPlacer.instance.spawnChance;
                return;
            case (SingleResearch.enemyDamage):
                GlobalConditionHolder.enemyDamage = true;
                return;
            case (SingleResearch.showUpcomingEnemies):
                GlobalConditionHolder.showUpcomingEnemies = true;
                return;
            case (SingleResearch.positiveEvents):
                GlobalConditionHolder.positiveEvents = true;
                return;

        }
    }
}

[Serializable]
public enum SingleResearch
{
    secondTower,
    doubleBuffs,
    interest,
    resetNewHandCost,
    firstDamageDoubled,
    moreDefenseAndGold,
    noCardBuying,
    noLevel0Cards,
    additionalForgeChoice,
    additionalGraveyardChoice,
    additionalExperience,
    additionalDuration,
    debuffAdditionalTime,
    additionalCoinReward,
    firstAction,
    coinsFromResearch,
    poisonKills,
    terrainBonusesIncreased,
    doublePoisonDamage,
    doubleTerrain,
    enemyDamage,
    showUpcomingEnemies,
    positiveEvents
}
