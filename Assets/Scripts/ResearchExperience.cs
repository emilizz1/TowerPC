using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Research/Experience")]
[Serializable]

public class ResearchExperience : Research
{
    [SerializeField] int additionalExperience;

    public override void Researched()
    {
        base.Researched();
        PasiveTowerStatsController.extraExperience += additionalExperience;
    }
}
