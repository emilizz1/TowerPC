using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using I2.Loc;


[Serializable]
public enum CardType
{
    Tower,
    Action,
    Spell,
    All,
    Structure
}

public class Card : ScriptableObject
{
    public string cardName;
    public CardType cardType;
    public Sprite cardImage;
    public int moneyCost;
    public int manaCost;
    public int cardTier;
    public int cardLevel;
    public LocalizedString descriptionText;
    [TextArea(10,10)]
    public string description;
    public List<DamageTypes> damageTypes;
    public CardTags cardTag;
    public CardCollectionType cardCollectionType;

    public virtual string GetDescription()
    {
        return descriptionText;
    }
}

[Serializable]
public enum CardTags
{
    None,
    Improvement
}

[Serializable]
public enum CardCollectionType
{
    None,
    Base,
    Knight,
    Mage,
    Admiral,
    Hunter
}
