using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using I2.Loc;

public class Deck : MonoSingleton<Deck>
{
    public Transform deckTransform;
    public int newHandCostMultiplayer = 5;

    [SerializeField] TextMeshProUGUI amountText;
    [SerializeField] TextMeshProUGUI drawNewCardsText;
    [SerializeField] LocalizedString newHandBase;
    [SerializeField] LocalizedString deckText;
    public Button newCardsButton;

    internal List<Card> deckCards;

    int drewNewCards = 0;

    protected override void Awake()
    {
        base.Awake();
        drawNewCardsText.text = newHandBase + " " + drewNewCards * newHandCostMultiplayer;
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
        UpdateAmountText();
        return cardToDraw;
    }

    public void AddCard(Card cardToAdd)
    {
        deckCards.Add(cardToAdd);
        UpdateAmountText();
    }

    public void UpdateAmountText()
    {
        amountText.text = deckCards.Count.ToString();
    }

    public void PressedDrawNewCards()
    {
        SoundsController.instance.PlayOneShot("Click");
        if (Money.instance.TryPaying(drewNewCards * newHandCostMultiplayer))
        {
            newCardsButton.interactable = false;
               drewNewCards++;
            drawNewCardsText.text = newHandBase + " " + drewNewCards * newHandCostMultiplayer;
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
            CardShowcase.instance.Open(deckText, deckCards, ShowcasePurpose.Deck);
            SoundsController.instance.PlayOneShot("Click");
        }
    }

    public void ResetNewHandCost()
    {
        drewNewCards = 0;
        drawNewCardsText.text = newHandBase + " " + drewNewCards * newHandCostMultiplayer;
    }
}
