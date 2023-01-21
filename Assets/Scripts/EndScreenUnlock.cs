using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndScreenUnlock : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] CardDisplay cardDisplay;

    public void Display(Character.LevelUpDescription levelUpDescription)
    {
        text.text = levelUpDescription.text;
        if(levelUpDescription.cardToDisplay != null)
        {
            cardDisplay.DisplayCard(levelUpDescription.cardToDisplay);
        }
        else
        {
            cardDisplay.gameObject.SetActive(false);
        }
    }
}
