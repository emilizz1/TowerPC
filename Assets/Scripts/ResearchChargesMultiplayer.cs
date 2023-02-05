using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Research/ChargesMultiplayer")]
[Serializable]
public class ResearchChargesMultiplayer : Research
{
    [SerializeField] CardType type;
    [SerializeField] int amount;
    [SerializeField] ChargesUses usedFor;

    [Serializable]
    public enum ChargesUses
    {
        all,
        deck
    }

    public override void Researched()
    {
        base.Researched();

    }

}
