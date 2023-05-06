     using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using I2.Loc;

public class Discard : MonoSingleton<Discard>
{
    public Transform discardTransform;

    [SerializeField] TextMeshProUGUI amountText;
    [SerializeField] GameObject cardDisplayPrefab;
    [SerializeField] Animator animator;
    [SerializeField] LocalizedString discardText;

    internal List<Card> discardCards;

    private void Start()
    {
        discardCards = new List<Card>();
        UpdateAmountText();
    }

    public void ShuffleDiscard()
    {
        SoundsController.instance.PlayOneShot("Shuffle");
        int cardsToRemove = discardCards.Count;
        animator.SetTrigger("Shuffle");
        for (int i = 0; i < cardsToRemove; i++)
        {
            Card cardToRemove = discardCards[0];
            discardCards.Remove(cardToRemove);
            Deck.instance.AddCard(cardToRemove);
        }
        UpdateAmountText();
    }

    public void UpdateAmountText()
    {
        amountText.text = discardCards.Count.ToString();
    }

    public void DiscardCardFromHand(CardDisplay discardedCard)
    {
        SoundsController.instance.PlayOneShot("Discard");
        LeanTween.move(discardedCard.gameObject, discardTransform.position, 0.25f);
        LeanTween.rotate(discardedCard.gameObject, Vector3.zero, 0.25f);
        StartCoroutine(discardedCard.ResetAfterTime(0.25f));
        discardCards.Add(discardedCard.displayedCard);
        UpdateAmountText();
    }

    public void AddCard(Card cardToAdd)
    {
        discardCards.Add(cardToAdd);
        UpdateAmountText();
    }

    public void OpenShowcase()
    {
        if (discardCards.Count > 0 && TurnController.currentPhase == TurnController.TurnPhase.Preperation)
        {
            CardShowcase.instance.Open(discardText, discardCards, ShowcasePurpose.Discard);
            SoundsController.instance.PlayOneShot("Click");
        }
    }
}
