using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CharacterSelectionController : MonoSingleton<CharacterSelectionController>
{
    public static int MAX_SELECTED = 2;

    [SerializeField] TextMeshProUGUI selectedCount;
    [SerializeField] GameObject nextButton;



    int currentlySelected;

    private void Start()
    {
        UpdateText();
    }

    public bool TrySelecting()
    {
        if(currentlySelected < MAX_SELECTED)
        {
            currentlySelected++;
            UpdateText();
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Deselected()
    {
        currentlySelected--;
        UpdateText();
    }

    void UpdateText()
    {
        selectedCount.text = "Selected " + currentlySelected + " / " + MAX_SELECTED;
        nextButton.SetActive(currentlySelected == MAX_SELECTED);
    }

    public void StartGame()
    {
        SceneManager.instance.LoadScene(SceneManager.GAME);
    }
}
