using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using I2.Loc;

public class CardShowcase : MonoSingleton<CardShowcase>
{
    [SerializeField] List<ShowcaseCardDisplay> cardDisplays;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TweenAnimator animator;
    [SerializeField] GameObject closeButton;
    [SerializeField] LocalizedString leaveCardText;
    [SerializeField] LocalizedString sacrificeCardText;

    bool opened;

    public void Open(string name, List<Card> cardsToDisplay, ShowcasePurpose showcasePurpose)
    {
        if (!opened)
        {
            opened = true; 
            animator.PerformTween(1);
            nameText.text = name;
            if(showcasePurpose == ShowcasePurpose.Leave)
            {
                nameText.text = leaveCardText;
            }

            for (int i = 0; i < cardDisplays.Count; i++)
            {
                if(i < cardsToDisplay.Count)
                {
                    cardDisplays[i].gameObject.SetActive(true);
                    cardDisplays[i].cardDisplay.DisplayCard(cardsToDisplay[i]);
                }
                else
                {
                    cardDisplays[i].gameObject.SetActive(false);
                }

                switch (showcasePurpose)
                {
                    case (ShowcasePurpose.Deck):
                        cardDisplays[i].button.interactable = false;
                        closeButton.SetActive(true);
                        break;
                    case (ShowcasePurpose.Discard):
                        cardDisplays[i].button.interactable = false;
                        closeButton.SetActive(true);
                        break;
                    case (ShowcasePurpose.Remove):
                        cardDisplays[i].button.interactable = true;
                        closeButton.SetActive(false);
                        break;
                    case (ShowcasePurpose.Leave):
                        cardDisplays[i].button.interactable = true;
                        closeButton.SetActive(false);
                        break;
                    case (ShowcasePurpose.Sacrifice):
                        cardDisplays[i].button.interactable = true;
                        closeButton.SetActive(false);
                        break;
                    case (ShowcasePurpose.Upgrade):
                        if(cardsToDisplay[i] == CardHolderManager.instance.curseCard)
                        {
                            cardDisplays[i].gameObject.SetActive(false);
                        }
                        cardDisplays[i].button.interactable = true;
                        closeButton.SetActive(false);
                        break;
                    case (ShowcasePurpose.Duplicate):
                        cardDisplays[i].button.interactable = true;
                        closeButton.SetActive(false);
                        break;
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

[Serializable] 
public enum ShowcasePurpose
{
    Deck,
    Discard,
    Remove,
    Leave,
    Sacrifice,
    Upgrade,
    Duplicate
}
