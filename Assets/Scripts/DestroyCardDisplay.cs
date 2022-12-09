using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyCardDisplay : MonoBehaviour
{
    [SerializeField] Transform cardPos;
    [SerializeField] CardDisplay cardDisplay;

    bool destroyed;

    public void DisplayCard(Card card, Vector3 startPos)
    {
        cardDisplay.gameObject.SetActive(true);
        cardDisplay.transform.SetParent(transform);
        cardDisplay.transform.position = startPos;
        cardDisplay.DisplayCard(card);

        destroyed = false;

        LeanTween.move(cardDisplay.gameObject, cardPos, 0.5f);
    }

    public void DestroyCard()
    {
        destroyed = true;
        DestroyCardManager.instance.CardDestroyed();
        LeanTween.move(cardDisplay.gameObject, new Vector3(2000f, 2000f), 0.5f);
        LeanTween.rotate(cardDisplay.gameObject, new Vector3(0f, 0f, 720f), 0.3f);
        StartCoroutine(DestroyCardWindow.instance.CloseAfterTime(0.75f));
    }

    public void DiscardLeftCard()
    {
        if (!destroyed)
        {
            Discard.instance.DiscardCardFromHand(cardDisplay);
        }
    }
}
