using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectionButton : MonoBehaviour
{
    [SerializeField] Image selectedBg;
    [SerializeField] Character character;

    bool selected;

    private void Start()
    {
        selectedBg.gameObject.SetActive(false);
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
