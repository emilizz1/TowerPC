using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DifficultySelection : MonoBehaviour
{
    [SerializeField] GameObject normalLock;
    [SerializeField] GameObject hardLock;
    [SerializeField] GameObject easySelected;
    [SerializeField] GameObject normalSelected;
    [SerializeField] GameObject hardSelected;
    [SerializeField] Button normalButtton;
    [SerializeField] Button hardButton;

    private void Start()
    {
        if(ProgressManager.GetLevel("Base") >= 4)
        {
            normalLock.SetActive(false);
            normalButtton.interactable = true;
        }
        if(ProgressManager.GetLevel("Base") >= 8)
        {
            hardLock.SetActive(false);
            hardButton.interactable = true;
        }

        if(PlayerPrefs.GetInt("Difficulty" , 0) == 0)
        {
            EasyPressed();
        }
        else if (PlayerPrefs.GetInt("Difficulty", 0) == 1)
        {
            NoormalPressed();
        }
        if (PlayerPrefs.GetInt("Difficulty", 0) == 2)
        {
            HardPressed();
        }
    }

    public void EasyPressed()
    {
        easySelected.SetActive(true);
        normalSelected.SetActive(false);
        hardSelected.SetActive(false);

        CharacterSelector.difficulty = 0;

        PlayerPrefs.SetInt("Difficulty", 0);
    }

    public void NoormalPressed()
    {
        easySelected.SetActive(false);
        normalSelected.SetActive(true);
        hardSelected.SetActive(false);

        CharacterSelector.difficulty = 1;

        PlayerPrefs.SetInt("Difficulty", 1);
    }

    public void HardPressed()
    {
        easySelected.SetActive(false);
        normalSelected.SetActive(false);
        hardSelected.SetActive(true);

        CharacterSelector.difficulty = 2;

        PlayerPrefs.SetInt("Difficulty", 2);
    }
}
