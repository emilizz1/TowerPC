using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowCardAnimation : MonoSingleton<ShowCardAnimation>
{
    [SerializeField] List<CardDisplay> cardDisplays;

    int used;
    int showing;

    public void ShowCard(Card cardToShow, bool deck)
    {
        cardDisplays[used].DisplayCard(cardToShow);

        StartCoroutine(ShowingCard(cardDisplays[used],deck));
        used++;
        if(used == cardDisplays.Count)
        {
            used = 0;
        }
    }

    IEnumerator ShowingCard(CardDisplay cardDisplay, bool deck)
    {
        yield return new WaitForSeconds(showing * 0.5f);
        showing++;
        LeanTween.scale(cardDisplay.gameObject, Vector3.one, 0.5f);
        yield return new WaitForSeconds(1f);
        if (deck)
        {
            LeanTween.move(cardDisplay.gameObject, Deck.instance.deckTransform, 0.5f);
        }
        else
        {
            LeanTween.move(cardDisplay.gameObject, Discard.instance.discardTransform, 0.5f);
        }
        LeanTween.scale(cardDisplay.gameObject, Vector3.zero, 0.5f);
        yield return new WaitForSeconds(0.5f);
        showing--;
    }
}
