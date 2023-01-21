using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Research/Hand")]
[Serializable]

public class ResearchHand : Research
{
    [SerializeField] int increaseHandSize;

    public override void Researched()
    {
        base.Researched();

        if (increaseHandSize > 0)
        {
            Hand.instance.handSize += increaseHandSize;
        }
    }
}
