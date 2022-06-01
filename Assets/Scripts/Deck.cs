using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Deck : MonoSingleton<Deck>
{
    [SerializeField] TextMeshProUGUI amountText;

    internal List<Card> deckCards;

    protected override void Awake()
    {
        base.Awake();
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
}
