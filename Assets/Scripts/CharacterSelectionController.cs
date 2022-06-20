using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CharacterSelectionController : MonoSingleton<CharacterSelectionController>
{
    public static int MAX_SELECTED = 2;

    [SerializeField] TextMeshProUGUI selectedCount;
    [SerializeField] GameObject nextButton;



    List<Character> currentlySelected = new List<Character>();

    private void Start()
    {
        UpdateText();
        currentlySelected = new List<Character>();
    }

    public bool TrySelecting(Character character)
    {
        if(currentlySelected.Count < MAX_SELECTED)
        {
            currentlySelected.Add(character);
            UpdateText();
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Deselected(Character character)
    {
        currentlySelected.Remove(character);
        UpdateText();
    }

    void UpdateText()
    {
        selectedCount.text = "Selected " + currentlySelected + " / " + MAX_SELECTED;
        nextButton.SetActive(currentlySelected.Count == MAX_SELECTED);
    }

    public void StartGame()
    {
        CharacterSelector.firstCharacter = currentlySelected[0];
        CharacterSelector.secondCharacter = currentlySelected[1];
        SceneManager.instance.LoadScene(SceneManager.GAME);
    }
}
