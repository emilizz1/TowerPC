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
    [SerializeField] CostMultipleyerAppliedShopType usedForType;


    public override void Researched()
    {
        base.Researched();
        if(usedFor == CostMultipleyerApplied.playing)
        {
            CostController.AddNewPlayingCostMultiplayer(type, amount);
        }

        if(usedFor == CostMultipleyerApplied.buying)
        {
            switch (usedForType)
            {
                case (CostMultipleyerAppliedShopType.market):
                    CostController.AddNewMarketBuyingCostMultiplayer(type, amount);
                    break;
                case (CostMultipleyerAppliedShopType.forge):
                    CostController.AddNewForgeBuyingCostMultiplayer(type, amount);
                    break;
                case (CostMultipleyerAppliedShopType.graveyard):
                    CostController.AddNewGraveyardBuyingCostMultiplayer(type, amount);
                    break;
            }

        }
    }

}

[Serializable]
public enum CostMultipleyerApplied
{
    playing,
    buying
}

[Serializable]
public enum CostMultipleyerAppliedShopType
{
    market,
    forge,
    graveyard
}
