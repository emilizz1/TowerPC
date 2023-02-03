using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CardShowcase : MonoSingleton<CardShowcase>
{
    [SerializeField] List<CardDisplay> cardDisplays;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TweenAnimator animator;

    bool opened;

    public void Open(string name, List<Card> cardsToDisplay)
    {
        if (!opened)
        {
            opened = true; 
            animator.PerformTween(1);
            nameText.text = name;

            for (int i = 0; i < cardDisplays.Count; i++)
            {
                if(i < cardsToDisplay.Count)
                {
                    cardDisplays[i].gameObject.SetActive(true);
                    cardDisplays[i].DisplayCard(cardsToDisplay[i]);
                }
                else
                {
                    cardDisplays[i].gameObject.SetActive(false);
                }
            }
        }
    }

    public void Close()
    {
        if (opened)
        {
            opened = false;
            animator.PerformTween(0);
        }
    }
    
}
