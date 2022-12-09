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

    public virtual void Researched()
    {
        if (displayIcon)
        {
            GlobalBuffIcons.instance.DisplayBuff(sprite, explanation);
        }
    }
}
