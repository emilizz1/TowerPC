using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyCardDisplay : MonoBehaviour
{
    [SerializeField] Transform cardPos;
    [SerializeField] CardDisplay cardDisplay;

    bool cardDestroyed;

    public void DisplayCard(Card card, Vector3 startPos)
    {
        cardDisplay.transform.SetParent(transform);
        cardDisplay.transform.position = startPos;
        cardDisplay.DisplayCard(card);


        LeanTween.move(cardDisplay.gameObject, cardPos, 0.5f);
    }

    public void DestroyCard()
    {
        if (!cardDestroyed)
        {
            cardDestroyed = true;
            LeanTween.move(cardDisplay.gameObject, new Vector3(2000f, 2000f), 0.5f);
            LeanTween.rotate(cardDisplay.gameObject, new Vector3(0f, 0f, 720f), 0.3f);
            StartCoroutine(DestroyCardWindow.instance.CloseAfterTime(0.75f));
        }
    }

    public void DiscardLeftCard()
    {
        if (!cardDestroyed)
        {
            Discard.instance.DiscardCardFromHand(cardDisplay);
        }
    }
}
