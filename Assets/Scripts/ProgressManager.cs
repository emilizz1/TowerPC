using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ProgressManager
{
    internal static Dictionary<string, int> progress = new Dictionary<string, int>();
    internal static Dictionary<string, int> levels = new Dictionary<string, int>();

    static bool initialized;

    internal static int[] baseLevelUps = new int[] { 200, 300, 400, 550, 700, 850, 1100, 1300, 1500, 1800};
    internal static int[] characterLevelUps = new int[] { 300, 550, 800, 1100, 1500, 1750 };

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

        int levelUps = 0;

        while(progress[name] > (character? characterLevelUps[GetLevel(name) - 1 + levelUps] : baseLevelUps[GetLevel(name) - 1 + levelUps]))
        {
            progress[name] -= character ? characterLevelUps[GetLevel(name) - 1 + levelUps] : baseLevelUps[GetLevel(name) - 1 + levelUps];
            levelUps++;
        }

        if(levelUps> 0)
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
        PlayerPrefs.SetInt(name + "Level", levels[name]);
    }
}
