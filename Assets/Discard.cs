using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Discard : MonoSingleton<Discard>
{
    [SerializeField] TextMeshProUGUI amountText;
    [SerializeField] GameObject cardDisplayPrefab;
    [SerializeField] Animator animator;
    public Transform discardTransform;

    internal List<Card> discardCards;

    private void Start()
    {
        discardCards = new List<Card>();
        amountText.text = discardCards.Count.ToString();
    }

    public void ShuffleDiscard()
    {
        StartCoroutine(Shuffling());
        amountText.text = discardCards.Count.ToString();
    }

    IEnumerator Shuffling()
    {
        int cardsToRemove = discardCards.Count;
        animator.SetTrigger("Shuffle");
        for (int i = 0; i < cardsToRemove; i++)
        {
            Card cardToRemove = discardCards[i];
            discardCards.Remove(cardToRemove);
            Deck.instance.AddCard(cardToRemove);
            amountText.text = discardCards.Count.ToString();
            yield return new WaitForSeconds(0.2f);
        }
    }

    public void DiscardCardFromHand(CardDisplay discardedCard)
    {
        discardedCard.transform.SetParent(discardTransform);
        //discardedCard.animator.PerformTween(1);
        discardCards.Add(discardedCard.displayedCard);
        amountText.text = discardCards.Count.ToString();
    }

    public void DiscardCard(CardDisplay discardedCard)
    {

    }

    public void AddCard(Card cardToAdd)
    {
        //Add animation for buying card
        discardCards.Add(cardToAdd);
        amountText.text = discardCards.Count.ToString();
    }
}
