using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Card Holder")]
[Serializable]
public class CardHolder : ScriptableObject
{
    public List<CardHolderCollection> cardsCollection;

    [Serializable]
    public struct CardHolderCollection
    {
        public int levelFrom;
        public List<Card> cards;

    }
}
