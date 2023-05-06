using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Research/CharacterGetCard")]
[Serializable]

public class ResearchCharacterCard : ResearchGetCard
{
    [SerializeField] bool firstCharacter;

    [SerializeField] List<Card> knightCards;
    [SerializeField] List<Card> mageCards;
    [SerializeField] List<Card> admiralCards;
    [SerializeField] List<Card> hunterCards;

    public override void Initialize()
    {
        base.Initialize();
        if (firstCharacter)
        {
            cardToGet = GetCard(CharacterSelector.firstCharacter.name);
        }
        else
        {
            cardToGet = GetCard(CharacterSelector.secondCharacter.name);
        }
    }

    Card GetCard(string name)
    {
       switch(name)
        {
            case ("Knight"):
                return knightCards[UnityEngine.Random.Range(0, knightCards.Count)];
            case ("Mage"):
                return knightCards[UnityEngine.Random.Range(0, mageCards.Count)];
            case ("Admiral"):
                return knightCards[UnityEngine.Random.Range(0, admiralCards.Count)];
            case ("Hunter"):
                return knightCards[UnityEngine.Random.Range(0, hunterCards.Count)];
        }
        return null;
    }
}
