using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterSelectionButton : MonoBehaviour
{
    [SerializeField] Image selectedBg;
    [SerializeField] Character character;
    [SerializeField] int lockedLevel;
    [SerializeField] GameObject lockScreen;
    [SerializeField] Button button;
    [SerializeField] TextMeshProUGUI level;

    bool selected;

    private void Start()
    {
        selectedBg.gameObject.SetActive(false);

        level.text = "Level " + ProgressManager.GetLevel(character.characterName);

        if(ProgressManager.GetLevel("Base") < lockedLevel)
        {
            button.interactable = false;
            lockScreen.SetActive(true);
            level.transform.parent.gameObject.SetActive(false);
        }
    }

    public void Pressed()
    {
        if (selected)
        {
            CharacterSelectionController.instance.Deselected(character);
            selected = false;
            selectedBg.gameObject.SetActive(false);
        }
        else
        {
            if (CharacterSelectionController.instance.TrySelecting(character))
            {
                selectedBg.gameObject.SetActive(true);
                selected = true;

            }
        }
    }
}
