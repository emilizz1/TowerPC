using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoSingleton<MenuController>
{
    public void PressedPlay()
    {
        SceneManager.LoadScene(SceneManager.CHARACTER_SELECTION);
    }

    public void PressedCollection()
    {

    }

    public void PressedLearnToPlay()
    {

    }

    public void PressedOptions()
    {

    }

    public void PressedQuit()
    {
        Application.Quit();
    }

    public void PressedMenu()
    {
        SceneManager.LoadScene(SceneManager.MENU);
    }
}
