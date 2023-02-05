using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
    public float buyCostMultiplayer;
    public int cardLevel;
    [TextArea(15,20)]
    public string description;
    public List<PasiveTowerStatsController.DamageTypes> damageTypes;
}
