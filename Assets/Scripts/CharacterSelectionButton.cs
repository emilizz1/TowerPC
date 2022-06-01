using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectionButton : MonoBehaviour
{
    [SerializeField] Image selectedBg;

    bool selected;

    private void Start()
    {
        selectedBg.gameObject.SetActive(false);
    }

    public void Pressed()
    {
        if (selected)
        {
            CharacterSelectionController.instance.Deselected();
            selected = false;
            selectedBg.gameObject.SetActive(false);
        }
        else
        {
            if (CharacterSelectionController.instance.TrySelecting())
            {
                selectedBg.gameObject.SetActive(true);
                selected = true;

            }
        }
    }
}
