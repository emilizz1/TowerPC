using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public enum CardType
{
    tower,
    action,
    spell,
    all
}

public class Card : ScriptableObject
{
    public CardType cardType;
    public Sprite cardImage;
    public int moneyCost;
    public int manaCost;
    public float buyCostMultiplayer;
    [TextArea(15,20)]
    public string description;
}
