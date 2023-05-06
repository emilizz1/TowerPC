using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EventWindow : MonoSingleton<EventWindow>
{
    [SerializeField] TextMeshProUGUI eventName;
    [SerializeField] TextMeshProUGUI eventDescription;
    [SerializeField] List<TextMeshProUGUI> eventChoiceDescriptions;
    [SerializeField] List<CardDisplay> eventCardDisplays;
    [SerializeField] GameObject thirdEvent;
    [SerializeField] TweenAnimator animator;
    [SerializeField] List<Event> baseEvents;
    [SerializeField] List<Event> knightEvents;
    [SerializeField] List<Event> mageEvents;
    [SerializeField] List<Event> admiralEvents;
    [SerializeField] List<Event> hunterEvents;
    [SerializeField] List<Event> goldenCharmNextEvents;
    [SerializeField] Event marketEvent;
    [SerializeField] Event forgeEvent;
    [SerializeField] Event graveyardEvent;

    Event myEvent;
    List<Event> usableEvents;

    float characterEventChance = 0.2f;
    float pastEventPositivity;

    int turnsAfterIdol = 0;

    void Start()
    {
        usableEvents = new List<Event>();

        foreach (Event currentEvent in baseEvents)
        {
            usableEvents.Add(currentEvent);
        }

        if (CharacterSelector.firstCharacter.characterName == "Knight" || CharacterSelector.secondCharacter.characterName == "Knight")
        {
            foreach (Event currentEvent in knightEvents)
            {
                usableEvents.Add(currentEvent);
            }
        }
        if (CharacterSelector.firstCharacter.characterName == "Mage" || CharacterSelector.secondCharacter.characterName == "Mage")
        {
            foreach (Event currentEvent in mageEvents)
            {
                usableEvents.Add(currentEvent);
            }
        }
        if (CharacterSelector.firstCharacter.characterName == "Admiral" || CharacterSelector.secondCharacter.characterName == "Admiral")
        {
            foreach (Event currentEvent in admiralEvents)
            {
                usableEvents.Add(currentEvent);
            }
        }
        if (CharacterSelector.firstCharacter.characterName == "Hunter" || CharacterSelector.secondCharacter.characterName == "Hunter")
        {
            foreach (Event currentEvent in hunterEvents)
            {
                usableEvents.Add(currentEvent);
            }
        }

    }

    public void OpenEvent()
    {
        Cover.cover = true;
        animator.PerformTween(1);

        PickEvent();
        DisplayEvent();
    }

    public void OpenBaseSelection()
    {
        Cover.cover = true;
        animator.PerformTween(1);

        if (MarketWindow.instance.market)
        {
            myEvent = marketEvent;
        }
        if (MarketWindow.instance.forge)
        {
            myEvent = forgeEvent;
        }
        if (MarketWindow.instance.graveyard)
        {
            myEvent = graveyardEvent;
        }

        DisplayEvent();
    }

    void PickEvent()
    {
        if (GlobalConditionHolder.goldenCharm)
        {
            turnsAfterIdol++;
            if (turnsAfterIdol == 2)
            {
                myEvent = goldenCharmNextEvents[Random.Range(0, goldenCharmNextEvents.Count)];
                pastEventPositivity += myEvent.positivity;
                return;
            }
        }

        int index = Random.Range(0, usableEvents.Count);
        float eventPositivity = Random.Range(-10f, 10f) + pastEventPositivity;
        if (eventPositivity < -5f)
        {
            while (usableEvents[index].positivity != -1)
            {
                index++;
                if (index == usableEvents.Count)
                {
                    index = 0;
                }
            }
        }
        else if (eventPositivity > 5f)
        {
            while (usableEvents[index].positivity != 1)
            {
                index++;
                if (index == usableEvents.Count)
                {
                    index = 0;
                }
            }
        }

        myEvent = usableEvents[index];
        usableEvents.RemoveAt(index);
        pastEventPositivity += myEvent.positivity;
    }

    void DisplayEvent()
    {
        eventName.text = myEvent.nameText;
        eventDescription.text = myEvent.descriptionText;
        thirdEvent.SetActive(myEvent.eventChoiceDatas.Count == 3);
        myEvent.Preperation();

        for (int i = 0; i < myEvent.eventChoiceDatas.Count; i++)
        {
            eventChoiceDescriptions[i].text = myEvent.eventChoiceDatas[i].choiceDescription;
            if (myEvent.eventChoiceDatas[i].cardToGet != null)
            {
                eventCardDisplays[i].gameObject.SetActive(true);
                eventCardDisplays[i].DisplayCard(myEvent.eventChoiceDatas[i].cardToGet);
                eventChoiceDescriptions[i].GetComponent<RectTransform>().sizeDelta = new Vector2(420, 170);
            }
            else
            {
                eventCardDisplays[i].gameObject.SetActive(false);
                eventChoiceDescriptions[i].GetComponent<RectTransform>().sizeDelta = new Vector2(420, 490);
            }
        }
    }

    public void Close()
    {
        Cover.cover = false;
        animator.PerformTween(0);
    }

    public void EventChosen(int index)
    {
        myEvent.ChoiceMade(index);
        Close();
    }
}
