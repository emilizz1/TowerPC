using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ProgressManager
{
    internal static Dictionary<string, int> progress = new Dictionary<string, int>();
    internal static Dictionary<string, int> levels = new Dictionary<string, int>();

    static bool initialized;

    internal static int[] baseLevelUps = new int[] { 150, 200, 300, 400, 500, 600, 700, 800, 900, 1000};
    internal static int[] characterLevelUps = new int[] {250, 400, 550, 800, 1000, 1250, 1500 };

    static void Initialize()
    {
        initialized = true;

        progress = new Dictionary<string, int>();
        levels = new Dictionary<string, int>();

        progress.Add("Base", SavedData.savesData.baseProgress);
        progress.Add("Knight", SavedData.savesData.knightProgress);
        progress.Add("Mage", SavedData.savesData.mageProgress);
        progress.Add("Admiral", SavedData.savesData.admiralProgress);

        levels.Add("Base", SavedData.savesData.baseLevel);
        levels.Add("Knight", SavedData.savesData.knightLevel);
        levels.Add("Mage", SavedData.savesData.mageLevel);
        levels.Add("Admiral", SavedData.savesData.admiralLevel);
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
            if (character ? characterLevelUps.Length >= GetLevel(name) - 1 + levelUps : baseLevelUps.Length >= GetLevel(name) - 1 + levelUps)
            {
                break;
            }
        }

        if (levelUps > 0)
        {
            ChangeLevel(name, levelUps);
        }

        switch (name)
        {
            case ("Base"):
                SavedData.savesData.baseProgress = progress[name];
                break;
            case ("Knight"):
                SavedData.savesData.knightProgress = progress[name];
                break;
            case ("Mage"):
                SavedData.savesData.mageProgress = progress[name];
                break;
            case ("Admiral"):
                SavedData.savesData.admiralProgress = progress[name];
                break;
        }
        SavedData.Save();
    }

    public static void ChangeLevel(string name, int change)
    {
        if (!initialized)
        {
            Initialize();
        }
        levels[name] += change;
        Analytics.instance.LevelUp(name, levels[name]);

        switch (name)
        {
            case ("Base"):
                SavedData.savesData.baseLevel = levels[name];
                break;
            case ("Knight"):
                SavedData.savesData.knightLevel = levels[name];
                break;
            case ("Mage"):
                SavedData.savesData.mageLevel = levels[name];
                break;
            case ("Admiral"):
                SavedData.savesData.admiralLevel = levels[name];
                break;
        }
        SavedData.Save();
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
