using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Research : ScriptableObject
{
    public Sprite sprite;
    public int timeToResearch;
    public int tier;
    public string explanation;
    public bool displayIcon;
    public ResearchType researchType;

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
        Castle,
        Charges,
        Cost,
        Currency,
        Experience,
        Hand,
        Marketplace,
        Terrain,
        Stats
    }
}
