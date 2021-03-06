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
        int cardsToRemove = discardCards.Count;
        animator.SetTrigger("Shuffle");
        for (int i = 0; i < cardsToRemove; i++)
        {
            Card cardToRemove = discardCards[0];
            discardCards.Remove(cardToRemove);
            Deck.instance.AddCard(cardToRemove);
        }
        amountText.text = discardCards.Count.ToString();
    }

    public void DiscardCardFromHand(CardDisplay discardedCard)
    {
        LeanTween.move(discardedCard.gameObject, discardTransform.position, 0.25f);
        LeanTween.rotate(discardedCard.gameObject, Vector3.zero, 0.25f);
        StartCoroutine( discardedCard.ResetAfterTime(0.25f));
        discardCards.Add(discardedCard.displayedCard);
        amountText.text = discardCards.Count.ToString();
    }

    public void AddCard(Card cardToAdd)
    {
        //Add animation for buying card
        discardCards.Add(cardToAdd);
        amountText.text = discardCards.Count.ToString();
    }
}
