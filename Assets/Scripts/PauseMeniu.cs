using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMeniu : MonoBehaviour
{
    public void Open()
    {
        gameObject.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Close()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1f;
    }

    public void Menu()
    {
        SceneManager.instance.LoadScene(SceneManager.MENU);
        Time.timeScale = 1f;
    }
}
