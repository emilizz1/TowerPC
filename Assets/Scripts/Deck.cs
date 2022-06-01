using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Deck : MonoSingleton<Deck>
{
    [SerializeField] TextMeshProUGUI amountText;
    [SerializeField] ParticleSystem shuffleAnimation;

    List<Card> deckCards;

    protected override void Awake()
    {
        base.Awake();
        deckCards = new List<Card>();
    }

    public Card GetCardToDraw()
    {
        if(deckCards.Count == 0)
        {
            Discard.instance.ShuffleDiscard();
            if (deckCards.Count == 0)
            {
                return null;
            }
            shuffleAnimation.Play();
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
