using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroScene : MonoBehaviour
{
    void Start()
    {
        SceneManager.instance.LoadScene(SceneManager.MENU);
    }
}
