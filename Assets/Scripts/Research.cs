using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using I2.Loc;

public class Research : ScriptableObject
{
    public Sprite sprite;
    public int timeToResearch;
    public int tier;
    public string researchName;
    public LocalizedString researchNameText;
    public string explanation;
    public LocalizedString explanationText;
    public bool displayIcon;
    public ResearchType researchType;

    public virtual void Initialize()
    {

    }

    public virtual void Researched()
    {
        if (displayIcon)
        {
            GlobalBuffIcons.instance.DisplayBuff(sprite, explanation);
        }
        Analytics.instance.Researched(name);
    }

    [Serializable]
    public enum ResearchType
    {
        Card,
        SecondTowerAbility,
        SingleCondition,
        CastleHealth,
        CostMultiplayer,
        Currency,
        Hand,
        Terrain,
        TowerStats
    }
}
