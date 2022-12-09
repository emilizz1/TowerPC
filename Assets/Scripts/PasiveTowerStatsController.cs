using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PasiveTowerStatsController
{
    public static Dictionary<DamageTypes, Tower.TowerStats> pasiveStats = new Dictionary<DamageTypes, Tower.TowerStats>();

    public static int extraExperience;

    public enum DamageTypes
    {
        Arrow,
        Magic,
        All,
        None
    }

    public static void AddAdditionalPasiveStats(DamageTypes type, Tower.TowerStats stats)
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

    static void AddNewStatToOldTowers(DamageTypes type, Tower.TowerStats stats)
    {

        foreach (Tower tower in TowerPlacer.allTowers)
        {
            if (type == DamageTypes.All || tower.damageTypes.Contains(type))
            {
                tower.statsMultiplayers.CombineStats(stats);
                tower.SetupRange();
            }
        }
    }

    public static Tower.TowerStats GetStats(List<DamageTypes> types)
    {
        Tower.TowerStats finalStats = new Tower.TowerStats();

        foreach(DamageTypes type in types)
        {
            if (pasiveStats.ContainsKey(type))
            {
                finalStats.CombineStats(pasiveStats[type]);
            }
        }

        if (pasiveStats.ContainsKey(DamageTypes.All))
        {
            finalStats.CombineStats(pasiveStats[DamageTypes.All]);
        }

        return finalStats;
    }
}
