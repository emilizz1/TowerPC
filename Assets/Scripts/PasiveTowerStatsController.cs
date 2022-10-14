using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PasiveTowerStatsController
{
    public static Dictionary<Tower.TowerTypes, Tower.TowerStats> pasiveStats = new Dictionary<Tower.TowerTypes, Tower.TowerStats>();

    public static int extraExperience;

    public static void AddAdditionalPasiveStats(Tower.TowerTypes type, Tower.TowerStats stats)
    {
        AddNewStatToOldTowers(type, stats);

        if (pasiveStats.ContainsKey(type))
        {
            pasiveStats[type].CombineStats(stats);
        }
        else
        {
            pasiveStats.Add(type, stats);
        }
    }

    static void AddNewStatToOldTowers(Tower.TowerTypes type, Tower.TowerStats stats)
    {

        foreach (Tower tower in TowerPlacer.allTowers)
        {
            if (type == Tower.TowerTypes.All || tower.towerTypes.Contains(type))
            {
                tower.statsMultiplayers.CombineStats(stats);
                tower.SetupRange();
            }
        }
    }

    public static Tower.TowerStats GetStats(List<Tower.TowerTypes> types)
    {
        Tower.TowerStats finalStats = new Tower.TowerStats();

        foreach(Tower.TowerTypes type in types)
        {
            if (pasiveStats.ContainsKey(type))
            {
                finalStats.CombineStats(pasiveStats[type]);
            }
        }

        if (pasiveStats.ContainsKey(Tower.TowerTypes.All))
        {
            finalStats.CombineStats(pasiveStats[Tower.TowerTypes.All]);
        }

        return finalStats;
    }
}
