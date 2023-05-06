using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SavedData
{
    public static SavesData savesData = new SavesData();

    private const string FILENAME = "/SteamCloud_TowersDeck.sav";

    public static void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream stream = new FileStream(Application.persistentDataPath + FILENAME, FileMode.Create);

        bf.Serialize(stream, savesData);
        stream.Close();
    }

    public static void Load()
    {
        if (File.Exists(Application.persistentDataPath + FILENAME))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = new FileStream(Application.persistentDataPath + FILENAME, FileMode.Open);

            savesData = bf.Deserialize(stream) as SavesData;

            stream.Close();

        }
        else
        {
            TryToGatherDataFromPlayerPrefs();
            Debug.LogError("File not found.");  
        }
    }

    static void TryToGatherDataFromPlayerPrefs()
    {
        savesData = new SavesData();

        savesData.wins = PlayerPrefs.GetInt("Win", 0);
        savesData.gamesPlayed = PlayerPrefs.GetInt("GamesPlayed", 0);
        savesData.difficulty = PlayerPrefs.GetInt("Difficulty", 0);
        savesData.difficultiesUnlocked = PlayerPrefs.GetInt("DifficultiesUnlocked", 0);

        savesData.baseProgress = PlayerPrefs.GetInt("BaseProgress", 0);
        savesData.knightProgress = PlayerPrefs.GetInt("KnightProgress", 0);
        savesData.mageProgress = PlayerPrefs.GetInt("MageProgress", 0);
        savesData.admiralProgress = PlayerPrefs.GetInt("AdmiralProgress", 0);

        savesData.baseLevel = PlayerPrefs.GetInt("BaseLevel", 1);
        savesData.knightLevel = PlayerPrefs.GetInt("KnightLevel", 1);
        savesData.mageLevel = PlayerPrefs.GetInt("MageLevel", 1);
        savesData.admiralLevel = PlayerPrefs.GetInt("AdmiralLevel", 1);

        savesData.tipTower = PlayerPrefs.GetInt("TipTower", 0);
        savesData.tipTerrain = PlayerPrefs.GetInt("TipTerrain", 0);
        savesData.tipCards = PlayerPrefs.GetInt("TipCards", 0);
        savesData.tipPressingExpand = PlayerPrefs.GetInt("PressingExpand", 0);
        savesData.marketSkipped = PlayerPrefs.GetInt("marketSkipped", 0);
        savesData.tipMarket = PlayerPrefs.GetInt("marketTip", 0);

        savesData.openedCardIntro = PlayerPrefs.GetInt("OpenedCardIntro",0);

        savesData.fullscreen = PlayerPrefs.GetInt("fullscreen", Screen.fullScreen ? 1 : 0);
        savesData.resolutionWidth = PlayerPrefs.GetInt("resolutionWidth", Screen.currentResolution.width);
        savesData.resolutionHeight = PlayerPrefs.GetInt("resolutionHeight", Screen.currentResolution.height);
        savesData.refreshRate = PlayerPrefs.GetInt("refreshRate", Screen.currentResolution.refreshRate);
        savesData.soundEffects = PlayerPrefs.GetFloat("soundEffects", 1);
        savesData.music = PlayerPrefs.GetFloat("music", 1);

        savesData.leftCard = "Coins0";

        Save();
    }
}


[Serializable]
public class SavesData
{
    public int wins;
    public int normalWins;
    public int hardWins;
    public int nightmareWins;
    public int gamesPlayed;
    public int difficulty;
    public int difficultiesUnlocked;

    public int baseProgress;
    public int knightProgress;
    public int mageProgress;
    public int admiralProgress;

    public int baseLevel=1;
    public int knightLevel=1;
    public int mageLevel=1;
    public int admiralLevel=1;

    public int tipTower;
    public int tipTerrain;
    public int tipCards;
    public int tipPressingExpand;
    public int marketSkipped;
    public int tipMarket;

    public int openedCardIntro;

    public int fullscreen;
    public int resolutionWidth;
    public int resolutionHeight;
    public int refreshRate;
    public float soundEffects;
    public float music;

    public string leftCard;

    public int enemyArmor;
    public int enemyBandit;
    public int enemyBoss;
    public int enemyHealth;
    public int enemyHound;
    public int enemyShield;
}
