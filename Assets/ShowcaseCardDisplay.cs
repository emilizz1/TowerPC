using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowcaseCardDisplay : MonoBehaviour
{
    public CardDisplay cardDisplay;
    public Button button;

    internal ShowcasePurpose purpose;

    public void ButtonPressed()
    {
        SoundsController.instance.PlayOneShot("Click");
        switch (purpose)
        {
            case (ShowcasePurpose.Leave):
                RemoveCard();
                SavedData.savesData.leftCard = cardDisplay.displayedCard.cardName + cardDisplay.displayedCard.cardLevel;
                break;
            case (ShowcasePurpose.Sacrifice):
                RemoveCard();
                PlayerLife.instance.ChangeHealthAmount(4 * cardDisplay.displayedCard.cardTier);
                break;
            case (ShowcasePurpose.Remove):
                RemoveCard();
                break;
            case (ShowcasePurpose.Upgrade):
                Card newCard = CardHolderManager.instance.GetUpgradedCard(cardDisplay.displayedCard);
                Deck.instance.AddCard(Instantiate( newCard));
                ShowCardAnimation.instance.ShowCard(newCard, true);
                RemoveCard();
                break;
            case (ShowcasePurpose.Duplicate):
                Deck.instance.AddCard(Instantiate(cardDisplay.displayedCard));
                ShowCardAnimation.instance.ShowCard(cardDisplay.displayedCard, true);
                break;
        }

        CardShowcase.instance.Close();
    }

    private void RemoveCard()
    {
        if (Deck.instance.deckCards.Contains(cardDisplay.displayedCard))
        {
            Deck.instance.deckCards.Remove(cardDisplay.displayedCard);
            Deck.instance.UpdateAmountText();
        }
        else if (Discard.instance.discardCards.Contains(cardDisplay.displayedCard))
        {
            Discard.instance.discardCards.Remove(cardDisplay.displayedCard);
            Discard.instance.UpdateAmountText();
        }
    }

    private void OnMouseEnter()
    {
        transform.localScale = new Vector3(1.05f, 1.05f, 1.05f);
    }

    private void OnMouseExit()
    {
        transform.localScale = Vector3.one;

    }
}
