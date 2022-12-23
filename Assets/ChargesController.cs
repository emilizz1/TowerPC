using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargesController : MonoBehaviour
{
    public static Dictionary<CardType, int> chargesAddition = new Dictionary<CardType, int>();

    public static void AddNewChargesAddition(CardType type, int amount)
    {
        if (chargesAddition.ContainsKey(type))
        {
            chargesAddition[type] += amount;
        }
        else
        {
            chargesAddition.Add(type, amount);
        }
    }

    public static int GetChargesAddition(CardType type)
    {
        int multiplayer = 0;

        if (chargesAddition.ContainsKey(type))
        {
            multiplayer += chargesAddition[type];
        }

        if (chargesAddition.ContainsKey(CardType.all))
        {
            multiplayer += chargesAddition[CardType.all];
        }

        return multiplayer;
    }
}
