using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondTowerAbilityManager : MonoSingleton<SecondTowerAbilityManager>
{
    Dictionary<TowerType, int> secondAbilityUnlocks;

    internal int spearTowersPlaced;

    private void Start()
    {
        secondAbilityUnlocks = new Dictionary<TowerType, int>();
        secondAbilityUnlocks.Add(TowerType.Archer, 0);
        secondAbilityUnlocks.Add(TowerType.Castle, 0);
        secondAbilityUnlocks.Add(TowerType.Arcane, 0);
        secondAbilityUnlocks.Add(TowerType.Spear, 0);
        secondAbilityUnlocks.Add(TowerType.Crystal, 0);
        secondAbilityUnlocks.Add(TowerType.Energy, 0);
        secondAbilityUnlocks.Add(TowerType.Dart, 0);
        secondAbilityUnlocks.Add(TowerType.Cannon, 0);
        secondAbilityUnlocks.Add(TowerType.Naval, 0);
    }

    public int SecondSpecialUnlocked(TowerType type)
    {
        return secondAbilityUnlocks[type];
    }

    public void ResearchFinished(TowerType type, int outcome)
    {
        secondAbilityUnlocks[type] = outcome;

        if(type == TowerType.Archer)
        {
            ArcherUnlocked(outcome);
        }

    }

    private static void ArcherUnlocked(int outcome)
    {
        int archerTowers = 0;
        foreach (Tower tower in TowerPlacer.allTowers)
        {
            if (tower.towerType == TowerType.Archer)
            {
                archerTowers++;
            }
        }
        if (outcome == 1)
        {
            for (int i = 0; i < 3; i++)
            {
                TowerPlacer.castleTower.statsMultiplayers.damage[i] += (0.05f * archerTowers);
            }
        }
        else if (outcome == 2)
        {
            TowerStats newStats = new TowerStats();

            newStats.range += (0.02f * archerTowers);
            PasiveTowerStatsController.AddAdditionalPasiveStats(DamageTypes.All, newStats);
        }
    }
}

public enum TowerType
{
    Archer,
    Castle,
    Arcane,
    Spear,
    Crystal,
    Energy,
    Dart,
    Cannon,
    Naval
}
