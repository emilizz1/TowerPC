using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CostController
{
    public static Dictionary<CardType, float> playingCostMultiplayer = new Dictionary<CardType, float>();
    public static Dictionary<CardType, float> buyingCostMultiplayer = new Dictionary<CardType, float>();

    public static int currentTurnDiscount;

    public static void AddNewPlayingCostMultiplayer(CardType type, float amount)
    {
        if (playingCostMultiplayer.ContainsKey(type))
        {
            playingCostMultiplayer[type] += amount;
        }
        else
        {
            playingCostMultiplayer.Add(type, amount);
        }
    }

    public static void AddNewBuyingCostMultiplayer(CardType type, float amount)
    {
        if (buyingCostMultiplayer.ContainsKey(type))
        {
            buyingCostMultiplayer[type] += amount;
        }
        else
        {
            buyingCostMultiplayer.Add(type, amount);
        }
    }

    public static float GetPlayingCostMultiplayer(CardType type)
    {
        float multiplayer = 1f;

        if (playingCostMultiplayer.ContainsKey(type))
        {
            multiplayer += playingCostMultiplayer[type];
        }

        if (playingCostMultiplayer.ContainsKey(CardType.All))
        {
            multiplayer += playingCostMultiplayer[CardType.All];
        }

        return multiplayer;
    }

    public static float GetBuyingCostMultiplayer(CardType type)
    {
        float multiplayer = 1f;

        if (buyingCostMultiplayer.ContainsKey(type))
        {
            multiplayer += buyingCostMultiplayer[type];
        }

        if (buyingCostMultiplayer.ContainsKey(CardType.All))
        {
            multiplayer += buyingCostMultiplayer[CardType.All];
        }

        return multiplayer;
    }

    public static void Reset()
    {
        playingCostMultiplayer = new Dictionary<CardType, float>();
        buyingCostMultiplayer = new Dictionary<CardType, float>();
        currentTurnDiscount = 0;
    }
}
