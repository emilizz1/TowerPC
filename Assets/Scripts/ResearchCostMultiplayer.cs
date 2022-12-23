using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Research/CostMultiplayer")]
[Serializable]

public class ResearchCostMultiplayer : Research
{
    [SerializeField] CardType type;
    [SerializeField] float amount;
    [SerializeField] CostMultipleyerApplied usedFor;


    public override void Researched()
    {
        base.Researched();
        if(usedFor == CostMultipleyerApplied.playing)
        {
            CostController.AddNewPlayingCostMultiplayer(type, amount);
        }

        if(usedFor == CostMultipleyerApplied.buying)
        {
            CostController.AddNewBuyingCostMultiplayer(type, amount);

        }
    }

}

[Serializable]
public enum CostMultipleyerApplied
{
    playing,
    buying
}
