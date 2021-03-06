using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneManager
{
    public static string INTRO = "Intro";
    public static string MENU = "Menu";
    public static string CHARACTER_SELECTION = "CharacterSelection";
    public static string GAME = "Game";

    public static void LoadScene(string sceneToLoad)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneToLoad);
    }
}
