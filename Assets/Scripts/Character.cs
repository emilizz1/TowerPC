using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Character")]
[Serializable]

public class Character : ScriptableObject
{
    public int startingMoney;
    public int startingMana;
    public int startingMaxHealth;
    public CardHolder startingCards;
    public CardHolder marketCards;
    public TechTreeHolder techTree;
    public Sprite icon;
}
