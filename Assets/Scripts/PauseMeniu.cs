using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMeniu : MonoBehaviour
{
    public void Open()
    {
        Cover.cover = true;
        gameObject.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Close()
    {
        Cover.cover = false;
        gameObject.SetActive(false);
        Time.timeScale = 1f;
    }

    public void Menu()
    {
        SceneManager.LoadScene(SceneManager.MENU);
        Time.timeScale = 1f;
    }
}
