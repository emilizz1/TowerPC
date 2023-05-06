using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;

public static class AchievementManager
{
    const string finishedWave = "Finished_Wave_30";
    const string allDeckUpgraded = "All_Deck_Upgraded";
    const string goldCoins = "Got_1000_Gold_From_Coins";
    const string killedEnemies = "Killed_1000_Enemies";
    const string enemiesGotThough = "100_Enemies_Got_Through";
    const string fullLife = "Finished_Run_With_Full_Life";
    const string towersOnMap = "Have_50_Towers_On_Map";
    const string castedSpells = "Casted_100_Spells";
    const string maxed = "Maxed_All_Levels";
    const string storms = "Five_Storms";

    static int trackedStat;

    const string enemiesKilledStat = "enemiesKilled";
    const string goldGotFromCoinsStat = "goldGotFromCoins";
    const string enemiesFinishedStat = "enemiesFinished";
    const string spellsCasterStat = "spellsCasted";


    static int goldGotFromCoins;
    static int enemiesKilled;
    static int enemiesFinished;
    static int spellsCasted;

    static int currentStorms;

    public static void CompleteAchievement(string name)
    {
        if (GameSettings.instance.demo || !GameSettings.instance.steam) 
        {
            return;
        }

        if (SteamManager.Initialized)
        {
            SteamUserStats.GetStat(name, out trackedStat);
            trackedStat++;
            SteamUserStats.SetStat(name, trackedStat);

            SteamUserStats.StoreStats();


            bool completed = false;
            SteamUserStats.GetAchievement(name, out completed);
            if (!completed)
            {
                SteamUserStats.SetAchievement(name);
                SteamUserStats.StoreStats();
            }
        }
    }

    public static void FinishedWave30()
    {
        if (GameSettings.instance.demo || !GameSettings.instance.steam)
        {
            return;
        }

        if (SteamManager.Initialized)
        {
            bool completed = false;
            SteamUserStats.GetAchievement(finishedWave, out completed);
            if (!completed)
            {
                SteamUserStats.SetAchievement(finishedWave);
                SteamUserStats.StoreStats();
            }
        }
    }

    public static void AllDeckUpgraded()
    {
        if (GameSettings.instance.demo || !GameSettings.instance.steam)
        {
            return;
        }

        if (SteamManager.Initialized)
        {
            bool completed = false;
            SteamUserStats.GetAchievement(allDeckUpgraded, out completed);
            if (!completed)
            {
                SteamUserStats.SetAchievement(allDeckUpgraded);
                SteamUserStats.StoreStats();
            }
        }
    }

    public static void GoldGotFromCoins(int amount)
    {
        if (GameSettings.instance.demo || !GameSettings.instance.steam)
        {
            return;
        }

        if (SteamManager.Initialized)
        {
            SteamUserStats.GetStat(goldGotFromCoinsStat, out goldGotFromCoins);
            goldGotFromCoins += amount;
            SteamUserStats.SetStat(goldGotFromCoinsStat, goldGotFromCoins);
            SteamUserStats.StoreStats();

            bool completed = false;
            SteamUserStats.GetAchievement(goldCoins, out completed);
            if (!completed && goldGotFromCoins >= 1000)
            {
                SteamUserStats.SetAchievement(goldCoins);
                SteamUserStats.StoreStats();
            }
        }
    }

    public static void KilledEnemies()
    {
        if (GameSettings.instance.demo || !GameSettings.instance.steam)
        {
            return;
        }

        if (SteamManager.Initialized)
        {
            SteamUserStats.GetStat(enemiesKilledStat, out enemiesKilled);
            enemiesKilled++;
            SteamUserStats.SetStat(enemiesKilledStat, enemiesKilled);
            SteamUserStats.StoreStats();

            bool completed = false;
            SteamUserStats.GetAchievement(killedEnemies, out completed);
            if (!completed && enemiesKilled >= 1000)
            {
                SteamUserStats.SetAchievement(killedEnemies);
                SteamUserStats.StoreStats();
            }
        }
    }

    public static void EnemiesFinished()
    {
        if (GameSettings.instance.demo || !GameSettings.instance.steam)
        {
            return;
        }

        if (SteamManager.Initialized)
        {
            SteamUserStats.GetStat(enemiesFinishedStat, out enemiesFinished);
            enemiesFinished++;
            SteamUserStats.SetStat(enemiesFinishedStat, enemiesFinished);
            SteamUserStats.StoreStats();

            bool completed = false;
            SteamUserStats.GetAchievement(enemiesGotThough, out completed);
            if (!completed && enemiesFinished >= 100)
            {
                SteamUserStats.SetAchievement(enemiesGotThough);
                SteamUserStats.StoreStats();
            }
        }
    }

    public static void FullLifeFinish()
    {
        if (GameSettings.instance.demo || !GameSettings.instance.steam)
        {
            return;
        }

        if (SteamManager.Initialized)
        {
            bool completed = false;
            SteamUserStats.GetAchievement(fullLife, out completed);
            if (!completed)
            {
                SteamUserStats.SetAchievement(fullLife);
                SteamUserStats.StoreStats();
            }
        }
    }

    public static void TowersOnMap()
    {
        if (GameSettings.instance.demo || !GameSettings.instance.steam)
        {
            return;
        }

        if (SteamManager.Initialized)
        {
            bool completed = false;
            SteamUserStats.GetAchievement(towersOnMap, out completed);
            if (!completed)
            {
                SteamUserStats.SetAchievement(towersOnMap);
                SteamUserStats.StoreStats();
            }
        }
    }

    public static void SpellCasted()
    {
        if (GameSettings.instance.demo || !GameSettings.instance.steam)
        {
            return;
        }

        if (SteamManager.Initialized)
        {
            SteamUserStats.GetStat(spellsCasterStat, out spellsCasted);
            spellsCasted ++;
            SteamUserStats.SetStat(spellsCasterStat, spellsCasted);
            SteamUserStats.StoreStats();

            bool completed = false;
            SteamUserStats.GetAchievement(castedSpells, out completed);
            if (!completed && spellsCasted >= 100)
            {
                SteamUserStats.SetAchievement(castedSpells);
                SteamUserStats.StoreStats();
            }
        }
    }

    public static void MaxedLevels()
    {
        if (GameSettings.instance.demo || !GameSettings.instance.steam)
        {
            return;
        }

        if (SteamManager.Initialized)
        {
            bool completed = false;
            SteamUserStats.GetAchievement(maxed, out completed);
            if (!completed)
            {
                SteamUserStats.SetAchievement(maxed);
                SteamUserStats.StoreStats();
            }
        }
    }

    public static void StormPlaced()
    {
        if (GameSettings.instance.demo || !GameSettings.instance.steam)
        {
            return;
        }

        if (SteamManager.Initialized)
        {
            currentStorms++;
            bool completed = false;
            SteamUserStats.GetAchievement(storms, out completed);
            if (!completed && currentStorms >= 5)
            {
                SteamUserStats.SetAchievement(storms);
                SteamUserStats.StoreStats();
            }
        }
    }

    public static void StormRemoved()
    {
        if (GameSettings.instance.demo || !GameSettings.instance.steam)
        {
            return;
        }

        currentStorms--;
    }
}
