using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PasiveTowerStatsController
{
    public static Dictionary<DamageTypes, TowerStats> pasiveStats = new Dictionary<DamageTypes, TowerStats>();

    public static int extraExperience;


    public static void AddAdditionalPasiveStats(DamageTypes type, TowerStats stats)
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

    static void AddNewStatToOldTowers(DamageTypes type, TowerStats stats)
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

    public static TowerStats GetStats(List<DamageTypes> types)
    {
        TowerStats finalStats = new TowerStats();

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

    public static void Reset()
    {
        pasiveStats = new Dictionary<DamageTypes, TowerStats>();
        extraExperience = 0;
    }
}
public enum DamageTypes
{
    Arrow,
    Magic,
    All,
    None
}
