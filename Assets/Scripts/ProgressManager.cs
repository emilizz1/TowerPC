using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ProgressManager
{
    internal static Dictionary<string, int> progress = new Dictionary<string, int>();
    internal static Dictionary<string, int> levels = new Dictionary<string, int>();

    static bool initialized;

    internal static int[] baseLevelUps = new int[] {150, 150, 200, 250, 300, 400, 450, 700, 850, 1050, 1250, 1450, 1650, 1900, 2150, 2400, 2650, 2900, 3200, 3500 };
    internal static int[] characterLevelUps = new int[] {250, 400, 750, 1250, 1750, 2500, 3000 };

    static void Initialize()
    {
        initialized = true;

        progress = new Dictionary<string, int>();
        levels = new Dictionary<string, int>();

        progress.Add("Base", PlayerPrefs.GetInt("BaseProgress",0));
        progress.Add("Knight", PlayerPrefs.GetInt("KnightProgress",0));
        progress.Add("Mage", PlayerPrefs.GetInt("MageProgress",0));
        progress.Add("Admiral", PlayerPrefs.GetInt("AdmiralProgress",0));

        levels.Add("Base", PlayerPrefs.GetInt("BaseLevel",1));
        levels.Add("Knight", PlayerPrefs.GetInt("KnightLevel",1));
        levels.Add("Mage", PlayerPrefs.GetInt("MageLevel",1));
        levels.Add("Admiral", PlayerPrefs.GetInt("AdmiralLevel",1));
    }

    public static int GetProgress(string name)
    {
        if (!initialized)
        {
            Initialize();
        }
        return progress[name];
    }

    public static int GetLevel(string name)
    {
        if (!initialized)
        {
            Initialize();
        }
        return levels[name];
    }

    public static void ChangeProgress(string name, int change, bool character)
    {
        if (!initialized)
        {
            Initialize();
        }
        progress[name] += change;

        if (character ? (GetLevel(name) - 1) >= characterLevelUps.Length : (GetLevel(name)- 1) >= baseLevelUps.Length)
        {
            return;
        }

        int levelUps = 0;

        while (progress[name] > (character ? characterLevelUps[GetLevel(name) - 1 + levelUps] : baseLevelUps[GetLevel(name) - 1 + levelUps]))
        {
            progress[name] -= character ? characterLevelUps[GetLevel(name) - 1 + levelUps] : baseLevelUps[GetLevel(name) - 1 + levelUps];
            levelUps++;
            if (character ? characterLevelUps.Length == GetLevel(name) - 1 + levelUps : baseLevelUps.Length == GetLevel(name) - 1 + levelUps)
            {
                break;
            }
        }

        if (levelUps > 0)
        {
            ChangeLevel(name, levelUps);
        }

        PlayerPrefs.SetInt(name + "Progress", progress[name]);
    }

    public static void ChangeLevel(string name, int change)
    {
        if (!initialized)
        {
            Initialize();
        }
        levels[name] += change;
        Analytics.instance.LevelUp(name, levels[name]);
        PlayerPrefs.SetInt(name + "Level", levels[name]);
        CheckIfMaxed();
    }

    public static void ResetProgress()
    {
        PlayerPrefs.DeleteAll();
        Initialize();
    }

    public static void CheckIfMaxed()
    {
        if(GetLevel("Base")-1 >= characterLevelUps.Length &&
            GetLevel("Knight") - 1 >= characterLevelUps.Length &&
            GetLevel("Mage") - 1 >= characterLevelUps.Length &&
            GetLevel("Admiral") - 1 >= characterLevelUps.Length 
            )
        {
            AchievementManager.MaxedLevels();
        }
    }
}
