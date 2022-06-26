using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hand : MonoSingleton<Hand>
{
    [SerializeField] Transform cardsParent;
    [SerializeField] GameObject cardDisplayPrefab;

    internal List<Card> handCards;

    protected override void Awake()
    {
        base.Awake();
        handCards = new List<Card>();
    }

    private void Start()
    {
        CreateStartingDeck();

        DrawNewHand();
    }

    private static void CreateStartingDeck()
    {
        foreach (Card card in CharacterSelector.firstCharacter.startingCards.cards)
        {
            Deck.instance.AddCard(Instantiate(card));
        }


        foreach (Card card in CharacterSelector.secondCharacter.startingCards.cards)
        {
            Deck.instance.AddCard(Instantiate(card));
        }
    }

    public void DrawCard()
    {
        Card cardToDraw = Deck.instance.GetCardToDraw();
        if(cardToDraw == null)
        {
            return;
        }

        CardDisplay cardDisplay = HandCardSlotController.instance.GetDisplay(handCards.Count);
        cardDisplay.DisplayCard(cardToDraw);
        handCards.Add(cardToDraw);
        HandCardSlotController.instance.RearrangeCardSlots();
    }

    public void DrawNewHand()
    {
        StartCoroutine(DiscardAndDrawNewHand());
    }

    IEnumerator DiscardAndDrawNewHand()
    {
        yield return new WaitForSeconds(1f);

        foreach (CardDisplay display in HandCardSlotController.instance.cardDisplays)
        {
            if (display.displayedCard != null)
            {
                Discard.instance.DiscardCardFromHand(display);
                handCards.Remove(display.displayedCard);
                yield return new WaitForSeconds(0.2f);
            }
        }

        yield return new WaitForSeconds(1f);

        for (int i = 0; i < 5; i++)
        {            
            DrawCard(); 
            if (Deck.instance.deckCards.Count == 0)
            {
                Discard.instance.ShuffleDiscard();
                yield return new WaitForSeconds(0.75f);
            }
            yield return new WaitForSeconds(0.25f);
        }
        TurnController.FinishedDrawing();
    }

    public void DestroyCard(Card card)
    {
        handCards.Remove(card);
        HandCardSlotController.instance.RearrangeCardSlots();
    }
}
