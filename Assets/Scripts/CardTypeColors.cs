using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CardTypeColors
{
    public static Dictionary<CardType, Color> typeColors;

    static void Setup()
    {
        typeColors = new Dictionary<CardType, Color>();
        typeColors.Add(CardType.action, new Color(1f, 0.35f, 0.35f));
        typeColors.Add(CardType.spell, new Color(0.45f, 0.85f, 1f));
        typeColors.Add(CardType.tower, new Color(0.2f, 0.8f, 0.3f));
        typeColors.Add(CardType.structure, new Color(0.9f, 0.55f, 0.0f));
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
