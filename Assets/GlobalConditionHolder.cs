using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalConditionHolder 
{
    public static bool interest;
    public static bool firstHitDoubleDamage;
    public static bool increasedHealth;
    public static bool additionalDuration;
    public static bool doubleImrpovementSpells;
    public static bool additionalCoinReward;
    public static bool firstAction;
    public static bool coinsFromResearch;
    public static bool poisonKills;
    public static bool terrainBonusesIncreased;
    public static bool doublePoisonDamage;
    public static bool enemyDamage;
    public static bool showUpcomingEnemies;
    public static bool positiveEvents;
    public static bool banditsAppear;
    public static bool noGold;
    public static bool poisonDisabled;
    public static bool goldForSpells;
    public static bool goldenCharm;
    public static bool towerTax;
    public static bool slowDisabled;
    public static bool upgradedGoldenCharm;
    public static bool fungus;
    public static bool waterTiles;
    public static bool spikyPlant;
    public static bool maxHP;
    
    public static void Reset()
    {
        interest = false;
        firstHitDoubleDamage = false;
        increasedHealth = false;
        additionalDuration = false;
        doubleImrpovementSpells = false;
        additionalCoinReward = false;
        firstAction = false;
        coinsFromResearch = false;
        poisonKills = false;
        terrainBonusesIncreased = false;
        doublePoisonDamage = false;
        enemyDamage = false;
        showUpcomingEnemies = false;
        positiveEvents = false;
        banditsAppear = false;
        noGold = false;
        poisonDisabled = false;
        goldForSpells = false;
        goldenCharm = false;
        towerTax = false;
        slowDisabled = false;
        upgradedGoldenCharm = false;
        fungus = false;
        waterTiles = false;
        spikyPlant = false;
        maxHP = false;
    }
}
