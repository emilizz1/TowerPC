using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DestroyCardManager : MonoSingleton<DestroyCardManager>
{

    [SerializeField] List<DestroyCardDisplay> destroyCardDisplays;
    [SerializeField] List<Button> destroyButtons;

    public void DisplayNewChoices()
    {
        foreach (Button button in destroyButtons)
        {
            button.interactable = true;
        }
        StartCoroutine(DisplayChoices());
    }

    IEnumerator DisplayChoices()
    {
        foreach (DestroyCardDisplay display in destroyCardDisplays)
        {
            yield return new WaitForSeconds(0.25f);
            DrawCard(display);
        }
    }

    void DrawCard(DestroyCardDisplay display)
    {
        if (Discard.instance.discardCards.Count > 0)
        {
            Card cardToDraw = Discard.instance.discardCards[Random.Range(0, Discard.instance.discardCards.Count)];
            Discard.instance.discardCards.Remove(cardToDraw);
            display.DisplayCard(cardToDraw, Discard.instance.discardTransform.position);
        }
        else if(Deck.instance.deckCards.Count > 0)
        {

            Card cardToDraw = Deck.instance.deckCards[Random.Range(0, Deck.instance.deckCards.Count)];
            Deck.instance.deckCards.Remove(cardToDraw);
            display.DisplayCard(cardToDraw, Deck.instance.deckTransform.position);
        }
    }

    public void CloseWindow()
    {
        foreach (DestroyCardDisplay display in destroyCardDisplays)
        {
            display.DiscardLeftCard();
        }
    }

    public void CardDestroyed()
    {
        foreach(Button button in destroyButtons)
        {
            button.interactable = false;
        }
    }
}
