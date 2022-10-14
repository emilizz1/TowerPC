using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Research/Hand")]
[Serializable]

public class ResearchHand : Research
{
    [SerializeField] bool increaseHandSize;

    public override void Researched()
    {
        base.Researched();

        if (increaseHandSize)
        {
            Hand.instance.handSize++;
        }
    }
}
