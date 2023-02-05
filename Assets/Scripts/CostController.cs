using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CostController
{
    public static Dictionary<CardType, float> playingCostMultiplayer = new Dictionary<CardType, float>();
    public static Dictionary<CardType, float> buyingMarketCostMultiplayer = new Dictionary<CardType, float>();
    public static Dictionary<CardType, float> buyingForgeCostMultiplayer = new Dictionary<CardType, float>();
    public static Dictionary<CardType, float> buyingGraveyardCostMultiplayer = new Dictionary<CardType, float>();

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

    public static void AddNewMarketBuyingCostMultiplayer(CardType type, float amount)
    {
        if (buyingMarketCostMultiplayer.ContainsKey(type))
        {
            buyingMarketCostMultiplayer[type] += amount;
        }
        else
        {
            buyingMarketCostMultiplayer.Add(type, amount);
        }
    }

    public static void AddNewForgeBuyingCostMultiplayer(CardType type, float amount)
    {
        if (buyingForgeCostMultiplayer.ContainsKey(type))
        {
            buyingForgeCostMultiplayer[type] += amount;
        }
        else
        {
            buyingForgeCostMultiplayer.Add(type, amount);
        }
    }

    public static void AddNewGraveyardBuyingCostMultiplayer(CardType type, float amount)
    {
        if (buyingGraveyardCostMultiplayer.ContainsKey(type))
        {
            buyingGraveyardCostMultiplayer[type] += amount;
        }
        else
        {
            buyingGraveyardCostMultiplayer.Add(type, amount);
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

    public static float GetMarketBuyingCostMultiplayer(CardType type)
    {
        float multiplayer = 1f;

        if (buyingMarketCostMultiplayer.ContainsKey(type))
        {
            multiplayer += buyingMarketCostMultiplayer[type];
        }

        if (buyingMarketCostMultiplayer.ContainsKey(CardType.All))
        {
            multiplayer += buyingMarketCostMultiplayer[CardType.All];
        }

        return multiplayer;
    }

    public static float GetForgeBuyingCostMultiplayer(CardType type)
    {
        float multiplayer = 1f;

        if (buyingForgeCostMultiplayer.ContainsKey(type))
        {
            multiplayer += buyingForgeCostMultiplayer[type];
        }

        if (buyingForgeCostMultiplayer.ContainsKey(CardType.All))
        {
            multiplayer += buyingForgeCostMultiplayer[CardType.All];
        }

        return multiplayer;
    }

    public static float GetGraveyardBuyingCostMultiplayer(CardType type)
    {
        float multiplayer = 1f;

        if (buyingGraveyardCostMultiplayer.ContainsKey(type))
        {
            multiplayer += buyingGraveyardCostMultiplayer[type];
        }

        if (buyingGraveyardCostMultiplayer.ContainsKey(CardType.All))
        {
            multiplayer += buyingGraveyardCostMultiplayer[CardType.All];
        }

        return multiplayer;
    }

    public static void Reset()
    {
        playingCostMultiplayer = new Dictionary<CardType, float>();
        buyingMarketCostMultiplayer = new Dictionary<CardType, float>();
        buyingForgeCostMultiplayer = new Dictionary<CardType, float>();
        buyingGraveyardCostMultiplayer = new Dictionary<CardType, float>();
        currentTurnDiscount = 0;
    }
}
