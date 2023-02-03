using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoSingleton<MenuController>
{
    private void Start()
    {
        Soundtrack.instance.MenuScreen();
    }

    public void PressedPlay()
    {
        SceneManager.LoadScene(SceneManager.CHARACTER_SELECTION);
    }

    public void PressedQuit()
    {
        Application.Quit();
    }

    public void PressedMenu()
    {
        SceneManager.LoadScene(SceneManager.MENU);
    }

    public void PressedResetProgress()
    {
        ProgressManager.ResetProgress();
    }
}
