using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CardTypeColors
{
    public static Dictionary<CardType, Color> typeColors;

    static void Setup()
    {
        typeColors = new Dictionary<CardType, Color>();
        typeColors.Add(CardType.Action, new Color(0.9f, 0.9f, 0.9f));
        typeColors.Add(CardType.Spell, new Color(1f, 1f, 0.9f));
        typeColors.Add(CardType.Tower, new Color(1f, 1f, 1f));
        typeColors.Add(CardType.Structure, new Color(1f, 1f, 0.9f));
    }

    public static Color GetColor(CardType type)
    {
        if(typeColors == null)
        {
            Setup();
        }

        return typeColors[type];
    }
}
