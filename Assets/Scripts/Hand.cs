using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hand : MonoSingleton<Hand>
{
    [SerializeField] Transform cardsParent;
    [SerializeField] GameObject cardDisplayPrefab;

    internal List<Card> handCards;
    internal int handSize = 5;

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

    private void CreateStartingDeck()
    {
        List<Card> firstStartingCards = GetStartingCards(ProgressManager.GetLevel(CharacterSelector.firstCharacter.characterName), CharacterSelector.firstCharacter.startingCards);
        foreach (Card card in firstStartingCards)
        {
            Deck.instance.AddCard(Instantiate(card));
        }

        List<Card> secondStartingCards = GetStartingCards(ProgressManager.GetLevel(CharacterSelector.secondCharacter.characterName), CharacterSelector.secondCharacter.startingCards);
        foreach (Card card in secondStartingCards)
        {
            Deck.instance.AddCard(Instantiate(card));
        }
    }

    List<Card> GetStartingCards(int level, CardHolder cardHolder)
    {
        List<Card> finalList = new List<Card>();
        foreach (CardHolder.CardHolderCollection collection in cardHolder.cardsCollection)
        {
            if (collection.levelFrom <= level)
            {
                finalList = collection.cards;
            }
        }

        return finalList;
    }

    public void DrawCard()
    {
        Card cardToDraw = Deck.instance.GetCardToDraw();
        if (cardToDraw == null)
        {
            return;
        }
        SoundsController.instance.PlayOneShot("Draw");
        CardDisplay cardDisplay = HandCardSlotController.instance.GetDisplay(handCards.Count);
        cardDisplay.DisplayCard(cardToDraw);
        handCards.Add(cardToDraw);
        HandCardSlotController.instance.RearrangeCardSlots();
    }

    public void DrawNewHand()
    {
        StartCoroutine(DrawNewCardsAnimation(handSize));
    }

    public void DrawNewCards(int amount)
    {
        StartCoroutine(DrawNewCardsAnimation(amount));
    }

    public IEnumerator DrawNewCardsAnimation(int cardToDraw)
    {
        yield return new WaitForSeconds(1f);

        for (int i = 0; i < cardToDraw; i++)
        {
            if (Deck.instance.deckCards.Count == 0)
            {
                Discard.instance.ShuffleDiscard();
                yield return new WaitForSeconds(0.75f);
            }
            DrawCard();
            yield return new WaitForSeconds(0.25f);
        }
        TurnController.FinishedDrawing();
    }

    public void DiscardHand()
    {
        StartCoroutine(DiscardHandAnimation());
    }

    IEnumerator DiscardHandAnimation()
    {
        foreach (CardDisplay display in HandCardSlotController.instance.cardDisplays)
        {
            if (display.displayedCard != null)
            {
                if (display.dragging)
                {
                    display.dragging = false;
                }
                if (!display.front.activeSelf)
                {
                    //display.ReturnCardToHand();
                }
            }
        }

        yield return new WaitForSeconds(0.5f);

        foreach (CardDisplay display in HandCardSlotController.instance.cardDisplays)
        {
            if (display.displayedCard != null)
            {
                handCards.Remove(display.displayedCard);
                Discard.instance.DiscardCardFromHand(display);
                yield return new WaitForSeconds(0.2f);
            }
        }
    }

    public void DestroyCard(Card card)
    {
        handCards.Remove(card);
        HandCardSlotController.instance.RearrangeCardSlots();
    }

    public void RedrawCards()
    {
        StartCoroutine(RedrawCardsAnimation());
    }

    IEnumerator RedrawCardsAnimation()
    {
        DiscardHand();
        yield return new WaitForSeconds(0.5f + 0.2f * handCards.Count);
        DrawNewHand();
    }
}
