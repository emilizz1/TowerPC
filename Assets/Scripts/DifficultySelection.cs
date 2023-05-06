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
        if(SavedData.savesData.difficultiesUnlocked >= 1)
        {
            normalLock.SetActive(false);
            normalButtton.interactable = true;
        }
        if(SavedData.savesData.difficultiesUnlocked >= 2)
        {
            hardLock.SetActive(false);
            hardButton.interactable = true;
        }

        if(SavedData.savesData.difficulty == 0)
        {
            EasyPressed();
        }
        else if (SavedData.savesData.difficulty == 1)
        {
            NoormalPressed();
        }
        if (SavedData.savesData.difficulty == 2)
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

        SavedData.savesData.difficulty = 0;
        SavedData.Save();
    }

    public void NoormalPressed()
    {
        easySelected.SetActive(false);
        normalSelected.SetActive(true);
        hardSelected.SetActive(false);

        CharacterSelector.difficulty = 1;

        SavedData.savesData.difficulty = 1;
        SavedData.Save();
    }

    public void HardPressed()
    {
        easySelected.SetActive(false);
        normalSelected.SetActive(false);
        hardSelected.SetActive(true);

        CharacterSelector.difficulty = 2;

        SavedData.savesData.difficulty = 2;
        SavedData.Save();
    }
}
