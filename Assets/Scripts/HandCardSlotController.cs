using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandCardSlotController : MonoSingleton<HandCardSlotController>
{
    public List<CardDisplay> cardDisplays;
    [SerializeField] Transform deckTransform;
    [SerializeField] float oneCardRotation;
    [SerializeField] float oneCardPositionX;
    [SerializeField] float oneCardPositionY;

    private void Start()
    {
        cardDisplays[0].gameObject.SetActive(false);
        cardDisplays[1].gameObject.SetActive(false);
        cardDisplays[2].gameObject.SetActive(false);
        cardDisplays[3].gameObject.SetActive(false);
        cardDisplays[4].gameObject.SetActive(false);
    }

    public CardDisplay GetDisplay(int index)
    {
        CardDisplay display = null;

        foreach(CardDisplay cardDisplay in cardDisplays)
        {
            if(cardDisplay.displayedCard == null)
            {
                display = cardDisplay;
            }
        }

        if(display == null)
        {
            return null;
        }

        display.transform.position = deckTransform.transform.position;
        display.transform.SetAsLastSibling();
        cardDisplays.Remove(display);
        cardDisplays.Add(display);
        return display;
    }

    public CardDisplay GetDisplayByCard(Card card)
    {
        foreach(CardDisplay display in cardDisplays)
        {
            if(display.displayedCard == card)
            {
                return display;
            }
        }
        return null;
    }

    public CardDisplay GetRandomDisplayWithUpgradableCard(CardDisplay excludeDisplay = null)
    {
        List<CardDisplay> activeDisplays = new List<CardDisplay>();
        foreach(CardDisplay display in cardDisplays)
        {
            if(excludeDisplay == display)
            {
                continue;
            }
            else if (display.gameObject.activeInHierarchy && display.displayedCard != null)
            {
                if (display.displayedCard.cardLevel < 3)
                {
                    activeDisplays.Add(display);
                }
            }
        }
        return activeDisplays[Random.Range(0, activeDisplays.Count)];
    }

    public void RearrangeCardSlots()
    {
        float startingRotation = (Hand.instance.handCards.Count - 1) / 2f * oneCardRotation;
        float startingPositionX = -(Hand.instance.handCards.Count - 1) / 2f * oneCardPositionX;
        float startingPositionY = -(Hand.instance.handCards.Count - 1) / 2f * oneCardPositionY;
        int cardPosition = 0;
        for (int i = 0; i < cardDisplays.Count; i++)
        {
            if (Hand.instance.handCards.Contains(cardDisplays[i].displayedCard))
            {
                cardDisplays[i].gameObject.SetActive(true);
                LeanTween.scale(cardDisplays[i].gameObject, Vector3.one, 0.25f);
                LeanTween.rotateLocal(cardDisplays[i].gameObject, new Vector3(0f, 0f, startingRotation - oneCardRotation * cardPosition), 0.25f);
                LeanTween.moveLocal(cardDisplays[i].gameObject, new Vector3(startingPositionX + oneCardPositionX * cardPosition,
                    startingPositionY + oneCardPositionY * cardPosition <= 0 ? startingPositionY + oneCardPositionY * cardPosition :
                    startingPositionY + (oneCardPositionY * (Hand.instance.handCards.Count - cardPosition - 1)), 0f), 0.25f);
                cardPosition++;
            }
        }
    }

    public void RearrengeCardSlotsWithSelectedCard(int index)
    {
        float startingRotation = (Hand.instance.handCards.Count + 2 - 1) / 2f * oneCardRotation;
        float startingPositionX = -(Hand.instance.handCards.Count + 2 - 1) / 2f * oneCardPositionX;
        float startingPositionY = -(Hand.instance.handCards.Count + 2 - 1) / 2f * oneCardPositionY;
        int cardPosition = 0;
        for (int i = 0; i < cardDisplays.Count; i++)
        {
            if (Hand.instance.handCards.Contains(cardDisplays[i].displayedCard))
            {
                cardDisplays[i].gameObject.SetActive(true);
                if (index == i)
                {
                    cardPosition++;
                    LeanTween.scale(cardDisplays[i].gameObject, new Vector3(1.25f, 1.25f, 1.25f), 0.25f);
                    LeanTween.moveLocal(cardDisplays[i].gameObject, new Vector3(startingPositionX + oneCardPositionX * cardPosition, 50f, 0f), 0.25f);
                    LeanTween.rotateLocal(cardDisplays[i].gameObject, Vector3.zero, 0.25f);
                    cardPosition++;
                }
                else
                {
                    LeanTween.scale(cardDisplays[i].gameObject, Vector3.one, 0.25f);
                    LeanTween.moveLocal(cardDisplays[i].gameObject, new Vector3(startingPositionX + oneCardPositionX * cardPosition,
                        startingPositionY + oneCardPositionY * cardPosition <= 0 ? startingPositionY + oneCardPositionY * cardPosition :
                        startingPositionY + (oneCardPositionY * (Hand.instance.handCards.Count - cardPosition - 1)), 0f), 0.25f);
                    LeanTween.rotateLocal(cardDisplays[i].gameObject, new Vector3(0f, 0f, startingRotation - oneCardRotation * cardPosition), 0.25f);
                }
                cardPosition++;
            }
        }
    }

    public void ResetHandSlots()
    {
        cardDisplays[0].front.SetActive(true);
        cardDisplays[1].gameObject.SetActive(true);
        cardDisplays[2].gameObject.SetActive(true);
        cardDisplays[3].gameObject.SetActive(true);
        cardDisplays[4].gameObject.SetActive(true);
    }

    public void UpdateCards()
    {
        foreach (CardDisplay display in cardDisplays)
        {
            if (display.displayedCard != null)
            {
                display.DisplayCard(display.displayedCard);
            }
        }
    }
}
