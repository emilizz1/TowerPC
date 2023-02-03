using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Deck : MonoSingleton<Deck>
{
    public Transform deckTransform;

    [SerializeField] TextMeshProUGUI amountText;
    [SerializeField] List<int> drawNewCardsCost;
    [SerializeField] TextMeshProUGUI drawNewCardsText;
    [SerializeField] Button newCardsButton;

    internal List<Card> deckCards;

    int drewNewCards = 0;

    protected override void Awake()
    {
        base.Awake();
        drawNewCardsText.text = "New Hand " + drawNewCardsCost[drewNewCards];
        deckCards = new List<Card>();
    }

    public Card GetCardToDraw()
    {
        if(deckCards.Count == 0)
        {
            if (deckCards.Count == 0)
            {
                return null;
            }
        }

        Card cardToDraw = deckCards[Random.Range(0, deckCards.Count)];
        deckCards.Remove(cardToDraw);
        amountText.text = deckCards.Count.ToString();
        return cardToDraw;
    }

    public void AddCard(Card cardToAdd)
    {
        deckCards.Add(cardToAdd);
        amountText.text = deckCards.Count.ToString();
    }

    public void PressedDrawNewCards()
    {
        SoundsController.instance.PlayOneShot("Click");
        if (Money.instance.TryPaying(drawNewCardsCost[drewNewCards]))
        {
            newCardsButton.interactable = false;
               drewNewCards++;
            drawNewCardsText.text = "New Hand " + drawNewCardsCost[drewNewCards];
            Hand.instance.RedrawCards();
            StartCoroutine(ActivateButtonAfterTime());
        }
    }

    IEnumerator ActivateButtonAfterTime()
    {
        yield return new WaitForSeconds(3f);
        newCardsButton.interactable = true;
    }

    public void OpenShowcase()
    {
        if (deckCards.Count > 0 && TurnController.currentPhase == TurnController.TurnPhase.Preperation)
        {
            CardShowcase.instance.Open("Deck", deckCards);
            SoundsController.instance.PlayOneShot("Click");
        }
    }
}
